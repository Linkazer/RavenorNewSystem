using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle the Level selection.
/// </summary>
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelScriptable levelToLoad;
    [SerializeField] private float delayForStart = 0f;

    private void SelectLevel(LevelScriptable levelToSelect)
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

        Instantiate(levelToLoad.LevelPrefab).InstantiateLevel();
    }
}
