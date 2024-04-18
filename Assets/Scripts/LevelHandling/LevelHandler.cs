using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler CurrentLevel;

    [Header("Characters")]
    [SerializeField] private CharacterEntity[] startingControllableCharacters;
    [SerializeField] private CharacterEntity[] startingActiveEntities;

    [Header("Level Flow")]
    [SerializeField] private SequenceCutscene startCutscene;
    [SerializeField] private LevelEnd[] possibleEnds = new LevelEnd[0];

    public void InstantiateLevel()
    {
        CurrentLevel = this;

        StartCoroutine(InitializeLevel());
    }

    private IEnumerator InitializeLevel()
    {
        yield return 0;
        Grid.Instance.CreateGrid();

        ControllableTeamHandler.Instance.Initialize();

        yield return 0;

        foreach (CharacterEntity character in startingControllableCharacters)
        {
            character.Activate();
        }

        foreach (CharacterEntity character in startingActiveEntities)
        {
            character.Activate();
        }

        yield return 0;

        foreach (LevelEnd possibleEnd in possibleEnds)
        {
            possibleEnd.Initialize(this);
        }

        foreach (CharacterEntity character in startingControllableCharacters)
        {
            ControllableTeamHandler.Instance.AddCharacter(character);
        }

        startCutscene.StartAction(ActivateLevel);
    }

    public void ActivateLevel()
    {
        
    }
}
