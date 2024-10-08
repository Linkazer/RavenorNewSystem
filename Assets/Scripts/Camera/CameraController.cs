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

    [Header("Debug")]
    [SerializeField] private Transform currentFocus;

    private Vector2 mouseDirection;

    private Vector2 mouseStartWorldPosition;
    private Vector2 mouseStartScreenPosition;
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

        InputManager.Instance.OnMouseMiddleDown += StartMoveFromMiddleClic;
        InputManager.Instance.OnMouseMiddleUp += EndMoveFromMiddleClic;
        InputManager.Instance.OnMouseScroll += Zoom;
    }

    private void UnsetInput()
    {
        InputManager.Instance.OnMouseMiddleDown -= StartMoveFromMiddleClic;
        InputManager.Instance.OnMouseMiddleUp -= EndMoveFromMiddleClic;
        InputManager.Instance.OnMouseScroll -= Zoom;
    }

    /// <summary>
    /// Calcul la position de la camera selon le Clic Molette.
    /// </summary>
    /// <param name="mousePosition">The mouse position.</param>
    private void MoveFromMiddleClic(Vector2 mousePosition)
    {
        mouseDirection = (mouseStartScreenPosition - mousePosition) * mouseSpeed * 0.001f;

        SetCameraPosition(mouseDirection + mouseStartWorldPosition);
    }

    /// <summary>
    /// D�finit le point de d�part du Clic Molette.
    /// </summary>
    /// <param name="mouseWorldPosition"></param>
    private void StartMoveFromMiddleClic(Vector2 mouseWorldPosition)
    {
        if (currentFocus != null)
        {
            currentFocus = null;
        }

        Debug.Log("Start move middle");

        isMouseMoving = true;
        mouseStartScreenPosition = InputManager.MouseScreenPosition;
        mouseStartWorldPosition = cameraHandler.transform.position;
    }

    private void EndMoveFromMiddleClic(Vector2 mouseWorldPosition)
    {
        isMouseMoving = false;
    }

    /// <summary>
    /// D�placement la cam�ra dans la direction voulue.
    /// </summary>
    /// <param name="direction">La direction que la camera.</param>
    private void MoveCamera(Vector2 direction)
    {
        if (direction != Vector2.zero && currentFocus != null)
        {
            currentFocus = null;
        }

        SetCameraPosition(cameraHandler.transform.position + new Vector3(direction.x, direction.y, 0) * speed * Time.unscaledDeltaTime);
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

            if (nextZoom > 1)
            {
                nextZoom = 1;
            }
            else if (nextZoom < 0)
            {
                nextZoom = 0;
            }

            targetZoom = nextZoom;
            zoomDirection = -scrollDirection.y;
        }
    }

    /// <summary>
    /// Focus la camera sur un Transform.
    /// </summary>
    /// <param name="targetTransform">Le Transform � focus.</param>
    public void SetCameraFocus(Transform targetTransform)
    {
        currentFocus = targetTransform;
    }

    /// <summary>
    /// Met la camera � la position voulue. (Prend en compte les limites de la map)
    /// </summary>
    /// <param name="position">La position voulue pour la camera.</param>
    public void SetCameraPosition(Vector2 position)
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
            MoveFromMiddleClic(InputManager.MouseScreenPosition);
        }
        else if(actionMoveCameraInput.action.ReadValue<Vector2>() != Vector2.zero)
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
    }
}
