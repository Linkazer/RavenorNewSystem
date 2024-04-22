using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillCooldown;

    [Header("Description")]
    [SerializeField] private CanvasGroup descriptionGroup;
    [SerializeField] private TextMeshProUGUI titleTextUi;
    [SerializeField] private TextMeshProUGUI descriptionTextUi;
    [SerializeField] private TextMeshProUGUI cooldownTextUi;

    [Header("Use Left")]
    [SerializeField] private GameObject useLeftHolder;
    [SerializeField] private TextMeshProUGUI useLeftTextUi;

    [Header("URessource Cost")]
    [SerializeField] private GameObject ressourceCostHolder;
    [SerializeField] private TextMeshProUGUI ressourceCostTextUi;

    [Header("Complexity")]
    [SerializeField] private Image fullActionImage;
    [SerializeField] private Image freectionImage;

    private SkillHolder linkedSkill;

    public void SetSkill(SkillHolder nLinkedSkill)
    {
        if (linkedSkill != null)
        {
            linkedSkill.OnUpdateCooldown -= UpdateCooldown;
        }

        if (nLinkedSkill == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            linkedSkill = nLinkedSkill;

            skillIcon.sprite = linkedSkill.Scriptable.Icon;

            linkedSkill.OnUpdateCooldown += UpdateCooldown;

            UpdateCooldown(linkedSkill.CurrentCooldown);

            titleTextUi.text = linkedSkill.Scriptable.DisplayName;
            descriptionTextUi.text = linkedSkill.Scriptable.Description;
            cooldownTextUi.text = linkedSkill.Scriptable.Cooldown.ToString();

            if(linkedSkill.Scriptable.MaxUtilisationsByLevel > 0)
            {
                useLeftHolder.SetActive(true);
                useLeftTextUi.text = linkedSkill.UtilisationByLevelLeft.ToString();
            }
            else
            {
                useLeftHolder.SetActive(false);
            }

            if (linkedSkill.Scriptable.RessourceCost > 0)
            {
                ressourceCostHolder.SetActive(true);
                ressourceCostTextUi.text = linkedSkill.Scriptable.RessourceCost.ToString();
            }
            else
            {
                ressourceCostHolder.SetActive(false);
            }

            switch(linkedSkill.Scriptable.CastComplexity)
            {
                case SkillComplexity.Ordinary:
                    fullActionImage.enabled = true;
                    freectionImage.enabled = false;
                    break;
                case SkillComplexity.Fast:
                    fullActionImage.enabled = false;
                    freectionImage.enabled = true;
                    break;
            }

            gameObject.SetActive(true);
        }
    }

    private void UpdateCooldown(int turnLeft)
    {
        if (linkedSkill.Scriptable.Cooldown > 0)
        {
            skillCooldown.fillAmount = (float)turnLeft / (float)linkedSkill.Scriptable.Cooldown;
        }
        else
        {
            skillCooldown.fillAmount = 0;
        }
    }

    public void SetUsability(bool toSet)
    {
        canvasGroup.interactable = toSet;
    }

    public void UE_ShowDescription()
    {
        descriptionGroup.alpha = 1f;
        descriptionGroup.interactable = true;
        descriptionGroup.blocksRaycasts = true;
    }

    public void UE_HideDescription()
    {
        descriptionGroup.alpha = 0f;
        descriptionGroup.interactable = false;
        descriptionGroup.blocksRaycasts = false;
    }
}
