using System;
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

    [Header("Colors")]
    [SerializeField] private Color speakColor = Color.black;
    [SerializeField] private Color descriptionColor = new Color(0.37f, 0.16f, 0.05f);
    [SerializeField] private Color linkedSpeakColor = Color.magenta;
    [SerializeField] private Color linkedDescriptionColor = Color.magenta;

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

        string[] systemSentences = dialogueText.Split('<', '>');

        string fullSentence = "";
        bool isSystem = true;
        int systemCount = 0;

        bool isSpeaking = false;

        foreach (string systStr in systemSentences)
        {
            if (fullSentence != "" && isSystem)
            {
                fullSentence += $">";

                if (systemCount % 2 == 0)
                {
                    fullSentence += "</color>";
                }
            }

            isSystem = !isSystem;

            if (isSystem)
            {
                if (systemCount % 2 == 0)
                {
                    if (isSpeaking)
                    {
                        fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(linkedSpeakColor)}>";
                    }
                    else
                    {
                        fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(linkedDescriptionColor)}>";
                    }
                }

                systemCount++;

                fullSentence += "<" + systStr;
            }
            else
            {
                string[] formedSentence = systStr.Split('“', '”', '"');

                for (int i = 0; i < formedSentence.Length; i++)
                {
                    if (i != 0)
                    {
                        isSpeaking = !isSpeaking;

                        if (!isSpeaking)
                        {
                            fullSentence += '"';
                            fullSentence += "</color>";
                        }

                        if (isSpeaking)
                        {
                            fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(speakColor)}>\"";
                        }
                        else
                        {
                            fullSentence += $"<color=#{ColorUtility.ToHtmlStringRGB(descriptionColor)}>";
                        }
                    }

                    fullSentence += formedSentence[i];
                }
            }
        }

        fullSentence += "</color>";

        dialogueTextMesh.text = fullSentence;

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
