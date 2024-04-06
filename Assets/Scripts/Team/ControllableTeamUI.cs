using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllableTeamUI : PlayerActionHandler
{
    [SerializeField] private CanvasGroup handlerGroup;
    [SerializeField] private ControllableTeamHandler teamHandler;
    [SerializeField] private ControllableCharacterUI[] characterUis;

    [Header("Group Mode")]
    [SerializeField] private Button groupModeButton;
    [SerializeField] private Image groupModeIcon;
    [SerializeField] private VerticalLayoutGroup characterUisHolder;
    [SerializeField] private float spacingInGroupMode = 0f;
    [SerializeField] private float spacingOutGroupMode = 15f;
    [SerializeField] private Sprite groupModeInSprite;
    [SerializeField] private Sprite groupModeOutSprite;

    [Header("End Turn")]
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

    public void UE_ChangeGroupMode()
    {
        teamHandler.SetGroupMode(!teamHandler.AreCharacterGrouped);
    }

    public void SetGroupMode(bool toSet)
    {
        if(toSet)
        {
            characterUisHolder.spacing = spacingInGroupMode;
            groupModeIcon.sprite = groupModeInSprite;
        }
        else
        {
            characterUisHolder.spacing = spacingOutGroupMode;
            groupModeIcon.sprite = groupModeOutSprite;
        }
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
