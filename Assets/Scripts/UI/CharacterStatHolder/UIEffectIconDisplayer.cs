using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectIconDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject displayerHolder;

    [Header("Icon")]
    [SerializeField] private Image icon;

    private AppliedStatus currentStatus;

    public void SetEffect(AppliedStatus status)
    {
        if(currentStatus != null)
        {
            //Unsubscribe things ?
        }

        if(status == null)
        {
            currentStatus = null;
            displayerHolder.SetActive(false);
        }
        else
        {
            icon.sprite = status.Status.Icon;

            displayerHolder.SetActive(true);
        }
    }

    public void DisplayDetail()
    {
        //Popup avec les détails (Soit en clic droit, soit en mouse over)
    }
}
