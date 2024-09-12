using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMVT_StayMelee : AIMovementBehavior
{
    private const float MaxDistanceForMelee = Pathfinding.DiagonaleDistance;
    private const float OpportunityAttackMalus = 50;

    public override List<Node> GetBestMovementNodes(EC_Movement characterMovement)
    {
        List<Node> toReturn = new List<Node>();
        Node casterNode = characterMovement.CurrentNode;

        Node targetCharacterNode = GetClosestCharacterode(casterNode, BattleManager.Instance.GetCharacterInBattleByAlliance(characterMovement.HoldingEntity as CharacterEntity, false));

        if (Pathfinding.Instance.IsNodeVisible(targetCharacterNode, casterNode, MaxDistanceForMelee))
        {
            toReturn.Add(casterNode);
        }
        else
        {
            float maxScore = -1;
            PathType currentPathType = PathType.PathBlockByMovingObstacle;

            List<Node> possibleMovements = Pathfinding.Instance.CalculatePathfinding(casterNode, null, characterMovement.MovementLeft);

            foreach (Node n in possibleMovements)
            {
                float distanceMovementPosTargetPos = -1;

                targetCharacterNode = GetClosestCharacterode(casterNode, BattleManager.Instance.GetCharacterInBattleByAlliance(characterMovement.HoldingEntity as CharacterEntity, false));

                List<Node> pathFromNToTarget = Pathfinding.Instance.CalculatePathfinding(n, targetCharacterNode, -1, true, true);

                if (pathFromNToTarget.Count > 0)
                {
                    distanceMovementPosTargetPos = Pathfinding.Instance.GetPathLength(n, pathFromNToTarget);

                    if (currentPathType != PathType.PathClear)
                    {
                        currentPathType = PathType.PathClear;
                        maxScore = -1;
                    }
                }
                else if (currentPathType != PathType.PathClear)
                {
                    pathFromNToTarget = Pathfinding.Instance.CalculatePathfinding(n, targetCharacterNode, -1, false, true);

                    if (pathFromNToTarget.Count > 0)
                    {
                        distanceMovementPosTargetPos = Pathfinding.Instance.GetPathLength(n, pathFromNToTarget);
                    }
                }

                //Pourcentage par rapport à la distance voulue
                if (distanceMovementPosTargetPos > MaxDistanceForMelee)
                {
                    distanceMovementPosTargetPos += Mathf.Abs(distanceMovementPosTargetPos - MaxDistanceForMelee);
                }
                else if (distanceMovementPosTargetPos >= 0)//La position est à la bonne distance voulue
                {
                    distanceMovementPosTargetPos = 0;
                }

                if (distanceMovementPosTargetPos >= 0)
                {
                    if (n != casterNode)
                    {
                        List<Node> pathFromCasterToN = Pathfinding.Instance.CalculatePathfinding(casterNode, n, -1, true, true);
                        pathFromCasterToN.Insert(0, casterNode);

                        //TODO : Opportunity attack score
                        /*if (RVN_AiBattleManager.Instance.OpportunityAttackScore(currentCharacter, currentCharacterHealth, pathFromCasterToN) > 0)
                        {
                            distanceMovementPosTargetPos += OpportunityAttackMalus;
                        }*/
                    }

                    if (maxScore < 0 || distanceMovementPosTargetPos < maxScore)
                    {
                        toReturn = new List<Node>();

                        toReturn.Add(n);

                        maxScore = distanceMovementPosTargetPos;
                    }
                    else if (distanceMovementPosTargetPos == maxScore)
                    {
                        toReturn.Add(n);
                    }
                }
            }
        }

        return toReturn;
    }

    private Node GetClosestCharacterode(Node nodeToCheck, List<CharacterEntity> possibleTarget)
    {
        List<Node> possibleNodeTargets = new List<Node>();

        float distanceFound = -1f;

        foreach (CharacterEntity character in possibleTarget)
        {
            if(character.TryGetEntityComponentOfType(out EC_NodeHandler nodeHandler))
            {
                if(distanceFound < 0 || Pathfinding.Instance.GetDistance(nodeToCheck, nodeHandler.CurrentNode) < distanceFound)
                {
                    possibleNodeTargets.Clear();
                    distanceFound = Pathfinding.Instance.GetDistance(nodeToCheck, nodeHandler.CurrentNode);
                }

                if(Pathfinding.Instance.GetDistance(nodeToCheck, nodeHandler.CurrentNode) == distanceFound)
                {
                    possibleNodeTargets.Add(nodeHandler.CurrentNode);
                }
            }
        }

        if (possibleNodeTargets.Count > 0)
        {
            return possibleNodeTargets[Random.Range(0, possibleNodeTargets.Count)];
        }
        else
        {
            return null;
        }
    }
}
