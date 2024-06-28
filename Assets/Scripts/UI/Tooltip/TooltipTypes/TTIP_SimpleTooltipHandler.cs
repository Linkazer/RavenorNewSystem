using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TTIP_SimpleTooltipHandler : TooltipHandler<TTIP_SimpleTooltipData>
{
    [SerializeField] private TextMeshProUGUI textHolder;

    protected override void DisplayTooltipData(TTIP_SimpleTooltipData data)
    {
        textHolder.text = data.TextToDisplay;
    }
}
