using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/Create new Level")]
public class LevelScriptable : ScriptableObject
{
    [SerializeField] private LevelHandler levelPrefab;

    [Header("Level Details")]
    [SerializeField] private RVN_Text title;
    [SerializeField] private RVN_Text description;
    [SerializeField] private CharacterScriptable[] charactersDetails;

    public LevelHandler LevelPrefab => levelPrefab;

    public string Title => title.GetText();
    public string Description => description.GetText();

    public CharacterScriptable[] CharactersDetails => charactersDetails;
}
