using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;

        public CameraState() { }

        public CameraState(Vector3 r)
        {
            r.x = yaw;
            r.y = pitch;
            r.z = roll;
        }

        public void LerpTowards(CameraState target, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
        }
    }

    CameraState m_TargetCameraState;
    CameraState m_InterpolatingCameraState;
    [Header("Translation Settings")]
    [Tooltip("Controls the jump force")]
    public float jumpForce = 2.0f;

    [Tooltip("Controls the height of jump")]
    public Vector3 jump = new Vector3(0, 2.0f, 0);

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Time it takes to interpolate player movement direction to rotate."), Range(0.001f, 1f)]
    public float movementLerpTime = 0.1f;
    [Tooltip("Percentage of rotation to complete after time period."), Range(0.01f, 1f)]
    public float movementLerpBasePcnt = 0.75f;

    [Header("Camera Settings")]
    [Tooltip("BoomArm component")]
    public GameObject BoomArm;

    [Header("Mesh Settings")]
    [Tooltip("MeshObject component")]
    public GameObject MeshObject;

    [Header("Item Slots")]
    [Tooltip("The bone which serves as the root for the right hand slot.")]
    public GameObject RightHand;
    [Tooltip("The bone which serves as the root for the left hand slot.")]
    public GameObject LeftHand;
    [Tooltip("The bone which serves as the root for the back slot.")]
    public GameObject Back;

    // Private variables
    private float m_MovementSpeed = 5.0f;
    private bool m_IsJumping = false;
    private bool m_IsFalling = false;
    private Rigidbody m_RigidBody;
    public Animator Animator { get { return m_Animator; } }
    private Animator m_Animator;
    private Dictionary<Slot, GameObject> ItemSlotMap = new Dictionary<Slot, GameObject>();
    private Ability m_CurrentAbility;
    

    private void Init()
    {
        // Grab component references
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = MeshObject.GetComponent<Animator>();

        // Bind events
        ActiveCollidersChanged += new Action(UpdateFalling);
    }

    private void UpdateFalling()
    {
        m_IsFalling = !IsInCollision;
        m_Animator.SetBool("IsFalling", m_IsFalling);
    }

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        m_TargetCameraState = new CameraState(BoomArm.transform.eulerAngles);
        m_InterpolatingCameraState = new CameraState(BoomArm.transform.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        CheckExit();
        GetPlayerMovement();
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        
    }

    public void UpdateAbility(Ability newAbility)
    {
        if (m_CurrentAbility)
        {
            m_CurrentAbility.Unequipped();
        }
        m_CurrentAbility = newAbility;
        newAbility?.Equipped(this);
    }

    private void GetPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_CurrentAbility?.BeginUse(this);
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_CurrentAbility?.EndUse(this);
        }
    }

    private void CheckExit()
    {
        // Exit Sample  
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    private void GetPlayerMovement()
    {
        // Determine mouse rotation
        GetInputRotation();
        // Determine jumping
        GetInputJump();
        // Determine player mesh movement
        Vector3 translateDir = GetInputTranslationDirection();
        if (translateDir.sqrMagnitude != 0)
        {
            ApplyMovementDirectionLerp(translateDir);
            // Also set the walking animation value to true
            m_Animator.SetBool("IsWalking", true);
        }
        else
        {
            m_Animator.SetBool("IsWalking", false);
        }
        Vector3 translation = translateDir * Time.deltaTime;
        translation *= m_MovementSpeed;
        gameObject.transform.Translate(translation);
    }

    private void ApplyMovementDirectionLerp(Vector3 targetDir)
    {
        Vector3 curDir = MeshObject.transform.forward;
        curDir.y = 0;
        float movementLerpPcnt = 1f - Mathf.Exp((Mathf.Log(1f - movementLerpBasePcnt) / movementLerpTime) * Time.deltaTime);
        float newDirX = Mathf.Lerp(curDir.x, targetDir.x, movementLerpPcnt);
        float newDirZ = Mathf.Lerp(curDir.z, targetDir.z, movementLerpPcnt);
        Vector3 newDir = new Vector3(newDirX, 0, newDirZ);
        float angle = AngleSigned(curDir, newDir, MeshObject.transform.up);
        MeshObject.transform.Rotate(MeshObject.transform.up, angle);
    }

    public static float AngleSigned(Vector3 from, Vector3 to, Vector3 normal)
    {
        return Mathf.Atan2(
            Vector3.Dot(normal, Vector3.Cross(from, to)),
            Vector3.Dot(from, to)) * Mathf.Rad2Deg;
    }

    private void GetInputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsFalling && !m_IsJumping)
        {
            m_Animator.SetLayerWeight(1, 0);
            m_IsJumping = true;
            m_Animator.SetBool("IsJumping", m_IsJumping);
            m_RigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        m_IsJumping = false;
        m_Animator.SetBool("IsJumping", m_IsJumping);
    }

    private void GetInputRotation()
    {
        var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

        var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

        m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
        m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;

        // Framerate-independent interpolation
        // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
        var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
        m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, rotationLerpPct);

        m_InterpolatingCameraState.UpdateTransform(BoomArm.transform);
    }

    private Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += BoomArm.transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += (-1 * BoomArm.transform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += (-1 * BoomArm.transform.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += BoomArm.transform.right;
        }
        direction.y = 0;
        direction.Normalize();
        return direction;
    }

    public bool EquipItem(GameObject item)
    {
        Equippable equip = item.GetComponent<Equippable>();
        // No component, no good
        if (!equip) return false;

        // Cleanup already equipped item
        GameObject val;
        if (ItemSlotMap.TryGetValue(equip.ItemSlot, out val))
        {
            ItemSlotMap.Remove(equip.ItemSlot);
            Destroy(val);
        }

        // Get correct slot object and attach it
        ItemSlotMap.Add(equip.ItemSlot, item);
        GameObject slotObject;
        switch(equip.ItemSlot)
        {
            case Slot.rightHand:
                slotObject = RightHand;
                break;
            case Slot.leftHand:
                slotObject = LeftHand;
                break;
            case Slot.back:
                slotObject = Back;
                break;
            default:
                Debug.LogError("[PlayerController] EquipItem, there is no corresponding item slot.");
                return false;
        }
        // ... attach it ...
        item.transform.parent = slotObject.transform;
        item.transform.localPosition = equip.PickPosition;
        item.transform.localRotation = Quaternion.Euler(equip.PickRotation);

        return true;
    }
}
