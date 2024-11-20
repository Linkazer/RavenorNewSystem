using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script to handle the main Camera.
/// </summary>
public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform cameraHandler;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float mouseSpeed;

    [SerializeField] private bool enableEdgeCamera;
    [SerializeField] private Vector4 cameraLimit;

    [SerializeField] private InputActionReference actionMoveCameraInput;

    [Header("Zoom")]
    [SerializeField] private float zoomForce;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float startZoom = 0.5f;
    [SerializeField] private AnimationCurve zoomCurve;
    [SerializeField] private Vector2 zoomLimits;

    [SerializeField] private Vector2 zoomLimitsY;
    [SerializeField] private Vector2 zoomLimitsZ;

    [Header("Debug")]
    [SerializeField] private Transform currentFocus;

    private Vector2 mouseDirection;

    private Vector2 mouseStartWorldPosition;
    private Vector2 mouseStartScreenPosition;
    private Vector2 mouseLastScreenPosition;
    private bool isMouseMoving;

    private float targetZoom;
    private float zoomDirection;

    public Camera UsedCamera => camera;

    private float ZoomRange => zoomLimits.y - zoomLimits.x;

    private float RealTargetZoom => zoomLimits.x + (ZoomRange * zoomCurve.Evaluate(targetZoom));

    protected override void OnAwake()
    {
        base.OnAwake();

        targetZoom = startZoom;
        virtualCamera.m_Lens.OrthographicSize = RealTargetZoom;
    }

    private void Start()
    {
        if (InputManager.Instance != null)
        {
            SetInputs();
        }
        else
        {
            InputManager.WaitForInitialization += SetInputs;
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            UnsetInput();
        }
    }

    private void SetInputs()
    {
        InputManager.WaitForInitialization -= SetInputs;

        InputManager.Instance.OnSimpleMouseMiddleDown += StartRotateFromMiddleClic;
        InputManager.Instance.OnSimpleMouseMiddleUp += EndMoveFromMiddleClic;
        InputManager.Instance.OnMouseScroll += Zoom;
    }

    private void UnsetInput()
    {
        InputManager.Instance.OnSimpleMouseMiddleDown -= StartRotateFromMiddleClic;
        InputManager.Instance.OnSimpleMouseMiddleUp -= EndMoveFromMiddleClic;
        InputManager.Instance.OnMouseScroll -= Zoom;
    }

    /// <summary>
    /// Calcul la position de la camera selon le Clic Molette.
    /// </summary>
    /// <param name="mousePosition">The mouse position.</param>
    private void RotateFromMiddleClic(Vector2 mousePosition)
    {
        float rotationToApply = (mousePosition.x - mouseLastScreenPosition.x) * mouseSpeed;

        ChangeCameraRotation(rotationToApply);
    }

    /// <summary>
    /// Définit le point de départ du Clic Molette.
    /// </summary>
    private void StartRotateFromMiddleClic()
    {
        isMouseMoving = true;
        mouseStartScreenPosition = InputManager.MouseScreenPosition;
        mouseLastScreenPosition = mouseStartScreenPosition;
        mouseStartWorldPosition = cameraHandler.transform.position;
    }

    private void EndMoveFromMiddleClic()
    {
        isMouseMoving = false;
    }

    /// <summary>
    /// Déplacement la caméra dans la direction voulue.
    /// </summary>
    /// <param name="direction">La direction que la camera.</param>
    private void MoveCamera(Vector2 direction)
    {
        if (direction != Vector2.zero && currentFocus != null)
        {
            currentFocus = null;
        }

        float zDirection = 0;

        Node currenNode = Grid.Instance.GetNodeFromWorldPoint(cameraHandler.transform.position);


        if (currenNode != null)
        {
            zDirection = currenNode.WorldPosition.z - cameraHandler.transform.position.z;

            Debug.Log("Diff : " + zDirection);

            if (zDirection > 0.1f)
            {
                zDirection = 1;
            }
            else if (zDirection < -0.1f)
            {
                zDirection = -1;
            }
            else
            {
                zDirection = 0;
            }
        }

        CinemachineOrbitalTransposer orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        Vector2 calculatedDirection = Quaternion.AngleAxis(orbitalTransposer.m_XAxis.Value, Vector3.back) * direction;

        Vector3 plannedPosition = cameraHandler.transform.position + new Vector3(calculatedDirection.x, calculatedDirection.y, zDirection) * speed * Time.unscaledDeltaTime;

        SetCameraPosition(plannedPosition);
    }

    /// <summary>
    /// Zomm the camera.
    /// </summary>
    /// <param name="scrollDirection">The data of the scroll for the zoom.</param>
    private void Zoom(Vector2 scrollDirection)
    {
        if (scrollDirection.y != 0)
        {
            float nextZoom = targetZoom - (scrollDirection.normalized.y * zoomForce);

            float zZoom = zoomLimitsZ.x +(zoomLimitsZ.y - zoomLimitsZ.x) * nextZoom;
            float yZoom = zoomLimitsY.x +(zoomLimitsY.y - zoomLimitsY.x) * nextZoom;

            CinemachineOrbitalTransposer orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            orbitalTransposer.m_FollowOffset = new Vector3(orbitalTransposer.m_FollowOffset.x, yZoom, zZoom);

            targetZoom = nextZoom;

            /*if (nextZoom > 1)
            {
                nextZoom = 1;
            }
            else if (nextZoom < 0)
            {
                nextZoom = 0;
            }

            targetZoom = nextZoom;
            zoomDirection = -scrollDirection.y;*/
        }
    }

    /// <summary>
    /// Focus la camera sur un Transform.
    /// </summary>
    /// <param name="targetTransform">Le Transform à focus.</param>
    public void SetCameraFocus(Transform targetTransform)
    {
        currentFocus = targetTransform;
    }

    private void ChangeCameraRotation(float rotationToApply)
    {
        CinemachineOrbitalTransposer orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        orbitalTransposer.m_XAxis.Value += rotationToApply;
    }

    /// <summary>
    /// Met la camera à la position voulue. (Prend en compte les limites de la map)
    /// </summary>
    /// <param name="position">La position voulue pour la camera.</param>
    public void SetCameraPosition(Vector3 position)
    {
        if (position.x < cameraLimit.x)
        {
            position.x = cameraLimit.x;
        }
        else if (position.x > cameraLimit.y)
        {
            position.x = cameraLimit.y;
        }

        if (position.y < cameraLimit.z)
        {
            position.y = cameraLimit.z;
        }
        else if (position.y > cameraLimit.w)
        {
            position.y = cameraLimit.w;
        }

        cameraHandler.transform.position = position;
    }

    private void Update()
    {
        if (currentFocus != null)
        {
            SetCameraPosition(currentFocus.position);
        }

        if (isMouseMoving)
        {
            RotateFromMiddleClic(InputManager.MouseScreenPosition);
        }

        if(actionMoveCameraInput.action.ReadValue<Vector2>() != Vector2.zero)
        {
            MoveCamera(actionMoveCameraInput.action.ReadValue<Vector2>());
        }
        else if (enableEdgeCamera)
        {
            if (InputManager.MouseScreenPosition.x < 50)
            {
                MoveCamera(new Vector2(-1, 0));
            }
            else if (InputManager.MouseScreenPosition.x > Screen.width - 50)
            {
                MoveCamera(new Vector2(1, 0));
            }

            if (InputManager.MouseScreenPosition.y < 50)
            {
                MoveCamera(new Vector2(0, -1));
            }
            else if (InputManager.MouseScreenPosition.y > Screen.height - 50)
            {
                MoveCamera(new Vector2(0, 1));
            }
        }

        if (virtualCamera.m_Lens.OrthographicSize != RealTargetZoom)
        {
            if (Mathf.Abs(RealTargetZoom - virtualCamera.m_Lens.OrthographicSize) > Time.deltaTime)
            {
                virtualCamera.m_Lens.OrthographicSize += (RealTargetZoom - virtualCamera.m_Lens.OrthographicSize) * zoomSpeed * Time.deltaTime;
            }
            else
            {
                virtualCamera.m_Lens.OrthographicSize = RealTargetZoom;
            }
        }

        mouseLastScreenPosition = InputManager.MouseScreenPosition;
    }
}
