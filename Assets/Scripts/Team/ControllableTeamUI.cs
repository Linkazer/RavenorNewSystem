using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableTeamUI : PlayerActionHandler
{
    [SerializeField] private CanvasGroup handlerGroup;
    [SerializeField] private ControllableTeamHandler teamHandler;
    [SerializeField] private ControllableCharacterUI[] characterUis;
    [SerializeField] private CanvasGroup endTurnGroup;

    private List<ControllableCharacterUI> usedDisplays = new List<ControllableCharacterUI>();

    public override void Enable()
    {
        
    }

    public override void Disable()
    {
        
    }

    public override void Lock(bool doesLock)
    {
        base.Lock(doesLock);

        handlerGroup.interactable = !doesLock;
    }

    public void SelectCharacter(CharacterEntity toSelect)
    {
        teamHandler.SelectCharacter(toSelect);
    }

    public void UE_EndCharacterTurn()
    {
        teamHandler.DeactivateControllableCharacter(PlayerActionManager.Instance.EntityHandled as CharacterEntity);
    }

    public void AddCharacter(CharacterEntity character)
    {
        ControllableCharacterUI characterUi = null;

        foreach (ControllableCharacterUI chara in characterUis)
        {
            if (!usedDisplays.Contains(chara))
            {
                characterUi = chara;
                break;
            }
        }

        usedDisplays.Add(characterUi);
        characterUi.SetCharacter(character);
    }

    public void RemoveCharacter(CharacterEntity character)
    {
        ControllableCharacterUI characterUiToRemove = null;

        foreach (ControllableCharacterUI chara in characterUis)
        {
            if (chara.HoldedCharacter == character)
            {
                characterUiToRemove = chara;
                break;
            }
        }

        characterUiToRemove.UnsetCharacter();
        usedDisplays.Remove(characterUiToRemove);
    }

    public void EnableCharacter(CharacterEntity character)
    {
        foreach (ControllableCharacterUI displayChara in usedDisplays)
        {
            if (displayChara.HoldedCharacter == character)
            {
                displayChara.Enable();
            }
        }
    }

    public void DisableCharacter(CharacterEntity character)
    {
        foreach (ControllableCharacterUI displayChara in usedDisplays)
        {
            if (displayChara.HoldedCharacter == character)
            {
                displayChara.Disable();
            }
        }
    }

    public void SetEndableTurn(bool canEndTurn)
    {
        endTurnGroup.interactable = canEndTurn;
    }
}
