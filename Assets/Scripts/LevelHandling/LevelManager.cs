using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the Level selection.
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    private static LevelScriptable levelToLoad;

    [SerializeField] private LevelScriptable testLevel;
    [SerializeField] private float delayForStart = 0f;

    public static void SelectLevel(LevelScriptable levelToSelect)
    {
        levelToLoad = levelToSelect;
    }

    private void Start()
    {
        StartCoroutine(StartLevelWithDelay(delayForStart)); //TODO : A faire après le load de la scène dans le scene manager
    }

    private IEnumerator StartLevelWithDelay(float delay)
    {
        if (delay < 0)
        {
            yield return 0;
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }

        if(levelToLoad == null)
        {
            levelToLoad = testLevel;
        }
        Instantiate(levelToLoad.LevelPrefab).InstantiateLevel();
    }
}
