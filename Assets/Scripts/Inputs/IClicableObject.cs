using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IClicableObject
{
    [SerializeField] protected UnityEvent OnLeftMouseDownEvent { get; set; }
    [SerializeField] protected UnityEvent OnRightMouseDownEvent { get; set; }
    [SerializeField] protected UnityEvent OnMouseEnterEvent { get; set; }
    [SerializeField] protected UnityEvent OnMouseExitEvent { get; set; }

    public void OnMouseDown(int mouseId)
    {
        Debug.Log(mouseId);
        switch (mouseId)
        {
            case 0:
                OnLeftMouseDownEvent?.Invoke();
                break;
            case 1:
                OnRightMouseDownEvent?.Invoke();
                break;
        }
    }

    public void OnMouseEnter()
    {
        OnMouseEnterEvent?.Invoke();
    }

    public void OnMouseExit()
    {
        OnMouseExitEvent?.Invoke();
    }
}
