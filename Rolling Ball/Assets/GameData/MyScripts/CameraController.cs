
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public enum Mode
    {
        Fixed,
        SmoothFollow,
        TopView
    }

    ;
    public static CameraController Instance;
    public Mode mode = Mode.SmoothFollow;

    public Transform target;
    public Transform targetFixedPosition;

    public KeyCode changeCameraKey = KeyCode.C;

    public CameraOrbitSettings orbitSettings = new CameraOrbitSettings();
    public CameraSmoothFollowSettings smoothFollowSettings = new CameraSmoothFollowSettings();


    Transform m_transform;
    Mode m_prevMode = Mode.SmoothFollow;

    //VehicleView Values
    float VehicleDistance, VehicleHeight, VehicleViewRatio;
    private void Awake()
    {
        Instance = this;
        m_transform = GetComponent<Transform>();
        m_transform.LookAt((target.position + EndPtOffset) + Vector3.up);
    }
    
    public void ChangeCameraView()
    {
        if (mode == Mode.SmoothFollow)
        {

            mode = Mode.TopView;

        }
        else if (mode == Mode.TopView)
        {
            mode = Mode.SmoothFollow;
        }
        else
            mode++;

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(changeCameraKey))
        {
            if (mode == Mode.TopView)
                mode = Mode.Fixed;
            else
                mode++;
        }

        if (mode != m_prevMode)
        {
            ResetCamera();
            m_prevMode = mode;
        }

        switch (mode)
        {
            case Mode.Fixed:
                DoFixedCamera();
                break;

            case Mode.SmoothFollow:
                DoSmoothFollow();
                break;

            case Mode.TopView:
                DoFixedCamera();
                break;
        }


    }


    public void ResetCamera()
    {
        // Reset each camera mode

        ResetSmoothFollow();
    }


    //----------------------------------------------------------------------------------------------


    void DoFixedCamera()
    {
        Transform fixedTarget = targetFixedPosition != null ? targetFixedPosition : target;
        if (fixedTarget == null)
            return;

        m_transform.position = fixedTarget.position;
        m_transform.rotation = fixedTarget.rotation;
    }


    //----------------------------------------------------------------------------------------------


    private Vector3 m_smoothLastPos = Vector3.zero;
    private Vector3 m_smoothVelocity = Vector3.zero;
    private float m_smoothTargetAngle = 0.0f;


    void ResetSmoothFollow()
    {
        if (target == null)
            return;

        m_smoothLastPos = target.position;
        m_smoothVelocity = target.forward * 2.0f;
        m_smoothTargetAngle = target.eulerAngles.y;
    }

    public Vector3 EndPtOffset,ChangeInValue;
    public  float newViewHeight = 0.02f;
    internal void SetHightView(float Value)
    {
        newViewHeight = Value;

    }
    public void SetEndPtOffSet(Vector3 value,float SpeedVlaue)
    {
        ChangeInValue = value;
        smoothFollowSettings.velocityDamping = SpeedVlaue;
    }
    public void DoSmoothFollow()
    {
        if (isDragging)
        {
            m_orbitX += Input.GetAxis("Mouse X") * orbitSettings.horizontalSpeed;
            m_orbitY -= Input.GetAxis("Mouse Y") * orbitSettings.verticalSpeed;
            orbitSettings.distance -= Input.GetAxis("Mouse ScrollWheel") * orbitSettings.distanceSpeed;

            m_orbitY = Mathf.Clamp(m_orbitY, orbitSettings.minVerticalAngle, orbitSettings.maxVerticalAngle);
            orbitSettings.distance = Mathf.Clamp(orbitSettings.distance, orbitSettings.minDistance, orbitSettings.maxDistance);

            m_orbitDistance = Mathf.Lerp(m_orbitDistance, orbitSettings.distance, orbitSettings.distanceDamping * Time.deltaTime);
            m_transform.rotation = Quaternion.Slerp(m_transform.rotation, Quaternion.Euler(m_orbitY, m_orbitX, 0), Time.deltaTime * orbitSettings.orbitDamping);
            m_transform.position = target.position + m_transform.rotation * new Vector3(0.0f, 0.0f, -m_orbitDistance);
        }
        else
        {
            if (target&&!IsOnlyLookAt)
            {
                Vector3 updatedVelocity = ((target.position + EndPtOffset) - m_smoothLastPos) / Time.deltaTime;
                m_smoothLastPos = (target.position + EndPtOffset);

                updatedVelocity.y = 0.0f;

                if (updatedVelocity.magnitude > 1.0f)
                {
                    m_smoothVelocity = Vector3.Lerp(m_smoothVelocity, updatedVelocity, smoothFollowSettings.velocityDamping * Time.deltaTime);
                    m_smoothTargetAngle = Mathf.Atan2(m_smoothVelocity.x, m_smoothVelocity.z) * Mathf.Rad2Deg;
                }

                if (!smoothFollowSettings.followVelocity)
                    m_smoothTargetAngle = target.eulerAngles.y;

                float wantedHeight = (target.position.y + EndPtOffset.y) + smoothFollowSettings.height;
                float currentRotationAngle = m_transform.eulerAngles.y;
                float currentHeight = m_transform.position.y;

                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, m_smoothTargetAngle, smoothFollowSettings.rotationDamping * Time.deltaTime);
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, smoothFollowSettings.heightDamping * Time.deltaTime);

                Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

                m_transform.position = (target.position + EndPtOffset);
                m_transform.position -= currentRotation * Vector3.forward * smoothFollowSettings.distance;

                Vector3 t = m_transform.position;
                t.y = currentHeight;
                m_transform.position = t;
                if (!NotAllowedToLookAt)
                {
                    //m_transform.LookAt((target.position + EndPtOffset) + Vector3.up * smoothFollowSettings.height * smoothFollowSettings.viewHeightRatio);
                }
             
            }
            else
            {
                if (target)
                {
                    m_transform.LookAt((target.position + EndPtOffset) + Vector3.up * smoothFollowSettings.height * smoothFollowSettings.viewHeightRatio);
                }
            }
        }

        EndPtOffset = Vector3.MoveTowards(EndPtOffset, ChangeInValue, LerpingVectorSpeed * Time.deltaTime);
        smoothFollowSettings.viewHeightRatio = Mathf.MoveTowards(smoothFollowSettings.viewHeightRatio, newViewHeight, LerpingSingleSpeed * Time.deltaTime);
    }
    public float LerpingVectorSpeed = 10;
    public float LerpingSingleSpeed = 10;

    //----------------------------------------------------------------------------------------------
    internal bool IsOnlyLookAt, NotAllowedToLookAt;
   
    private float m_orbitX = 0.0f;
    private float m_orbitY = 0.0f;
    private float m_orbitDistance;


    void InitializeMouseOrbit()
    {
        m_orbitDistance = orbitSettings.distance;

        Vector3 angles = m_transform.eulerAngles;
        m_orbitX = angles.y;
        m_orbitY = angles.x;
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }
    public void OnEndrag()
    {
        isDragging = false;
    }
    bool isDragging;

    void DoMouseOrbit()
    {
        if (target == null)
            return;
        if (isDragging)
        {
            m_orbitX += Input.GetAxis("Mouse X") * orbitSettings.horizontalSpeed;
            m_orbitY -= Input.GetAxis("Mouse Y") * orbitSettings.verticalSpeed;
            orbitSettings.distance -= Input.GetAxis("Mouse ScrollWheel") * orbitSettings.distanceSpeed;

            m_orbitY = Mathf.Clamp(m_orbitY, orbitSettings.minVerticalAngle, orbitSettings.maxVerticalAngle);
            orbitSettings.distance = Mathf.Clamp(orbitSettings.distance, orbitSettings.minDistance, orbitSettings.maxDistance);

            m_orbitDistance = Mathf.Lerp(m_orbitDistance, orbitSettings.distance, orbitSettings.distanceDamping * Time.deltaTime);
            m_transform.rotation = Quaternion.Slerp(m_transform.rotation, Quaternion.Euler(m_orbitY, m_orbitX, 0), Time.deltaTime * orbitSettings.orbitDamping);
            m_transform.position = target.position + m_transform.rotation * new Vector3(0.0f, 0.0f, -m_orbitDistance);
        }
    }

}
