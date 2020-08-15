using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Header("Camera Settings")]
    [Tooltip("BoomArm component")]
    public GameObject BoomArm;

    // Private variables
    private float m_MovementSpeed = 5.0f;

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
        // Determine player mesh movement
        Vector3 translation = GetInputTranslationDirection() * Time.deltaTime;
        translation *= m_MovementSpeed;
        // Rodrigues' Rotation formula -- TODO fix this shit
        //translation = (translation * (Mathf.Cos(m_InterpolatingCameraState.yaw))) 
        //    + (Vector3.Cross(Vector3.up, translation) * Mathf.Sin(m_InterpolatingCameraState.yaw)) 
        //    + ((Vector3.up * Vector3.Dot(Vector3.up, translation)) * (1 - Mathf.Cos(m_InterpolatingCameraState.yaw)));
        gameObject.transform.Translate(translation);
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
}
