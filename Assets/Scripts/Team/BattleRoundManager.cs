using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoundManager : Singleton<BattleRoundManager>
{
    [SerializeField] private ControllableTeamHandler controllableTeamHandler;

    [SerializeField] private List<CharacterEntity> charactersInBattle = new List<CharacterEntity>();

    [SerializeField] private List<CharacterEntity> currentlyPlayingCharacter = new List<CharacterEntity>();

    [SerializeField] private int nextCharacterIndex;

    private bool isBattleRunning = false;

    public bool IsBattleRunning => isBattleRunning;

    public void OnCharacterUpdateHostility(CharacterEntity updatedEntity)
    {
        if (!CheckHostileCharacterLeft())
        {
            EndBattle();
        }
        else if(!isBattleRunning)
        {
            StartBattle();
        }
    }

    public void StartBattle()
    {
        if (!isBattleRunning)
        {
            charactersInBattle.Clear();

            foreach (CharacterEntity character in CharacterManager.Instance.ActiveCharacters) 
            { 
                if(character.Hostility != CharacterHostility.Neutral)
                {
                    charactersInBattle.Add(character);
                }
            }

            charactersInBattle.Sort((x,y) => CheckCharacterPriority(x,y));

            nextCharacterIndex = 0;

            RoundManager.instance.SetRoundMode(RoundMode.Round);

            isBattleRunning = true;

            StartNextCharactersTurn();
        }
    }

    private int CheckCharacterPriority(CharacterEntity chara1, CharacterEntity chara2)
    {
        if (chara1.Priority > chara2.Priority)
        {
            return 1;
        }
        else if (chara1.Priority < chara2.Priority)
        {
            return -1;
        }

        return 0;
    }

    public void EndBattle()
    {
        if (isBattleRunning)
        {
            isBattleRunning = false;

            charactersInBattle.Clear();

            RoundManager.instance.SetRoundMode(RoundMode.RealTime);
        }
    }

    public void AddCharacterInBattle(CharacterEntity characterToAdd)
    {
        if(!charactersInBattle.Contains(characterToAdd))
        {
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

        while(nextCharacterIndex < charactersInBattle.Count && charactersInBattle[nextCharacterIndex].Hostility == nextCharacters[0].Hostility && controllableTeamHandler.CanCharacterBeControlled(charactersInBattle[nextCharacterIndex]))
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
            if(character.Hostility == CharacterHostility.Hostile)
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

        charactersInBattle.Sort((x, y) => CheckCharacterPriority(x, y));

        StartBattleRound();
    }
}
