using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECAH_Movement : PlayerEntityActionHandler<EC_Movement>
{
    private EC_Movement movementHandler => entityActionComponentHandled;

    [SerializeField] private CanvasGroup movementGroup;

    private void Update()
    {
        if (RoundManager.Instance.CurrentRoundMode == RoundMode.Round)
        {
            if (movementHandler.CanMove && !isLocked)
            {
                movementHandler.DisplayAction(InputManager.MousePosition);
            }
        }
    }

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
        enabled = true;

        if (movementHandler.CanMove)
        {
            base.SelectAction();

            InputManager.Instance.OnMouseLeftDownWithoutObject += UseAction;
            InputManager.Instance.OnMouseLeftDownOnObject += UseActionOnInteractible;
        }
    }

    public override void UnselectAction()
    {
        enabled = false;
        movementHandler.UndisplayAction();

        InputManager.Instance.OnMouseLeftDownWithoutObject -= UseAction;
        InputManager.Instance.OnMouseLeftDownOnObject -= UseActionOnInteractible;
        base.UnselectAction();
    }

    /// <summary>
    /// Handle interaction.
    /// </summary>
    /// <param name="interactibleObject"></param>
    private void UseActionOnInteractible(EC_Clicable interactibleObject)
    {
        if(interactibleObject.HoldingEntity.TryGetComponentOfType<EC_Interactable>(out EC_Interactable interactibleComponent))
        {
            UseAction(interactibleComponent.HoldingEntity.transform.position, () => interactibleComponent.PlayInteraction(EndAction));
        }
        else
        {
            EndAction();
        }
    }

    public override void UseAction(Vector2 usePosition, Action callback)
    {
        movementHandler.UndisplayAction();

        base.UseAction(usePosition, callback);
    }

    protected override void EndAction()
    {
        base.EndAction();
    }
}
