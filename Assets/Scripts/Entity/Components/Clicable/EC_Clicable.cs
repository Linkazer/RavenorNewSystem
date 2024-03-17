using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class EC_Clicable : EntityComponent
{
    [SerializeField] private Collider2D clicableCollider;

    [Header("Events")]
    [SerializeField] protected UnityEvent OnLeftMouseDownEvent;
    [SerializeField] protected UnityEvent OnRightMouseDownEvent;
    [SerializeField] protected UnityEvent OnMouseEnterEvent;
    [SerializeField] protected UnityEvent OnMouseExitEvent;

    public Action OnLeftMouseDown;
    public Action OnRightMouseDown;
    public Action OnMouseEnter;
    public Action OnMouseExit;

    public void MouseDown(int mouseId)
    {
        switch (mouseId)
        {
            case 0:
                OnLeftMouseDown?.Invoke();
                OnLeftMouseDownEvent?.Invoke();
                break;
            case 1:
                OnRightMouseDown?.Invoke();
                OnRightMouseDownEvent?.Invoke();
                break;
        }
    }

    public void MouseEnter()
    {
        OnMouseEnter?.Invoke();
        OnMouseEnterEvent?.Invoke();
    }

    public void MouseExit()
    {
        OnMouseExit?.Invoke();
        OnMouseExitEvent?.Invoke();
    }

    protected override void InitializeComponent()
    {
        
    }

    public override void Activate()
    {
        clicableCollider.enabled = true;
    }

    public override void Deactivate()
    {
        clicableCollider.enabled = false;
    }

    public override void StartRound()
    {
        
    }

    public override void EndRound()
    {
        
    }
}
