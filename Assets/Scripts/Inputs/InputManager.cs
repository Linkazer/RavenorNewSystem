using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Handle the Inputs of the Player.
/// </summary>
public class InputManager : Singleton<InputManager>
{
    [SerializeField] public LayerMask layerToIgnore;

    [Header("Inputs")]
    [SerializeField] private InputActionReference actionMouseMovementInput;
    [SerializeField] private InputActionReference actionMouseLeftClicInput;
    [SerializeField] private InputActionReference actionMouseRightClicInput;
    [SerializeField] private InputActionReference actionMouseMiddle;
    [SerializeField] private InputActionReference actionMouseScroll;

    [Header("Datas")]
    [SerializeField] private Camera usedCamera;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private EventSystem evtSyst;

    [Header("Common interactions")]
    [SerializeField] private UICharacterBattleInformation characterBattleInformation;


    private Vector2 mouseScreenPosition;

    private Vector3 mouseScreenToWorldPosition;
    private Vector3? mouseWorldPosition;

    private EC_Clicable currentClicHandlerTouched;

    private bool isMiddleMouseDown;

    private RaycastHit raycastHited;

    public Action OnSimpleMouseLeftDown;
    public Action<Vector2> OnMouseLeftDown;
    public Action<Vector2> OnMouseLeftDownWithoutObject;
    public Action<EC_Clicable> OnMouseLeftDownOnObject;
    public Action OnSimpleMouseRightDown;
    public Action<Vector2> OnMouseRightDown;
    public Action<Vector2> OnMouseRightDownWithoutObject;
    public Action<EC_Clicable> OnMouseRightDownOnObject;
    public Action<Vector2> OnMoveCameraInput;
    public Action OnSimpleMouseMiddleDown;
    public Action<Vector2> OnMouseMiddleDown;
    public Action OnSimpleMouseMiddleUp;
    public Action<Vector2> OnMouseMiddleUp;
    public Action<Vector2> OnMouseScroll;

    public PlayerControl PlayerControl => playerControl;
    public InputActionReference ScrollAction => actionMouseScroll;
    public static Vector2 MouseScreenPosition => instance.mouseScreenPosition;
    public static Vector2? MousePosition => instance.mouseWorldPosition;

    protected override void OnAwake()
    {
        playerControl = new PlayerControl();
        base.OnAwake();
    }

    private void OnEnable()
    {
        playerControl.Enable();

        actionMouseMovementInput.action.performed += UpdateMousePosition; //Called while mouse is moving.

        actionMouseLeftClicInput.action.canceled += LeftMouseInput; //Called when the left clic is done.

        actionMouseRightClicInput.action.canceled += RightMouseInput; //Called when the right clic is done.

        actionMouseMiddle.action.started += MiddleMouseInputDown; //Called when the middle clic start.
       
        actionMouseMiddle.action.canceled += MiddleMouseInputDown; //Called when the middle clic is done.

        actionMouseScroll.action.performed += ScrollMouseInput; //Called while the middle clic is pressed.
    }

    private void OnDisable()
    {
        playerControl.Disable();

        actionMouseMovementInput.action.performed -= UpdateMousePosition;

        actionMouseLeftClicInput.action.canceled -= LeftMouseInput;

        actionMouseRightClicInput.action.canceled -= RightMouseInput;

        actionMouseMiddle.action.performed -= MiddleMouseInputDown;

        actionMouseScroll.action.performed -= ScrollMouseInput;
    }

    private void Update()
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            UpdateRaycastHitObject();
        }
    }

    private void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();

        mouseScreenToWorldPosition = usedCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void LeftMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnSimpleMouseLeftDown?.Invoke();

            if (mouseWorldPosition != null)
            {
                OnMouseLeftDown?.Invoke(mouseWorldPosition.Value);
            }

            if (currentClicHandlerTouched != null)
            {
                OnMouseLeftDownOnObject?.Invoke(currentClicHandlerTouched);

                currentClicHandlerTouched.MouseDown(0);
            }

            else if (mouseWorldPosition != null)
            {
                OnMouseLeftDownWithoutObject?.Invoke(mouseWorldPosition.Value);
            }
        }
    }

    private void MiddleMouseInputDown(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            if (context.started)
            {
                isMiddleMouseDown = true;

                OnSimpleMouseMiddleDown.Invoke();

                if (mouseWorldPosition != null)
                {
                    OnMouseMiddleDown?.Invoke(mouseWorldPosition.Value);
                }
            }
        }

        if (context.canceled && isMiddleMouseDown)
        {
            isMiddleMouseDown = false;

            OnSimpleMouseMiddleUp.Invoke();

            if (mouseWorldPosition != null)
            {
                OnMouseMiddleUp?.Invoke(mouseWorldPosition.Value);
            }
        }
    }

    private void RightMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnSimpleMouseRightDown.Invoke();

            if (mouseWorldPosition != null)
            {
                OnMouseRightDown?.Invoke(mouseWorldPosition.Value);
            }

            if (currentClicHandlerTouched != null)
            {
                OnMouseRightDownOnObject?.Invoke(currentClicHandlerTouched);

                currentClicHandlerTouched.MouseDown(1);
            }
            else if(mouseWorldPosition != null)
            {
                OnMouseRightDownWithoutObject?.Invoke(mouseWorldPosition.Value);
            }
        }
    }

    private void ScrollMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnMouseScroll?.Invoke(context.ReadValue<Vector2>());
        }
    }

    private Ray ray;

    private RaycastHit UpdateRaycastHitObject()
    {
        ray = usedCamera.ScreenPointToRay(mouseScreenPosition);

        if(Physics.Raycast(ray, out raycastHited, 5000, ~layerToIgnore))
        {
            mouseWorldPosition = raycastHited.point;
        }
        else
        {
            mouseWorldPosition = mouseScreenToWorldPosition;
        }

        EC_Clicable lastClicHandler = currentClicHandlerTouched;

        if (raycastHited.transform != null)
        {
            if (raycastHited.transform.gameObject.GetComponent<EC_Clicable>() != null)
            {
                currentClicHandlerTouched = raycastHited.transform.gameObject.GetComponent<EC_Clicable>();
            }
            else
            {
                currentClicHandlerTouched = null;
            }
        }
        else if (currentClicHandlerTouched != null)
        {
            currentClicHandlerTouched = null;
        }

        if (currentClicHandlerTouched != lastClicHandler)
        {
            if (lastClicHandler != null)
            {
                lastClicHandler.MouseExit();
            }

            if (currentClicHandlerTouched != null)
            {
                currentClicHandlerTouched.MouseEnter();
            }
        }

        return raycastHited;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(ray);
    }
}
