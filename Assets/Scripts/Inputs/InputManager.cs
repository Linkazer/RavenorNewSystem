using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    private Vector2 mouseScreenPosition;

    private Vector2 mouseWorldPosition;

    private IClicableObject currentClicHandlerTouched;

    private bool isMiddleMouseDown;

    public Action<Vector2> OnMouseLeftDown;
    public Action<IClicableObject> OnMouseLeftDownOnObject;
    public Action<Vector2> OnMouseRightDown;
    public Action<IClicableObject> OnMouseRightDownOnObject;
    public Action<Vector2> OnMoveCameraInput;
    public Action<Vector2> OnMouseMiddleDown;
    public Action<Vector2> OnMouseMiddleUp;
    public Action<Vector2> OnMouseScroll;

    public PlayerControl PlayerControl => playerControl;
    public InputActionReference ScrollAction => actionMouseScroll;
    public static Vector2 MouseScreenPosition => instance.mouseScreenPosition;
    public static Vector2 MousePosition => instance.mouseWorldPosition;

    protected override void OnAwake()
    {
        playerControl = new PlayerControl();
        base.OnAwake();
    }

    private void OnEnable()
    {
        playerControl.Enable();

        actionMouseMovementInput.action.performed += UpdateMousePosition;

        actionMouseLeftClicInput.action.canceled += LeftMouseInput;

        actionMouseRightClicInput.action.canceled += RightMouseInput;

        actionMouseMiddle.action.started += MiddleMouseInputDown;

        actionMouseMiddle.action.canceled += MiddleMouseInputDown;

        actionMouseScroll.action.performed += ScrollMouseInput;
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

            OnMouseScroll?.Invoke(actionMouseScroll.action.ReadValue<Vector2>().normalized);
        }
    }

    private void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mouseScreenPosition = context.ReadValue<Vector2>();

        mouseWorldPosition = usedCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void LeftMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnMouseLeftDown?.Invoke(mouseWorldPosition);

            if (currentClicHandlerTouched != null)
            {
                OnMouseLeftDownOnObject?.Invoke(currentClicHandlerTouched);

                currentClicHandlerTouched.OnMouseDown(0);
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
                OnMouseMiddleDown?.Invoke(mouseWorldPosition);
            }
        }

        if (context.canceled && isMiddleMouseDown)
        {
            isMiddleMouseDown = false;
            OnMouseMiddleUp?.Invoke(mouseWorldPosition);
        }
    }

    private void RightMouseInput(InputAction.CallbackContext context)
    {
        if (!evtSyst.IsPointerOverGameObject())
        {
            OnMouseRightDown?.Invoke(mouseWorldPosition);

            if (currentClicHandlerTouched != null)
            {
                OnMouseRightDownOnObject?.Invoke(currentClicHandlerTouched);

                currentClicHandlerTouched.OnMouseDown(1);
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

    private RaycastHit2D UpdateRaycastHitObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero, 25f, ~layerToIgnore);

        IClicableObject lastClicHandler = currentClicHandlerTouched;

        if (hit.transform != null && hit.transform.gameObject.GetComponent<IClicableObject>() != null)
        {
            currentClicHandlerTouched = hit.transform.gameObject.GetComponent<IClicableObject>();
        }
        else if (currentClicHandlerTouched != null)
        {
            currentClicHandlerTouched = null;
        }

        if (currentClicHandlerTouched != lastClicHandler)
        {
            if (lastClicHandler != null)
            {
                lastClicHandler.OnMouseExit();
            }

            if (currentClicHandlerTouched != null)
            {
                currentClicHandlerTouched.OnMouseEnter();
            }
        }

        return hit;
    }
}
