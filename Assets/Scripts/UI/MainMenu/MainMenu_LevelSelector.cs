using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_LevelSelector : MonoBehaviour
{
    [Header("Level data")]
    [SerializeField] private LevelScriptable level;

    [Header("Selection Button")]
    [SerializeField] private GameObject selectionHolder;
    [SerializeField] private TextMeshProUGUI selectionLevelName;

    [Header("Selected")]
    [SerializeField] private GameObject selectedHolder;
    [SerializeField] private TextMeshProUGUI selectedLevelName;
    [SerializeField] private TextMeshProUGUI selectedLevelDescription;

    [Header("Dev")]
    [SerializeField] private MainMenu_LevelSelectionManager selectionManager;

    public LevelScriptable Level => level;

    private void OnEnable()
    {
        selectionLevelName.text = level.Title;

        selectedLevelName.text = level.Title;
        selectedLevelDescription.text = level.Description;
    }

    public void UnselectLevel()
    {
        selectionHolder.SetActive(true);
        selectedHolder.SetActive(false);
    }

    public void UE_SelectLevel()
    {
        selectionHolder.SetActive(false);
        selectedHolder.SetActive(true);

        selectionManager.SelectLevel(this);
    }

    public void UE_PlayLevel()
    {
        selectionManager.LoadLevel(level);
    }
}
