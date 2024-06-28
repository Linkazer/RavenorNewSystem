using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Simple Tooltip", menuName ="Tooltip/Simple Tooltip")]
public class TTIP_SimpleTooltipData : TooltipData
{
    [SerializeField] private RVN_Text displayedText;

    public string TextToDisplay => displayedText.GetText();
}
