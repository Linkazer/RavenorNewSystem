using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECUI_ArmorState : MonoBehaviour
{
    [SerializeField] private Image armorImage;

    [SerializeField] private Animator armorAnimator;

    [SerializeField] private Sprite cleanArmor;
    [SerializeField] private Sprite brokenArmor;

    public void SetVisible(bool isVisible)
    {
        armorImage.gameObject.SetActive(isVisible);
    }

    public void SetBroken(bool isBroken)
    {
        if(isBroken)
        {
            armorImage.sprite = brokenArmor;
        }
        else
        {
            armorImage.sprite = cleanArmor;
        }
    }
}
