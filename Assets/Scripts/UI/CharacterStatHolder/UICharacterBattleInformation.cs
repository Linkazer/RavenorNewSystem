using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICharacterBattleInformation : MonoBehaviour
{
    [SerializeField] private GameObject informationHolder;

    [Header("Character")]
    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Hp and Armor")]
    [SerializeField] private ECUI_ArmorState[] armorPoints;
    [SerializeField] private TextMeshProUGUI healthTextDisplayer;
    [SerializeField] private Image healthFilledImage;

    [Header("Effects")]
    [SerializeField] private UIEffectIconDisplayer[] statusDisplayers;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI strengthStat;
    [SerializeField] private TextMeshProUGUI agilityStat;
    [SerializeField] private TextMeshProUGUI spiritStat;
    [SerializeField] private TextMeshProUGUI presenceStat;
    [SerializeField] private TextMeshProUGUI instinctStat;

    [Header("Defense")]
    [SerializeField] private TextMeshProUGUI dodgeStat;
    [SerializeField] private TextMeshProUGUI willStat;

    private CharacterEntity currentCharacter;

    private void Start()
    {
        InputManager.Instance.OnMouseRightDownOnObject += OnMouseRightClicOnObject;
        InputManager.Instance.OnMouseRightDownWithoutObject += OnMouseRightClic;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnMouseRightDownOnObject -= OnMouseRightClicOnObject;
            InputManager.Instance.OnMouseRightDownWithoutObject -= OnMouseRightClic;
        }
    }

    private void OnMouseRightClic(Vector2 mousePosition)
    {
        if(informationHolder.activeSelf)
        {
            UnsetCharacter();
        }
    }

    private void OnMouseRightClicOnObject(EC_Clicable clicTarget)
    {
        if(!(clicTarget.HoldingEntity is CharacterEntity) && informationHolder.activeSelf)
        {
            UnsetCharacter();
        }
        else if(clicTarget.HoldingEntity is CharacterEntity)
        {
            SetCharacter(clicTarget.HoldingEntity as CharacterEntity);
        }
    }

    public void SetCharacter(CharacterEntity character)
    {
        if(currentCharacter == character)
        {
            UnsetCharacter(); 
            return;
        }

        if(currentCharacter != null)
        {
            UnsetCharacter();
        }

        characterIcon.sprite = character.CharacterData.Portrait;
        characterName.text = character.CharacterData.Name;

        if(character.TryGetEntityComponentOfType(out EC_HealthHandler healthHandler))
        {
            healthTextDisplayer.text = $"{healthHandler.CurrentHealth} / {healthHandler.MaxHealth}";
            healthFilledImage.fillAmount = (float)healthHandler.CurrentHealth / (float)healthHandler.MaxHealth;

            for (int i = 0; i < armorPoints.Length; i++)
            {
                if (i < healthHandler.MaxArmor)
                {
                    armorPoints[i].SetVisible(true);
                    if (i < healthHandler.CurrentArmor)
                    {
                        armorPoints[i].SetBroken(false);
                    }
                    else
                    {
                        armorPoints[i].SetBroken(true);
                    }
                }
                else
                {
                    armorPoints[i].SetVisible(false);
                }
            }
        }

        if(character.TryGetEntityComponentOfType(out EC_TraitHandler traitHandler))
        {
            strengthStat.text = traitHandler.Force.ToString();
            agilityStat.text = traitHandler.Agilite.ToString();
            spiritStat.text = traitHandler.Esprit.ToString();
            instinctStat.text = traitHandler.Instinct.ToString();
            presenceStat.text = traitHandler.Presence.ToString();
        }

        if (character.TryGetEntityComponentOfType(out EC_SkillAbsorberHandler skillAbsorberHandler))
        {
            dodgeStat.text = skillAbsorberHandler.Dodge.ToString();
            willStat.text = skillAbsorberHandler.Will.ToString();
        }

        if(character.TryGetEntityComponentOfType(out EC_StatusHandler statusHandler))
        {
            List<AppliedStatus> status = statusHandler.GetAppliedStatus();
            for (int i = 0; i < statusDisplayers.Length; i++)
            {
                if(i < status.Count)
                {
                    statusDisplayers[i].SetEffect(status[i]);
                }
                else
                {
                    statusDisplayers[i].SetEffect(null);
                }
            }
        }

        informationHolder.SetActive(true);
    }

    public void UnsetCharacter()
    {
        currentCharacter = null;

        informationHolder.SetActive(false);
    }
}
