using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillCooldown;

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
}
