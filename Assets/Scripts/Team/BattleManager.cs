using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    /// <summary>
    /// The ControllableTeamHandler
    /// </summary>
    [SerializeField] private ControllableTeamHandler controllableTeamHandler;

    /// <summary>
    /// List of all character used in the battle.
    /// </summary>
    [SerializeField] private List<CharacterEntity> charactersInBattle = new List<CharacterEntity>();

    /// <summary>
    /// List of character that can do actions.
    /// </summary>
    [SerializeField] private List<CharacterEntity> currentlyPlayingCharacter = new List<CharacterEntity>();

    private int nextCharacterIndex;

    private bool isBattleRunning = false;

    public bool IsBattleRunning => isBattleRunning;

    /// <summary>
    /// Update the Battle state when a character change its hostility
    /// </summary>
    /// <param name="updatedEntity">The character updated</param>
    public void OnCharacterUpdateHostility(Entity updatedEntity)
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

    /// <summary>
    /// Start a battle. Does nothing is a battle si already running
    /// </summary>
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

    /// <summary>
    /// Check which character play first.
    /// </summary>
    /// <param name="chara1"></param>
    /// <param name="chara2"></param>
    /// <returns></returns>
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

    /// <summary>
    /// End the battle.
    /// </summary>
    public void EndBattle()
    {
        if (isBattleRunning)
        {
            isBattleRunning = false;

            charactersInBattle.Clear();

            RoundManager.instance.SetRoundMode(RoundMode.RealTime);
        }
    }

    /// <summary>
    /// Add a new character in the battle.
    /// </summary>
    /// <param name="characterToAdd">The character to add.</param>
    public void AddCharacterInBattle(CharacterEntity characterToAdd)
    {
        if(!charactersInBattle.Contains(characterToAdd))
        {
            charactersInBattle.Add(characterToAdd);
        }
    }

    /// <summary>
    /// Remove a character from the battle.
    /// </summary>
    /// <param name="entityToRemove">The character to remove from the battle.</param>
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

    /// <summary>
    /// Start the turn of multiple characters.
    /// </summary>
    /// <param name="characters">The character to start turns.</param>
    /// <param name="isPlayerControlled">Are they controlled by the player.</param>
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

    /// <summary>
    /// Called when a character end its turn.
    /// </summary>
    /// <param name="characterToEndTurn"></param>
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

    /// <summary>
    /// Search for the list of next character turns.
    /// </summary>
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

    /// <summary>
    /// Check if there are ennemies left in the battle.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Start a new Battle round. It allows every element outside the battle to start their round.
    /// </summary>
    private void StartBattleRound()
    {
        RoundManager.instance.StartGlobalRound();

        nextCharacterIndex = 0;
    }

    /// <summary>
    /// Start a new Battle round. It allows every element outside the battle to end their round.
    /// </summary>
    private void EndBattleRound()
    {
        RoundManager.instance.EndGlobalRound();

        charactersInBattle.Sort((x, y) => CheckCharacterPriority(x, y));

        StartBattleRound();
    }
}
