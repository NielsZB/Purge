using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float normalMovementSpeed = 7f;
    [SerializeField] float normalRotationSpeed = 20f;
    [Space(10)]
    [SerializeField] float dodgeMovementSpeed = 17.7f;
    [SerializeField] float dodgeRotationSpeed = 30f;
    [SerializeField] float dodgeDuration = 0.45f;
    [Space(10)]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] float attackRotationSpeed;
    [Space(10)]
    [SerializeField] LayerMask groundMask = new LayerMask();
    [SerializeField] float checkSpacing = 0.25f;
    [SerializeField] float checkYOffset = 0;
    [SerializeField] float checkDistance = 0.6f;

    [SerializeField, Range(0f, 1f)] float normalToUp = 0.5f;
    Rigidbody rb;
    Transform cameraTransform;
    Targeting targeting;
    Vector3 direction;
    Quaternion directionRotation;

    Vector2 input;
    float inputAmount
    {
        get
        {
            return Mathf.Clamp01(Mathf.Abs(input.x) + Mathf.Abs(input.y));
        }
    }

    float movementSpeed;
    float rotationSpeed;

    Vector3 gravity;

    Vector3 combinedGroundPosition;
    Vector3 updatedGroundPosition;

    Vector3 prevUp;
    bool grounded;

    public bool movable { get; private set; } = true;

    public float SmoothedMovementSpeed { get; private set; }

    private void SetupRigidbody()
    {
        if (!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    private void Awake()
    {
        SetupRigidbody();
        cameraTransform = Camera.main.transform;
        targeting = GetComponent<Targeting>();
        movementSpeed = normalMovementSpeed;
        rotationSpeed = normalRotationSpeed;
    }

    private void Update()
    {
        Vector3 localUp = Vector3.Lerp(GroundNormal(), Vector3.up, normalToUp);
        Vector3 LocalUpSmoothed = Vector3.Lerp(transform.up, localUp, Time.deltaTime * 3f);

        Vector3 localForward = cameraTransform.forward;
        Vector3 localSide = cameraTransform.right;

        localForward = Vector3.ProjectOnPlane(localForward, LocalUpSmoothed);
        localSide = Vector3.ProjectOnPlane(localSide, LocalUpSmoothed);

        direction = localForward * input.y + localSide * input.x;

        //direction.y = 0;

        if (targeting.HasTarget)
        {
            Vector3 correctedTargetPosition = targeting.Target.position;
            correctedTargetPosition.y = transform.position.y;

            directionRotation = Quaternion.LookRotation(correctedTargetPosition - transform.position);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                directionRotation,
                Time.deltaTime * inputAmount * rotationSpeed);
        }
        else
        {
            if (direction != Vector3.zero)
            {

                directionRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    directionRotation,
                    Time.deltaTime * inputAmount * rotationSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        grounded = GroundCheck();



        if (!grounded)
        {
            gravity += Physics.gravity * Time.deltaTime;
        }


        rb.velocity = (direction * movementSpeed * inputAmount) + gravity;
        
        updatedGroundPosition.Set(rb.position.x, CalculateAverageGroundPoint().y, rb.position.z);

        SmoothedMovementSpeed = rb.velocity.magnitude.Remap01(0, movementSpeed - 3f).Clamped01();
        if (grounded && updatedGroundPosition != rb.position)
        {
            rb.MovePosition(updatedGroundPosition);
            gravity = Vector3.zero;
        }
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position + (Vector3.up * checkYOffset), Vector3.down, checkDistance, groundMask);
    }

    private Vector3 GroundNormal()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * checkYOffset), Vector3.down, out RaycastHit hit, checkDistance, groundMask) && Vector3.Dot(Vector3.up, hit.normal) > 0.2f)
        {
            return hit.normal;
        }
        else
        {
            return Vector3.up;
        }
    }

    private bool GroundCheck(float xOffset, float zOffset, out Vector3 groundPoint)
    {
        Vector3 origin = transform.TransformPoint(xOffset, checkYOffset, zOffset);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, checkDistance, groundMask))
        {
            Debug.DrawLine(origin, hit.point, Color.green);
            //Debug.Log("standing on" + hit.collider.gameObject, hit.collider.gameObject);
            groundPoint = hit.point;
            return true;
        }
        else
        {
            Debug.DrawRay(origin, Vector3.down * checkDistance, Color.red);

            groundPoint = Vector3.zero;
            return false;
        }
    }
    private int GroundCheckPoint(float xOffset, float zOffset)
    {
        if (GroundCheck(xOffset, zOffset, out Vector3 temporaryPoint))
        {
            combinedGroundPosition += temporaryPoint;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private Vector3 CalculateAverageGroundPoint()
    {
        int groundAverage = 1;

        GroundCheck(0, 0, out combinedGroundPosition);

        groundAverage +=
            GroundCheckPoint(checkSpacing, 0) +
            GroundCheckPoint(-checkSpacing, 0) +
            GroundCheckPoint(0, checkSpacing) +
            GroundCheckPoint(0, -checkSpacing);

        return combinedGroundPosition / groundAverage;
    }

    private IEnumerator DashActive()
    {
        movable = false;
        float t = 0;

        movementSpeed = dodgeMovementSpeed;
        rotationSpeed = dodgeRotationSpeed;

        while (t < 1)
        {
            t += Time.deltaTime / dodgeDuration;
            yield return null;
        }

        movementSpeed = normalMovementSpeed;
        rotationSpeed = normalRotationSpeed;

        movable = true;
    }

    public void Move(Vector2 input, bool dash = false)
    {
        if (movable)
        {

            if (dash)
            {
                StartCoroutine(DashActive());

                if (this.input != Vector2.zero)
                {
                    this.input = input;
                }
                else
                {
                    this.input.Set(transform.forward.x, transform.forward.z);
                }

            }
            else
            {
                this.input = input;
            }
        }
    }

    public void EnableMovement()
    {
        movable = true;
    }

    public void DisableMovement()
    {
        movable = false;
        input = Vector2.zero;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    public void ChangeSpeeds()
    {
        movementSpeed = attackMovementSpeed;
        rotationSpeed = attackRotationSpeed;
    }

    public void ResetSpeeds()
    {
        movementSpeed = normalMovementSpeed;
        rotationSpeed = normalRotationSpeed;
    }
}
