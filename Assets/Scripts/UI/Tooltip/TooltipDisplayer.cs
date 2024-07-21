using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipData tooltip;
    [SerializeField] private Transform tooltipPosition;
    [SerializeField] private Vector2 offset = new Vector2(0, 25);

    private void Start()
    {
        if (tooltipPosition == null)
        {
            tooltipPosition = transform;
        }
    }

    public void ShowTooltip()
    {
        TooltipManager.Instance.DisplayTooltip(tooltip, tooltipPosition.position + new Vector3(offset.x, offset.y, 0));
    }

    public void HideTooltip()
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }
}
