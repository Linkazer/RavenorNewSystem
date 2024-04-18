using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableTeamHandler : Singleton<ControllableTeamHandler>
{
    [SerializeField] private List<CharacterEntity> controllableCharacters = new List<CharacterEntity>();

    [Header("UI")]
    [SerializeField] private ControllableTeamUI teamUi;

    private bool areGrouped = true;

    private List<CharacterEntity> currentControllableCharacters = new List<CharacterEntity>();

    public List<CharacterEntity> CurrentControllableCharacters => currentControllableCharacters;

    public bool AreCharacterGrouped => areGrouped;

    public void Initialize()
    {
        if (controllableCharacters.Count > 0)
        {
            SelectCharacter(controllableCharacters[0]);
        }

        teamUi.SetGroupMode(areGrouped);

        RoundManager.Instance.actOnUpdateRoundMode += OnChangeRoundMode;
        OnChangeRoundMode(RoundManager.Instance.CurrentRoundMode);
    }

    private void OnDestroy()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.actOnUpdateRoundMode -= OnChangeRoundMode;
        }
    }

    public bool CanCharacterBeControlled(CharacterEntity character)
    {
        return controllableCharacters.Contains(character);
    }

    private void OnChangeRoundMode(RoundMode modeSet)
    {
        switch(modeSet)
        {
            case RoundMode.RealTime:
                foreach (CharacterEntity character in controllableCharacters)
                {
                    ActivateControllableCharacter(character);
                }
                teamUi.SetEndableTurn(false);
                break;
            case RoundMode.Round:
                foreach (CharacterEntity character in controllableCharacters)
                {
                    DeactivateControllableCharacter(character);
                }
                teamUi.SetEndableTurn(true);
                break;
        }
    }

    public void AddCharacter(CharacterEntity character)
    {
        if(!controllableCharacters.Contains(character))
        {
            controllableCharacters.Add(character);
            teamUi.AddCharacter(character);

            if(RoundManager.Instance.CurrentRoundMode == RoundMode.RealTime)
            {
                ActivateControllableCharacter(character);
            }

            if(controllableCharacters.Count == 1)
            {
                SelectCharacter(controllableCharacters[0]);
            }
        }
    }

    public void RemoveCharacter(CharacterEntity character)
    {
        if (controllableCharacters.Contains(character))
        {
            controllableCharacters.Remove(character);
            teamUi.RemoveCharacter(character);
        }
    }

    public void ActivateControllableCharacter(CharacterEntity character)
    {
        if (!currentControllableCharacters.Contains(character))
        {
            currentControllableCharacters.Add(character);
            teamUi.EnableCharacter(character);

            if(currentControllableCharacters.Count == 1)
            {
                SelectCharacter(currentControllableCharacters[0]);
            }
        }
    }

    public void DeactivateControllableCharacter(CharacterEntity character)
    {
        if (currentControllableCharacters.Contains(character))
        {
            currentControllableCharacters.Remove(character);
            teamUi.DisableCharacter(character);

            if (PlayerActionManager.Instance.EntityHandled == character)
            {
                if (RoundManager.instance.CurrentRoundMode == RoundMode.Round)
                {
                    BattleRoundManager.Instance.EndCharacterTurn(character);
                }

                if (currentControllableCharacters.Count > 0)
                {
                    PlayerActionManager.Instance.SelectEntity(currentControllableCharacters[0]);
                }
                else
                {
                    PlayerActionManager.Instance.SelectEntity(null);
                }
            }
        }
    }

    public void SelectCharacter(CharacterEntity character)
    {
        if (controllableCharacters.Contains(character))
        {
            PlayerActionManager.Instance.SelectEntity(character);
        }
    }

    public void SetGroupMode(bool toSet)
    {
        areGrouped = toSet;
        teamUi.SetGroupMode(areGrouped);
    }
}
