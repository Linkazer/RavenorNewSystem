using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECAH_Movement : PlayerEntityActionHandler<EC_Movement>
{
    private EC_Movement movementHandler => entityActionComponentHandled;

    [SerializeField] private CanvasGroup movementGroup;

    public override void Enable()
    {
        movementGroup.alpha = 1f;
        movementGroup.interactable = true;
        movementGroup.blocksRaycasts = true;
    }

    public override void Disable()
    {
        movementGroup.alpha = 0f;
        movementGroup.interactable = false;
        movementGroup.blocksRaycasts = false;
    }

    public override void UpdateActionAvailibility()
    {
        if(movementHandler.CanMove)
        {
            movementGroup.interactable = !isLocked;
            movementGroup.blocksRaycasts = !isLocked;
        }
        else
        {
            movementGroup.interactable = false;
            movementGroup.blocksRaycasts = false;
        }
    }

    public override void SelectAction()
    {
        if (movementHandler.CanMove)
        {
            base.SelectAction();

            InputManager.Instance.OnMouseLeftDown += UseAction;
        }
    }

    public override void UnselectAction()
    {
        InputManager.Instance.OnMouseLeftDown -= UseAction;
        base.UnselectAction();
    }
}
