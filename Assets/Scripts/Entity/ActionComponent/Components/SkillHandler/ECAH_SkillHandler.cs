using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ECAH_SkillHandler : PlayerEntityActionHandler<EC_SkillHandler>
{
    private EC_SkillHandler skillHandler => entityActionComponentHandled;

    [SerializeField] private CanvasGroup skillsGroup;

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
            skillsButtons[i].SetUsability(skillHandler.UsableSkills[i].IsUsable());
        }
    }

    public override void UseAction(Vector2 usePosition)
    {
        InputManager.Instance.OnMouseLeftDown -= UseAction;
        base.UseAction(usePosition);
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
        List<Node> rangeNodes = Pathfinding.Instance.GetAllNodeInDistance(skillHandler.CurrentNode, skillHandler.SelectedSkill.Scriptable.Range, true);
        Node targetNode = Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition);

        GridZoneDisplayer.SetGridFeedback(rangeNodes, skillRangeColor);

        if (targetNode != null && rangeNodes.Contains(targetNode))
        {
            GridZoneDisplayer.SetGridFeedback(skillHandler.SelectedSkill.Scriptable.GetDisplayShape(targetNode, Grid.Instance.GetNodeFromWorldPoint(actionTargetPosition)), skillShapeColor);
        }
    }

    protected override void UndisplayAction()
    {
        GridZoneDisplayer.UnsetGridFeedback();
    }

    public void UE_SelectSkill(int skillIndex)
    {
        if (skillHandler.SelectedSkill == skillHandler.UsableSkills[skillIndex])
        {
            UE_UnselectSkill();
        }
        else
        {
            if(skillHandler.SelectedSkill == null)
            {
                InputManager.Instance.OnMouseLeftDown += UseAction;
            }

            skillHandler.SelectSkill(skillHandler.UsableSkills[skillIndex]);

            SelectAction();
        }
    }

    public void UE_UnselectSkill()
    {
        UnselectAction();
    }
}
