using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody))]
public class PMovement : MonoBehaviour
{
    #region serialized variables

    [SerializeField, Label("Movement Speed")] float normalMovementSpeed = 12.5f;
    [SerializeField, Label("Rotation Speed")] float normalRotationSpeed = 20f;
    [SerializeField] float boostDuration = 30f;
    [SerializeField] float boostAmount = 17f;
    [System.Serializable]
    struct AbilityData
    {
        public float movementSpeed;
        public float rotationSpeed;
        public float duration;
        public WaitForSeconds waitTime;

        public void UpdateWaitTime()
        {
            waitTime = new WaitForSeconds(duration);
        }

        public AbilityData(float movementSpeed, float rotationSpeed, float duration, WaitForSeconds waitTime)
        {
            this.movementSpeed = movementSpeed;
            this.rotationSpeed = rotationSpeed;
            this.duration = duration;
            this.waitTime = waitTime;
        }
    }

    [Space(10f)]
    [SerializeField] AbilityData dodge = new AbilityData(20f, 15f, 0.25f, new WaitForSeconds(0.25f));
    [SerializeField] AbilityData swiftMinor;
    [SerializeField] AbilityData swiftMajor;

    [System.Serializable]
    struct GroundData
    {
        public float offset;

        [Space(10f)]
        public LayerMask mask;
        public float checkSpacing;
        public float checkYOffset;
        public float checkDistance;
        [HideInInspector] public Vector3 combinedPosition;

        public GroundData(float offset, LayerMask mask, float checkSpacing, float checkYOffset, float checkDistance, Vector3 combinedPosition)
        {
            this.offset = offset;
            this.mask = mask;
            this.checkSpacing = checkSpacing;
            this.checkYOffset = checkYOffset;
            this.checkDistance = checkDistance;
            this.combinedPosition = combinedPosition;
        }
    }

    [Space(10f)]
    [SerializeField] GroundData ground = new GroundData();
    #endregion

    #region public variables
    public bool IsEnabled { get; private set; }
    public bool Grounded { get; private set; }

    #endregion

    #region private variables
    Rigidbody rb;
    Transform cameraTransform;
    Transform target;
    PTargeting targetingModule;

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
    Vector3 updatedGroundPosition;

    WaitForEndOfFrame endOfFrame;
    #endregion

    #region private methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetingModule = GetComponent<PTargeting>();
        rb.useGravity = false;
        cameraTransform = Camera.main.transform;
        dodge.UpdateWaitTime();
        swiftMinor.UpdateWaitTime();
        swiftMajor.UpdateWaitTime();
        movementSpeed = normalMovementSpeed;
        rotationSpeed = normalRotationSpeed;
        EnableMovement(true);
        endOfFrame = new WaitForEndOfFrame();
    }

    private void Update()
    {
        direction = NormalizedCameraCorrectedInput();
        direction.y = 0;
        SetDirectionRotation();
    }

    private void FixedUpdate()
    {
        target = targetingModule.Target;
        Grounded = GroundCheck();

        if (!Grounded)
        {
            gravity += Physics.gravity * Time.deltaTime;
        }

        rb.velocity = (direction * movementSpeed * inputAmount) + gravity;

        updatedGroundPosition.Set(rb.position.x, FindGround().y + ground.offset, rb.position.z);

        if (Grounded && updatedGroundPosition != rb.position)
        {
            rb.MovePosition(updatedGroundPosition);
            gravity = Vector3.zero;
        }
    }

    Vector3 NormalizedCameraCorrectedInput()
    {
        Vector3 correctedVertical = input.y * cameraTransform.forward;
        Vector3 correctedHorizontal = input.x * cameraTransform.right;


        return (correctedVertical + correctedHorizontal).normalized;
    }

    void SetDirectionRotation()
    {
        if (targetingModule.IsEnabled)
        {
            Vector3 correctedTargetPosition = target.position;
            correctedTargetPosition.y = transform.position.y;
            directionRotation = Quaternion.LookRotation(correctedTargetPosition - transform.position);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                directionRotation,
                Time.fixedDeltaTime * inputAmount * rotationSpeed);
        }
        else
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
    }

    bool GroundCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.up * ground.checkYOffset), Vector3.down, out hit, ground.checkDistance, ground.mask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool GroundCheck(float xOffset, float zOffset, float raylength, out Vector3 groundPoint)
    {
        Vector3 RayOrigin = transform.TransformPoint(xOffset, ground.checkYOffset, zOffset);
        RaycastHit hit;

        Ray ray = new Ray(RayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out hit, raylength, ground.mask))
        {
            Debug.DrawLine(RayOrigin, hit.point, Color.green);
            groundPoint = hit.point;
            return true;
        }
        else
        {
            Debug.DrawRay(RayOrigin, Vector3.down * raylength, Color.red);
            groundPoint = Vector3.zero;
            return false;
        }

    }

    int AddGroundCheckPoint(float xOffset, float zOffset)
    {
        Vector3 temporaryPoint;
        if (GroundCheck(xOffset, zOffset, ground.checkDistance, out temporaryPoint))
        {
            ground.combinedPosition += temporaryPoint;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    Vector3 FindGround()
    {
        int groundAverage = 1;

        GroundCheck(0, 0, ground.checkDistance, out ground.combinedPosition);

        groundAverage +=
            AddGroundCheckPoint(ground.checkSpacing, 0) +
            AddGroundCheckPoint(-ground.checkSpacing, 0) +
            AddGroundCheckPoint(0, ground.checkSpacing) +
            AddGroundCheckPoint(0, -ground.checkSpacing);

        return ground.combinedPosition / groundAverage;

    }

    IEnumerator AbilityActive(AbilityTypes type)
    {
        IsEnabled = false;
        if (type == AbilityTypes.Dodge)
        {
            movementSpeed = dodge.movementSpeed;
            rotationSpeed = dodge.rotationSpeed;
            yield return dodge.waitTime;
        }
        else if (type == AbilityTypes.Glimmer)
        {
            movementSpeed = swiftMinor.movementSpeed;
            rotationSpeed = swiftMinor.rotationSpeed;
            yield return swiftMinor.waitTime;
        }
        else if (type == AbilityTypes.EnergizedGlimmer)
        {
            movementSpeed = swiftMajor.movementSpeed;
            rotationSpeed = swiftMajor.rotationSpeed;
            yield return swiftMajor.waitTime;
        }

        IsEnabled = true;

        movementSpeed = normalMovementSpeed;
        rotationSpeed = normalRotationSpeed;
    }

    IEnumerator TemporarySpeed()
    {
        float Temp = normalMovementSpeed;
        normalMovementSpeed += boostAmount;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / boostDuration;
            yield return endOfFrame;
        }
        normalMovementSpeed = Temp;
    }
    #endregion

    #region public methods

    public void Move(Vector2 input)
    {
        if (IsEnabled)
        {
            this.input = input;
        }
    }

    public void Move(Vector2 input, AbilityTypes type)
    {
        if (IsEnabled)
        {
            StartCoroutine(AbilityActive(type));

            if (input != Vector2.zero)
            {
                this.input.Set(input.x, input.y);
            }
            else
            {
                this.input.Set(transform.forward.x, transform.forward.z);
            }

        }
    }

    public void EnableMovement(bool state)
    {
        IsEnabled = state;
    }

    public void BoostSpeedTemporarily()
    {
        StartCoroutine(TemporarySpeed());
    }

    #endregion
}
