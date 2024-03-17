using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup handlerGroup;
    [SerializeField] private TextMeshProUGUI dialogueTextMesh;
    [SerializeField] private CanvasGroup speakerGroup;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Image speakerPortrait;

    private Action endCallback;

    private void OnEnable()
    {
        if(DialogueManager.Instance != null)
        {
            DialogueManager.Instance.SetCurrentHandler(this);
        }
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.UnsetCurrentHandler(this);
        }
    }

    public void DisplayDialogueText(DialogueSpeaker dialogueSpeaker, string dialogueText, Action endDialogueTextCallback)
    {
        endCallback = endDialogueTextCallback;

        dialogueTextMesh.text = dialogueText;

        if(dialogueSpeaker == null)
        {
            speakerGroup.alpha = 0f;
        }
        else
        {
            speakerName.text = dialogueSpeaker.Name;
            speakerName.color = dialogueSpeaker.NameColor;
            speakerPortrait.sprite = dialogueSpeaker.Portrait;
            speakerGroup.alpha = 1f;
        }

        ActiveHandlerInput(true);
    }

    private void EndDisplayDialogueText()
    {
        ActiveHandlerInput(false);
        endCallback?.Invoke();
    }

    public void ShowDialogueHandler()
    {
        handlerGroup.alpha = 1f;
    }

    public void HideDialogueHandler()
    {
        handlerGroup.alpha = 0f;
    }

    private void ActiveHandlerInput(bool toSet)
    {
        handlerGroup.interactable = toSet;
        handlerGroup.blocksRaycasts = toSet;
    }

    /// <summary>
    /// Called by the Dialogue box button
    /// </summary>
    public void UE_ClicOnDialogueHandler()
    {
        EndDisplayDialogueText();
    }
}
