using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClicableObjectTest : MonoBehaviour, IClicableObject
{
    [SerializeField] protected UnityEvent OnLeftMouseDownEvent;
    [SerializeField] protected UnityEvent OnRightMouseDownEvent;
    [SerializeField] protected UnityEvent OnMouseEnterEvent;
    [SerializeField] protected UnityEvent OnMouseExitEvent;

    UnityEvent IClicableObject.OnLeftMouseDownEvent { get { return OnLeftMouseDownEvent; } set { OnLeftMouseDownEvent = value; } }
    UnityEvent IClicableObject.OnRightMouseDownEvent { get { return OnRightMouseDownEvent; } set { OnRightMouseDownEvent = value; } }
    UnityEvent IClicableObject.OnMouseEnterEvent { get { return OnMouseEnterEvent; } set { OnMouseEnterEvent = value; } }
    UnityEvent IClicableObject.OnMouseExitEvent { get { return OnMouseExitEvent; } set { OnMouseExitEvent = value; } }
}
