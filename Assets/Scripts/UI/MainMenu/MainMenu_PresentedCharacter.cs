using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu_PresentedCharacter : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject presentationHolder;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterDescription;
    [SerializeField] private Image characterIcon;

    [Header("Data")]
    [SerializeField] private CharacterScriptable baseCharacter;
    [SerializeField] private int characterTitleSize;

    public void SetCharacter(CharacterScriptable character)
    {
        if(character == null && baseCharacter == null)
        {
            presentationHolder.SetActive(false);
            return;
        }
        else
        {
            if(character == null)
            {
                character = baseCharacter;
            }

            characterIcon.sprite = character.Portrait;
            characterName.text = $"{character.Name}, <size={characterTitleSize}%>{character.CharacterTitle}";
            characterDescription.text = character.CharacterDescription;

            presentationHolder.SetActive(true);
        }
    }
}
