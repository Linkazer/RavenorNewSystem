using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : Singleton<TooltipManager>
{
    [SerializeField] private TooltipHandler[] tooltips;

    private Dictionary<Type, TooltipHandler> tooltipHandlersByType = new Dictionary<Type, TooltipHandler>();

    private TooltipHandler activeTooltip;

    protected override void OnAwake()
    {
        base.OnAwake();

        foreach (TooltipHandler handler in tooltips)
        {
            tooltipHandlersByType.Add(handler.TooltipTypeHandled, handler);
        }
    }

    public void DisplayTooltip(TooltipData tooltipToDisplay, Vector2 screenPosition)
    {
        if(activeTooltip != null)
        {
            HideTooltip();
        }

        activeTooltip = GetHandlerForTooltipType(tooltipToDisplay);

        if (activeTooltip != null) 
        { 
            activeTooltip.DisplayTooltip(tooltipToDisplay, screenPosition);
        }
    }

    public void HideTooltip()
    {
        if (activeTooltip != null)
        {
            activeTooltip.HideTooltip();
            activeTooltip = null;
        }
    }

    private TooltipHandler GetHandlerForTooltipType(TooltipData tooltip)
    {
        if(tooltipHandlersByType.ContainsKey(tooltip.TooltipType))
        {
            return tooltipHandlersByType[tooltip.TooltipType];
        }
        else
        {
            Debug.LogError("Tooltip handler for " + tooltip.TooltipType + " does not exist");
            return null;
        }
    }
}