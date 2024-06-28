using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TooltipHandler : MonoBehaviour
{
    [SerializeField] private RectTransform canvasUsed;
    [SerializeField] private GameObject tooltipGameObject;
    [SerializeField] private RectTransform tooltipHandlerTransform;
    [SerializeField] private RectTransform tooltipSizeTransform;

    public abstract Type TooltipTypeHandled { get; }

    public void DisplayTooltip(TooltipData data, Vector2 screenPosition)
    {
        transform.position = screenPosition;

        Vector2 anchoredPosition = tooltipHandlerTransform.anchoredPosition;

        if (anchoredPosition.x + tooltipSizeTransform.rect.width / 2 > canvasUsed.rect.width)
        {
            anchoredPosition.x = canvasUsed.rect.width - tooltipSizeTransform.rect.width / 2;
        }
        else if (anchoredPosition.x - tooltipSizeTransform.rect.width / 2 < 0)
        {
            anchoredPosition.x = tooltipSizeTransform.rect.width / 2;
        }

        if (anchoredPosition.y + tooltipSizeTransform.rect.height > canvasUsed.rect.height)
        {
            anchoredPosition.y = canvasUsed.rect.height - tooltipSizeTransform.rect.height;
        }
        else if (anchoredPosition.y < 0)
        {
            anchoredPosition.y = 0;
        }

        tooltipHandlerTransform.anchoredPosition = anchoredPosition;

        DisplayTooltipData(data);

        tooltipGameObject.SetActive(true);
    }

    protected abstract void DisplayTooltipData(TooltipData data);

    public void HideTooltip()
    {
        tooltipGameObject.SetActive(false);
    }
}

public abstract class TooltipHandler<T> : TooltipHandler where T : TooltipData
{
    public override Type TooltipTypeHandled => typeof(T);

    protected override void DisplayTooltipData(TooltipData data)
    {
        DisplayTooltipData(data as T);
    }

    protected abstract void DisplayTooltipData(T data);
}
