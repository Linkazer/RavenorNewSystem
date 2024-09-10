using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_LevelSelector : MonoBehaviour
{
    [SerializeField] private LevelScriptable level;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private MainMenu_LevelSelectionManager selectionManager;

    [SerializeField] private Button button;

    private void OnEnable()
    {
        levelName.text = level.Title;
    }

    public void UE_SelectLevel()
    {
        button.Select();
        selectionManager.SelectLevel(level);
    }
}
