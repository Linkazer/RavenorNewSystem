using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MVT_MoveCharaToClic : MonoBehaviour
{
    public EC_Movement movementToMove;

    private bool isMoving;

    public Entity[] possibleEntities;

    private int curretnEntityid;

    public void ChangeEntity()
    {
        curretnEntityid++;

        if(curretnEntityid == possibleEntities.Length)
        {
            curretnEntityid = 0;
        }

        PlayerActionManager.Instance.SelectEntity(possibleEntities[curretnEntityid]);
    }

}
