using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnnemyBattleController : Singleton<EnnemyBattleController>
{
    [SerializeField] private int calculsByFrame;

   [SerializeField] private List<CharacterEntity> charactersToPlay = new List<CharacterEntity> ();

    private bool isProccessingCharacter = false;

    public void AddCharactersToPlay(List<CharacterEntity> newCharacters)
    {
        charactersToPlay.AddRange(newCharacters);

        if(!isProccessingCharacter)
        {
            PlayNextCharacter();
        }
    }

    private void EndCurrentCharacterTurn()
    {
        CharacterEntity characterToEndTurn = charactersToPlay[0];

        charactersToPlay.RemoveAt(0);
        BattleManager.Instance.EndCharacterTurn(characterToEndTurn);

        isProccessingCharacter = false;

        if (charactersToPlay.Count > 0)
        {
            PlayNextCharacter();
        }
        else
        {
            charactersToPlay.Clear();
        }
    }

    [ContextMenu("Play Next Character")]
    private void PlayNextCharacter()
    {
        isProccessingCharacter = true;

        Debug.Log("Calculate Action");

        StartCoroutine(CalculateCharacterPossibilities(charactersToPlay[0]));
    }

    private void FindCharacterAction(AIAction actionFound)
    {
        Debug.Log("Found action to do");
    }

    private IEnumerator CalculateCharacterPossibilities(CharacterEntity character)
    {
        AIAction actionToDo = null;

        Task<AIAction> task = CalculateAsynchroneAction(character);
        yield return new WaitUntil(() => task.IsCompleted);

        actionToDo = task.Result;

        FindCharacterAction(actionToDo);
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

        List<AIAction> possibleActions = new List<AIAction>();

        float maxScore = 0;

        foreach (AIConcideration concideration in characterScriptable.Condiderations)
        {
            //Check for Skill
            SkillHolder skillHolder = characterSkillHandler.GetSkillHolderForScriptable(concideration.SkillToCheck);

            if (skillHolder == null || !skillHolder.IsUsable())
            {
                if (skillHolder == null)
                {
                    Debug.LogError("Skill " + concideration.SkillToCheck + " note found in " + characterScriptable);
                }
                continue;
            }

            //TODO : Get Targets
            List<CharacterEntity> targets = CharacterManager.instance.GetPlayerEntites();

            //TODO : Get all possible movements
            List<Node> possibleMovements = new List<Node>();

            for(int i = 0; i < 250; i++)
            {
                possibleMovements.Add(characterSkillHandler.CurrentNode);
            }

            //TODO : Calculate Opprotunity attack score ?

            foreach (CharacterEntity target in targets)
            {
                AIAction actionToDoOnTarget = null;

                foreach (Node movementToCheck in possibleMovements)
                {
                    //TODO : Get all node in range for skill
                    List<Node> skillRangeNodes = new List<Node>();

                    for (int i = 0; i < 250; i++)
                    {
                        skillRangeNodes.Add(characterSkillHandler.CurrentNode);
                    }

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
                            float calculatedScore = CalculateActionScore(actionToCheck, concideration);

                            //TODO : Malus Opportunity Attack

                            //TODO : Calculs for next turn

                            //TODO : Calculate Movement cost

                            actionToCheck.score = calculatedScore;

                            if (calculatedScore > maxScore)
                            {
                                possibleActions = new List<AIAction>();

                                maxScore = calculatedScore;
                            }
                            
                            if(calculatedScore == maxScore)
                            {
                                actionToDoOnTarget = actionToCheck;
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
}
