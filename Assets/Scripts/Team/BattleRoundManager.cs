using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoundManager : Singleton<BattleRoundManager>
{
    [SerializeField] private ControllableTeamHandler controllableTeamHandler;

    [SerializeField] private List<CharacterEntity> charactersInBattle = new List<CharacterEntity>();

    [SerializeField] private List<CharacterEntity> currentlyPlayingCharacter = new List<CharacterEntity>();

    [SerializeField] private int nextCharacterIndex;

    [ContextMenu("1. Force Start Battle")]
    public void TEST_StartBattle()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        if (charactersInBattle.Count > 0)
        {
            //return;
        }

        nextCharacterIndex = 0;

        RoundManager.instance.SetRoundMode(RoundMode.Round);

        StartNextCharactersTurn();
    }

    public void EndBattle()
    {
        foreach(CharacterEntity characterToRemove in charactersInBattle)
        {
            characterToRemove.actOnDeactivateEntity -= RemoveCharacterFromBattle;
        }

        charactersInBattle.Clear();

        RoundManager.instance.SetRoundMode(RoundMode.RealTime);
    }

    public void AddCharacterInBattle(CharacterEntity characterToAdd)
    {
        if(!charactersInBattle.Contains(characterToAdd))
        {
            characterToAdd.actOnDeactivateEntity += RemoveCharacterFromBattle;

            charactersInBattle.Add(characterToAdd);
        }
    }

    public void RemoveCharacterFromBattle(Entity entityToRemove)
    {
        if (entityToRemove is CharacterEntity)
        {
            CharacterEntity characterToRemove = entityToRemove as CharacterEntity;
            if (charactersInBattle.Contains(characterToRemove))
            {
                charactersInBattle.Remove(characterToRemove);

                characterToRemove.actOnDeactivateEntity -= RemoveCharacterFromBattle;

                if (!CheckHostileCharacterLeft())
                {
                    EndBattle();
                }
            }
        }
    }

    private void StartCharactersTurn(List<CharacterEntity> characters, bool isPlayerControlled)
    {
        currentlyPlayingCharacter = new List<CharacterEntity>(characters);

        foreach (CharacterEntity character in currentlyPlayingCharacter)
        {
            character.StartRound();
        }

        if (isPlayerControlled)
        {
            foreach (CharacterEntity character in currentlyPlayingCharacter)
            {
                controllableTeamHandler.ActivateControllableCharacter(character);
            }
        }
        else
        {
            //AI
        }
    }

    public void EndCharacterTurn(CharacterEntity characterToEndTurn)
    {
        if (currentlyPlayingCharacter.Contains(characterToEndTurn))
        {
            characterToEndTurn.EndRound();

            currentlyPlayingCharacter.Remove(characterToEndTurn);

            if(currentlyPlayingCharacter.Count == 0)
            {
                StartNextCharactersTurn();
            }
        }
    }

    private void StartNextCharactersTurn()
    {
        if(nextCharacterIndex == 0)
        {
            EndBattleRound();
        }

        List<CharacterEntity> nextCharacters = new List<CharacterEntity>();

        nextCharacters.Add(charactersInBattle[nextCharacterIndex]);

        nextCharacterIndex++;

        while(nextCharacterIndex < charactersInBattle.Count && charactersInBattle[nextCharacterIndex].IsHostile == nextCharacters[0].IsHostile && controllableTeamHandler.CanCharacterBeControlled(charactersInBattle[nextCharacterIndex]))
        {
            nextCharacters.Add(charactersInBattle[nextCharacterIndex]);
            nextCharacterIndex++;
        }

        StartCharactersTurn(nextCharacters, controllableTeamHandler.CanCharacterBeControlled(nextCharacters[0]));

        if (nextCharacterIndex >= charactersInBattle.Count)
        {
            nextCharacterIndex = 0;
        }
    }

    private bool CheckHostileCharacterLeft()
    {
        foreach(CharacterEntity character in charactersInBattle)
        {
            if(character.IsHostile)
            {
                return true;
            }
        }

        return false;
    }

    private void StartBattleRound()
    {
        RoundManager.instance.StartGlobalRound();

        nextCharacterIndex = 0;
    }

    private void EndBattleRound()
    {
        RoundManager.instance.EndGlobalRound();

        StartBattleRound();
    }
}
