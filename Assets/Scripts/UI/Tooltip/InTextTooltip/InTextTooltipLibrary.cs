using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTextTooltipLibrary : Singleton<InTextTooltipLibrary>
{
    [Serializable]
    private struct TooltipWithId
    {
        public string ID;
        public TooltipData tooltip;
    }

    [SerializeField] private TooltipWithId[] tooltips;

    private Dictionary<string, TooltipData> tooltipByID = new Dictionary<string, TooltipData>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (TooltipWithId data in tooltips)
        {
            if (!tooltipByID.ContainsKey(data.ID))
            {
                tooltipByID.Add(data.ID, data.tooltip);
            }
            else
            {
                Debug.LogError($"{data.ID} Tooltip exist multiple times.");
            }
        }
    }

    public static TooltipData GetTooltipOfID(string id)
    {
        if (instance.tooltipByID.ContainsKey(id))
        {
            return instance.tooltipByID[id];
        }

        return null;
    }
}
