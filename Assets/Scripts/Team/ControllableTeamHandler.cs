using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableTeamHandler : Singleton<ControllableTeamHandler>
{
    [SerializeField] private List<CharacterEntity> controllableCharacters = new List<CharacterEntity>();

    [Header("UI")]
    [SerializeField] private ControllableTeamUI teamUi;

    private List<CharacterEntity> currentControllableCharacters = new List<CharacterEntity>();

    [ContextMenu("Initialize")]
    public void Initialize()
    {
        foreach(CharacterEntity character in controllableCharacters)
        {
            teamUi.AddCharacter(character); //Pour les tests
            ActivateControllableCharacter(character);
        }

        SelectCharacter(controllableCharacters[0]);

        RoundManager.Instance.actOnUpdateRoundMode += OnChangeRoundMode;
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
}
