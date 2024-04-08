using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelScriptable levelToLoad;

    private void SelectLevel(LevelScriptable levelToSelect)
    {
        levelToLoad = levelToSelect;
    }

    [ContextMenu("Start Level")]
    private void StartLevel()
    {
        Instantiate(levelToLoad.LevelPrefab).InstantiateLevel();
    }
}
