using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    #region Serialized Variables
    [System.Serializable]
    struct BaseData
    {
        public float movementSpeed;
        public float rotationSpeed;
        public LayerMask groundMask;
        public float groundOffset;

        public BaseData(float _movementSpeed, float _rotationSpeed, LayerMask _groundMask, float _groundOffset)
        {
            movementSpeed = _movementSpeed;
            rotationSpeed = _rotationSpeed;
            groundMask = _groundMask;
            groundOffset = _groundOffset;
        }
    }

    [SerializeField] BaseData settings = new BaseData(10f, 5f, 0, 0.5f);


    [System.Serializable]
    struct DodgeData
    {
        public float movementSpeed;
        public float rotationSpeed;
        public float duration;
        public WaitForSeconds dodgeTime;

        public void UpdateDodgeTime()
        {
            dodgeTime = new WaitForSeconds(duration);
        }

        public DodgeData(float _movementSpeed, float _rotationSpeed, float _duration)
        {
            movementSpeed = _movementSpeed;
            rotationSpeed = _rotationSpeed;
            duration = _duration;
            dodgeTime = new WaitForSeconds(_duration);
        }
    }

    [SerializeField] DodgeData dodgeSettings = new DodgeData(20f, 10f, 1f);

    [Space(10f)]
    [SerializeField] bool showDebug = false;
    [Header("Ground")]
    [SerializeField, ShowIf("showDebug"), Label("Check Offset")] float groundCheckOffset = 0.1f;
    [SerializeField, ShowIf("showDebug"), Label("Check Offset Y Axis")] float groundCheckYOffset = 0.5f;
    [SerializeField, ShowIf("showDebug"), Label("Check Distance")] float groundCheckDistance = 0.55f;

    #endregion

    #region Private Variables

    Rigidbody rb;
    Transform cameraTransform;

    Vector2 input = Vector2.zero;
    Vector3 correctedInput;
    float inputAmount;

    public bool InputIsEnabled { get; private set; }
    public bool MovementIsEnabled { get; private set; }


    Vector3 direction;
    Quaternion directionRotation;
    Transform target;

    Vector3 combinedGroundPosition;
    Vector3 updatedGroundPosition;
    Vector3 gravity;
    bool grounded;

    float movementSpeed = 0f;
    float rotationSpeed = 0f;
    #endregion

    #region Public Methods
    public void SetInput(Vector2 _input)
    {
        input = _input;
    }

    public void SetInput(float _xInput, float _yInput)
    {
        input.Set(_xInput, _yInput);
    }

    public void SetInputActive(bool _state)
    {
        InputIsEnabled = _state;
    }

    public void SetMovementActive(bool _state)
    {
        MovementIsEnabled = _state;
    }

    public void Dodge(Vector2 _input)
    {
        if (InputIsEnabled)
        {
            StartCoroutine(Dodging());

            if (_input != Vector2.zero)
            {
                SetInput(_input);
            }
            else
            {
                SetInput(transform.forward.x, transform.forward.z);
            }
        }
    }
    public void Dodge(float _xInput, float _yInput)
    {
        if (InputIsEnabled)
        {
            StartCoroutine(Dodging());

            if (_xInput != 0 && _yInput != 0)
            {
                SetInput(_xInput, _yInput);
            }
            else
            {
                SetInput(transform.forward.x, transform.forward.z);
            }
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    #endregion

    #region Private Methods
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        dodgeSettings.UpdateDodgeTime();
        movementSpeed = settings.movementSpeed;
        rotationSpeed = settings.rotationSpeed;

        SetInputActive(true);
        SetMovementActive(true);
    }

    private void Update()
    {
        direction = Vector3.zero;

        correctedInput = NormalizedCameraCorrectedInput();

        direction.Set(correctedInput.x, 0, correctedInput.z);

        inputAmount = Mathf.Clamp01(Mathf.Abs(input.x) + Mathf.Abs(input.y));

        SetDirectionRotation();
    }

    private void FixedUpdate()
    {
        grounded = GroundCast();

        if (!grounded)
        {
            gravity += Physics.gravity * Time.fixedDeltaTime;
        }

        rb.velocity = (direction * movementSpeed * inputAmount) + gravity;

        updatedGroundPosition.Set(rb.position.x, FindGround().y + settings.groundOffset, rb.position.z);

        if (GroundCast() && updatedGroundPosition != rb.position)
        {
            rb.MovePosition(updatedGroundPosition);
            gravity = Vector3.zero;
        }
    }

    void SetDirectionRotation()
    {
        if (target == null)
        {
            if (direction != Vector3.zero)
            {
                directionRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        directionRotation,
                        Time.fixedDeltaTime * inputAmount * rotationSpeed);
            }
        }
        else
        {
            Vector3 direction = target.position - transform.position;
            direction.y = transform.position.y;
            directionRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    directionRotation,
                    Time.fixedDeltaTime * rotationSpeed);
        }
    }

    Vector3 NormalizedCameraCorrectedInput()
    {
        Vector3 correctedVertical = input.y * cameraTransform.forward;
        Vector3 correctedHorizontal = input.x * cameraTransform.right;

        return (correctedVertical + correctedHorizontal).normalized;
    }


    Vector3 FindGround()
    {
        int groundAverage = 1;
        GroundCast(0, 0, groundCheckDistance, out combinedGroundPosition);

        groundAverage +=
            (GetGroundAverage(groundCheckOffset, 0) +
            GetGroundAverage(-groundCheckOffset, 0) +
            GetGroundAverage(0, groundCheckOffset) +
            GetGroundAverage(0, -groundCheckOffset));

        return combinedGroundPosition / groundAverage;
    }

    int GetGroundAverage(float _offsetX, float _offsetZ)
    {
        Vector3 temporaryGroundPosition;
        if (GroundCast(_offsetX, _offsetZ, groundCheckDistance, out temporaryGroundPosition))
        {
            combinedGroundPosition += temporaryGroundPosition;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    bool GroundCast(float _offsetX, float _offsetZ, float _rayLength, out Vector3 _groundPosition)
    {
        RaycastHit hit;

        Vector3 rayStartPoint = transform.TransformPoint(_offsetX, groundCheckYOffset, _offsetZ);

        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, _rayLength, settings.groundMask))
        {
            _groundPosition = hit.point;
            return true;
        }
        else
        {
            _groundPosition = Vector3.zero;
            return false;
        }
    }

    bool GroundCast()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * groundCheckYOffset), Vector3.down, groundCheckDistance, settings.groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 GroundCastPosition()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.up * groundCheckYOffset), Vector3.down, out hit, groundCheckDistance, settings.groundMask))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    IEnumerator Dodging()
    {
        InputIsEnabled = false;
        movementSpeed = dodgeSettings.movementSpeed;
        rotationSpeed = dodgeSettings.rotationSpeed;

        yield return dodgeSettings.dodgeTime;

        InputIsEnabled = true;

        movementSpeed = settings.movementSpeed;
        rotationSpeed = settings.rotationSpeed;
    }


    #endregion

    #region Debug Visualization
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (grounded)
            {
                Gizmos.color = Color.magenta;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawRay(transform.position + (Vector3.up * groundCheckYOffset), Vector3.down * groundCheckDistance);
            Gizmos.DrawRay(transform.TransformPoint(groundCheckOffset, groundCheckYOffset, 0), Vector3.down * groundCheckDistance);
            Gizmos.DrawRay(transform.TransformPoint(-groundCheckOffset, groundCheckYOffset, 0), Vector3.down * groundCheckDistance);
            Gizmos.DrawRay(transform.TransformPoint(0f, groundCheckYOffset, groundCheckOffset), Vector3.down * groundCheckDistance);
            Gizmos.DrawRay(transform.TransformPoint(0f, groundCheckYOffset, -groundCheckOffset), Vector3.down * groundCheckDistance);
        }
    }

#endif
    #endregion
}