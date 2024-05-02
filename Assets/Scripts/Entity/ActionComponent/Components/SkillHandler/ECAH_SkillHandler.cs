using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECAH_SkillHandler : PlayerEntityActionHandler<EC_SkillHandler>
{
    private EC_SkillHandler skillHandler => entityActionComponentHandled;

    [SerializeField] private CanvasGroup skillsGroup;

    [SerializeField] private TextMeshProUGUI ressourceAmountText;
    [SerializeField] private SkillButton[] skillsButtons;

    [SerializeField] private Color skillRangeColor;
    [SerializeField] private Color skillShapeColor;

    private void Update()
    {
        if (!isLocked && skillHandler.SelectedSkill != null)
        {
            DisplayAction(InputManager.MousePosition);
        }
    }

    public override void SetHandler(PlayerActionManager handler)
    {
        base.SetHandler(handler);

        if(skillHandler.RessourceUsed != null)
        {
            skillHandler.RessourceUsed.actOnUpdateRessource += UpdateRessource;

            UpdateRessource(skillHandler.RessourceUsed.CurrentAmount);
        }
    }

    public override void UnsetHandler()
    {
        if (skillHandler.RessourceUsed != null)
        {
            skillHandler.RessourceUsed.actOnUpdateRessource -= UpdateRessource;
        }

        base.UnsetHandler();
    }

    public override void Lock(bool doesLock)
    {
        base.Lock(doesLock);

        if(doesLock)
        {
            skillsGroup.interactable = false;
            skillsGroup.blocksRaycasts = false;
        }
        else if(skillsGroup.alpha > 0)
        {
            skillsGroup.interactable = true;
            skillsGroup.blocksRaycasts = true;
        }
    }

    public override void Enable()
    {
        skillsGroup.alpha = 1f;
        skillsGroup.interactable = true;
        skillsGroup.blocksRaycasts = true;

        for(int i = 0; i < skillsButtons.Length; i++)
        {
            if(i < skillHandler.UsableSkills.Count)
            {
                skillsButtons[i].SetSkill(skillHandler.UsableSkills[i]);
            }
            else
            {
                skillsButtons[i].SetSkill(null);
            }
        }

        UpdateActionAvailibility();
    }

    public override void Disable()
    {
        skillsGroup.alpha = 0f;
        skillsGroup.interactable = false;
        skillsGroup.blocksRaycasts = false;

        for (int i = 0; i < skillsButtons.Length; i++)
        {
            skillsButtons[i].SetSkill(null);
        }
    }

    public override void UpdateActionAvailibility()
    {
        skillsGroup.interactable = !isLocked;
        skillsGroup.blocksRaycasts = !isLocked;

        for (int i = 0; i < skillHandler.UsableSkills.Count; i++)
        {
            bool isUsable = (skillHandler.RessourceUsed == null || skillHandler.RessourceUsed.HasEnoughRessource(skillHandler.UsableSkills[i].Scriptable.RessourceCost))
                            && skillHandler.UsableSkills[i].IsUsable();

            skillsButtons[i].SetUsability(isUsable);
        }
    }

    public override void UseAction(Vector2 usePosition)
    {
        base.UseAction(usePosition);

        if(skillHandler.SelectedSkill == null)
        {
            InputManager.Instance.OnMouseLeftDown -= UseAction;
        }

    }

    protected override void EndAction()
    {
        base.EndAction();

        UnselectAction();
    }

    public override void SelectAction()
    {
        enabled = true;

        base.SelectAction();
    }

    public override void UnselectAction()
    {
        InputManager.Instance.OnMouseLeftDown -= UseAction;

        UndisplayAction();

        if(skillHandler != null)
        {
            skillHandler.SelectSkill(null);
        }

        base.UnselectAction();

        enabled = false;
    }

    protected override void DisplayAction(Vector3 actionTargetPosition)
    {
        GridZoneDisplayer.UnsetGridFeedback();
        List<Node> rangeNodes = Pathfinding.Instance.GetAllNodeInDistance(skillHandler.CurrentNode, skillHandler.SelectedSkill.Range, true);
        Node targetNode = Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition);

        GridZoneDisplayer.SetGridFeedback(rangeNodes, skillRangeColor);

        if (targetNode != null && rangeNodes.Contains(targetNode))
        {
            GridZoneDisplayer.SetGridFeedback(skillHandler.SelectedSkill.GetDisplayShape(targetNode, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition)), skillShapeColor);
        }
    }

    protected override void UndisplayAction()
    {
        GridZoneDisplayer.UnsetGridFeedback();
    }

    public void SelectSkill(SKL_SkillScriptable toSelect)
    {
        if (skillHandler.SelectedSkill == toSelect)
        {
            UE_UnselectSkill();
        }
        else
        {
            if (skillHandler.SelectedSkill == null)
            {
                InputManager.Instance.OnMouseLeftDown += UseAction;
            }

            skillHandler.SelectSkill(toSelect);

            SelectAction();
        }
    }

    private void UpdateRessource(int ressourceAmount)
    {
        ressourceAmountText.text = ressourceAmount.ToString();
    }

    public void UE_SelectSkill(int skillIndex)
    {
        SelectSkill(skillHandler.UsableSkills[skillIndex].Scriptable);
    }

    public void UE_UnselectSkill()
    {
        UnselectAction();
    }
}
