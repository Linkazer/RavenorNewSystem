using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu_LevelSelectionManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private CanvasGroup closedBookGroup;
    [SerializeField] private CanvasGroup openedBookGroup;

    [Header("Character Detail")]
    [SerializeField] private CanvasGroup characterInformationGroup;
    [SerializeField] private MainMenu_PresentedCharacter[] characterPresentations;

    [Header("Level Detail")]
    [SerializeField] private CanvasGroup levelInformationGroup;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelDescription;

    [Header("Scene")]
    [SerializeField] private int levelSceneIndex;

    private LevelScriptable selectedLevel;

    private void Start()
    {
        SelectCampaign();
    }

    public void SelectCampaign()
    {
        //Campaign selection
        foreach (MainMenu_PresentedCharacter character in characterPresentations)
        {
            character.SetCharacter(null);
        }
    }

    public void SelectLevel(LevelScriptable newLevel)
    {
        if (selectedLevel == newLevel)
        {
            EventSystem.current.SetSelectedGameObject(null);

            selectedLevel = null;

            characterInformationGroup.alpha = 1f;
            characterInformationGroup.interactable = true;
            characterInformationGroup.blocksRaycasts = true;

            levelInformationGroup.alpha = 0f;
            levelInformationGroup.interactable = false;
            levelInformationGroup.blocksRaycasts = false;

            return;
        }

        selectedLevel = newLevel;

        levelName.text = selectedLevel.Title;
        levelDescription.text = selectedLevel.Description;

        levelInformationGroup.alpha = 1;
        levelInformationGroup.interactable = true;
        levelInformationGroup.blocksRaycasts = true;

        characterInformationGroup.alpha = 0f;
        characterInformationGroup.interactable = false;
        characterInformationGroup.blocksRaycasts = false;
    }

    public void UE_OpenBook()
    {
        closedBookGroup.alpha = 0f;
        closedBookGroup.interactable = false;
        closedBookGroup.blocksRaycasts = false;

        openedBookGroup.alpha = 1f;
        openedBookGroup.interactable = true;
        openedBookGroup.blocksRaycasts = true;
    }

    public void UE_LoadLevel()
    {
        LevelManager.SelectLevel(selectedLevel);
        RVN_SceneManager.Instance.LoadScene(levelSceneIndex);
    }
}
