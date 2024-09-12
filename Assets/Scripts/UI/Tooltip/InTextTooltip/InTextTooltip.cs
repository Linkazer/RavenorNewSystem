using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

//Use <link=\"ID\"> Bla bla bla </link> to use a link.

public class InTextTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Canvas canvasTocheck;
    [SerializeField] private RectTransform textBoxRectTransform;

    private int activeLinkedElement = -1;
    private string activeTooltipID = "";

    // Update is called once per frame
    void Update()
    {
        CheckForTooltip();
    }

    private void CheckForTooltip()
    {
        int interesectingLink = TMP_TextUtilities.FindIntersectingLink(textBox, InputManager.MouseScreenPosition, null);

        if (interesectingLink != activeLinkedElement)
        {
            HideTooltip(activeTooltipID);
            activeTooltipID = "";
        }

        if (interesectingLink == -1)
        {
            return;
        }

        TMP_LinkInfo linkInfo = textBox.textInfo.linkInfo[interesectingLink];

        activeTooltipID = linkInfo.GetLinkID();

        ShowTooltip(activeTooltipID, InputManager.MouseScreenPosition);

        activeLinkedElement = interesectingLink;
    }

    private void ShowTooltip(string tooltipId, Vector2 position)
    {
        TooltipManager.Instance.DisplayTooltip(InTextTooltipLibrary.GetTooltipOfID(tooltipId), position);
    }

    private void HideTooltip(string tooltipId)
    {
        TooltipManager.Instance.HideTooltip();
    }
}
