using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Node;

public class EnnemyBattleController : Singleton<EnnemyBattleController>
{
    [SerializeField] private float durationBeforeEndAndStartTurn = 0.5f;

    [SerializeField] private List<CharacterEntity> charactersToPlay = new List<CharacterEntity> ();

    private bool isProccessingCharacter = false;

    private CharacterEntity CurrentCharacter => charactersToPlay[0];

    public void AddCharactersToPlay(List<CharacterEntity> newCharacters)
    {
        charactersToPlay.AddRange(newCharacters);

        if(!isProccessingCharacter)
        {
            StartAITurn();
        }
    }

    private void StartAITurn()
    {
        isProccessingCharacter = true;
        PlayerActionManager.instance.AddLock(this);
        PrepareStartCharacterTurn();
    }

    private void EndAITurn()
    {
        isProccessingCharacter = false;
        PlayerActionManager.instance.RemoveLock(this);
        charactersToPlay.Clear();
    }

    private void PrepareStartCharacterTurn()
    {
        CameraController.Instance.SetCameraFocus(charactersToPlay[0].transform);

        TimerManager.CreateGameTimer(durationBeforeEndAndStartTurn, PlayNextCharacter);
    }

    private void PlayNextCharacter()
    {
        //Debug.Log("Start AI Character Turn");

        StartCoroutine(CalculateCharacterPossibilities(charactersToPlay[0]));
    }

    private void PrepareEndCharacterTurn()
    {
        TimerManager.CreateGameTimer(durationBeforeEndAndStartTurn, EndCurrentCharacterTurn);
    }

    private void EndCurrentCharacterTurn()
    {
        CharacterEntity characterToEndTurn = charactersToPlay[0];

        charactersToPlay.RemoveAt(0);
        BattleManager.Instance.EndCharacterTurn(characterToEndTurn);

        //Debug.Log("End AI Character Turn");

        if (charactersToPlay.Count > 0)
        {
            PrepareStartCharacterTurn();
        }
        else
        {
            EndAITurn();
        }
    }

    private void DoNextAction(AIAction actionFound)
    {
        //Debug.Log("Do Next Action");

        CurrentCharacter.TryGetEntityComponentOfType(out EC_Movement characterMovementHandler);

        if (actionFound != null)
        { 
            if(actionFound.movementTarget != null && actionFound.movementTarget != characterMovementHandler.CurrentNode)
            {
                //Debug.Log("AI Move toward action destination");
                characterMovementHandler.TryMoveToDestination(actionFound.movementTarget, () => DoNextAction(actionFound));
            }
            else
            {
                //Debug.Log("AI Use skill");
                CurrentCharacter.TryGetEntityComponentOfType(out EC_SkillHandler characterSkillHandler);

                characterSkillHandler.SelectSkill(actionFound.skillToUse);
                characterSkillHandler.UseAction(actionFound.skillTarget.WorldPosition, () => DoNextAction(null));
            }
        }
        else if (characterMovementHandler.CanMove)
        {
            Node movementTarget = SearchForBestMovement(characterMovementHandler);

            if (movementTarget != characterMovementHandler.CurrentNode)
            {
                //Debug.Log("AI Move toward best destination");
                characterMovementHandler.TryMoveToDestination(movementTarget, () => DoNextAction(actionFound));
            }
            else
            {
                PrepareEndCharacterTurn();
            }
        }
        else
        {
            PrepareEndCharacterTurn();
        }
    }

    private IEnumerator CalculateCharacterPossibilities(CharacterEntity character)
    {
        AIAction actionToDo = null;

        Task<AIAction> task = CalculateAsynchroneAction(character);
        yield return new WaitUntil(() => task.IsCompleted);

        actionToDo = task.Result;

        DoNextAction(actionToDo);
    }

    private async Task<AIAction> CalculateAsynchroneAction(CharacterEntity character)
    {
        AIAction actionToReturn = await Task.Run(() => CalculateActionToDo(character));

        return actionToReturn;
    }


    private AIAction CalculateActionToDo(CharacterEntity character)
    {
        AICharacterScriptable characterScriptable = character.CharacterData as AICharacterScriptable;

        EC_SkillHandler characterSkillHandler = null;
        character.TryGetEntityComponentOfType(out characterSkillHandler);

        EC_Movement characterMovementHandler = null;
        character.TryGetEntityComponentOfType(out characterMovementHandler);

        List<AIAction> possibleActions = new List<AIAction>();

        float maxScore = 0;

        foreach (AIConcideration concideration in characterScriptable.Condiderations)
        {
            //Check for Skill
            SkillHolder skillHolder = characterSkillHandler.GetSkillHolderForScriptable(concideration.SkillToCheck);

            if (skillHolder == null || !characterSkillHandler.CanSelectSkill(skillHolder))
            {
                if (skillHolder == null)
                {
                    Debug.LogError("Skill " + concideration.SkillToCheck + " note found in " + characterScriptable);
                }
                continue;
            }

            List<CharacterEntity> targets = GetTargetCharacters(character, skillHolder.Scriptable);

            List<Node> possibleMovements = Pathfinding.Instance.CalculatePathfinding(characterMovementHandler.CurrentNode, null, characterMovementHandler.MovementLeft);

            //TODO : Calculate Opprotunity attack score ?

            foreach (CharacterEntity target in targets)
            {
                AIAction actionToDoOnTarget = null;

                float minimumDistance = -1f;

                target.TryGetEntityComponentOfType(out EC_NodeHandler targetNodeHandler);

                foreach (Node movementToCheck in possibleMovements)
                {
                    List<Node> skillRangeNodes = GetSkillUseNodes(movementToCheck, targetNodeHandler.CurrentNode, skillHolder.Scriptable);

                    foreach (Node skillNode in skillRangeNodes)
                    {
                        AIAction actionToCheck = new AIAction();
                        actionToCheck.character = character;
                        actionToCheck.skillToUse = skillHolder.Scriptable;
                        actionToCheck.movementTarget = movementToCheck;
                        actionToCheck.skillTarget = skillNode;
                        actionToCheck.hitedNodes = skillHolder.Scriptable.GetDisplayShape(movementToCheck, skillNode);

                        if (characterSkillHandler.CanUseSkillAtNode(skillHolder.Scriptable, movementToCheck, skillNode))
                        {
                            float calculatedMinimalDistance = -1f;

                            float calculatedScore = CalculateActionScore(actionToCheck, concideration);

                            //TODO : Malus Opportunity Attack

                            if(movementToCheck != characterMovementHandler.CurrentNode)
                            {
                                calculatedMinimalDistance = Pathfinding.Instance.GetDistance(movementToCheck, characterMovementHandler.CurrentNode);
                            }
                            else
                            {
                                calculatedMinimalDistance = 0f;
                            }

                            if (calculatedScore > maxScore)
                            {
                                possibleActions = new List<AIAction>();

                                maxScore = calculatedScore;
                            }
                            
                            if(calculatedScore == maxScore)
                            {
                                if (minimumDistance < 0 || calculatedMinimalDistance <= minimumDistance)
                                {
                                    minimumDistance = calculatedMinimalDistance;
                                    actionToDoOnTarget = actionToCheck;
                                }
                            }
                        }
                    }
                }

                if (actionToDoOnTarget != null)
                {
                    possibleActions.Add(actionToDoOnTarget);
                }
            }
        }

        if (possibleActions.Count > 0)
        {
            int rng = new System.Random().Next(0, possibleActions.Count);

            return possibleActions[rng];
        }
        else
        {
            return null;
        }
    }

    private List<CharacterEntity> GetTargetCharacters(CharacterEntity casterCharacter, SKL_SkillScriptable skillToCheck)
    {
        List<CharacterEntity> toReturn = new List<CharacterEntity>();

        List<CharacterEntity> charactersInBattle = BattleManager.Instance.CharactersInBattle;

        foreach (CharacterEntity character in charactersInBattle)
        {
            switch (skillToCheck.CastHostility)
            {
                case CharacterHostility.Ally:
                    if(CharacterManager.AreCharacterAlly(casterCharacter, character) == 1)
                    {
                        toReturn.Add(character);
                    }
                    break;
                case CharacterHostility.Hostile:
                    if (CharacterManager.AreCharacterAlly(casterCharacter, character) == -1)
                    {
                        toReturn.Add(character);
                    }
                    break;
                case CharacterHostility.Neutral:
                    toReturn.Add(character);
                    break;
            }
        }

        return toReturn;
    }

    private List<Node> GetSkillUseNodes(Node castNode, Node targetNode, SKL_SkillScriptable skillScriptable)
    {
        List<Node> toReturn = new List<Node>();

        foreach (Node n in skillScriptable.GetDisplayShape(castNode, targetNode))
        {
            if(Pathfinding.Instance.GetAllNodeInDistance(castNode, skillScriptable.Range, true).Contains(n))
            {
                toReturn.Add(n);
            }
        }

        return toReturn;
    }

    private Node SearchForBestMovement(EC_Movement characterMovementHandler)
    {
        List<Node> possibleTargetNodes = (CurrentCharacter.CharacterData as AICharacterScriptable).MovementBehavior.GetBestMovementNodes(characterMovementHandler);

        Node toReturn = null;

        if (possibleTargetNodes.Count > 0 && !possibleTargetNodes.Contains(characterMovementHandler.CurrentNode))
        {
            toReturn = possibleTargetNodes[UnityEngine.Random.Range(0, possibleTargetNodes.Count)];
        }
        else
        {
            toReturn = characterMovementHandler.CurrentNode;
        }

        return toReturn;
    }

    private float CalculateActionScore(AIAction plannedAction, AIConcideration consideration)
    {
        float result = 0;
        float coef = 0;

        foreach (AICalcul calculValue in consideration.ScoreCalculs)
        {
            result += CalculateConsideration(plannedAction, calculValue) * (calculValue.calculImportance + 1);
            coef += calculValue.calculImportance + 1;
        }

        if (coef == 0)
        {
            coef = 1;
        }

        return consideration.BonusScore + (result / coef);
    }

    private float CalculateConsideration(AIAction plannedAction, AICalcul calcul)
    {
        float calculResult = calcul.Calculate(plannedAction); ;

        return Mathf.Clamp(calculResult, 0, 1);
    }

    //TODO : Calculate Attack of Opportunity (Si nécessaire)
    /*private float CalculateOpportunityAttackScore(List<Node> pathTotravel, EC_NodeHandler movingCharacterHandler)
    {
        float ooportunityAttackAmount = 0;

        foreach (Node node in pathTotravel)
        {
            foreach(NodeBlocker blocker in node.exitBlockers)
            {
                if (blocker.Invoke(movingCharacterHandler, null, new object[] { false }))
                {
                    ooportunityAttackAmount++;
                }
            }
        }
    }*/
}
