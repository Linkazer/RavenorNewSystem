using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllableCharacterUI : MonoBehaviour
{
    [SerializeField] private ControllableTeamUI teamUi;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Image lockImage;

    private CharacterEntity holdedCharacter;

    public CharacterEntity HoldedCharacter => holdedCharacter;

    public void SetCharacter(CharacterEntity character)
    {
        holdedCharacter = character;

        characterPortrait.sprite = character.CharacterData.Portrait;

        canvasGroup.gameObject.SetActive(true);
    }

    public void UnsetCharacter()
    {
        Disable();

        holdedCharacter = null;

        canvasGroup.gameObject.SetActive(false);
    }

    public void Enable()
    {
        lockImage.enabled = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Disable()
    {
        lockImage.enabled = true;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void UE_SelectCharacter()
    {
        teamUi.SelectCharacter(holdedCharacter);
    }
}
