using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MVT_MoveCharaToClic : MonoBehaviour
{
    public EC_Movement movementToMove;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.OnMouseLeftDown += MoveCharaToPos;
    }

    private void Update()
    {
        if (!isMoving)
        {
            movementToMove.DisplayAction(InputManager.MousePosition);
        }
    }

    void MoveCharaToPos(Vector2 pos)
    {
        isMoving = true;
        movementToMove.UndisplayAction();
        movementToMove.TryMoveToDestination(pos, OnEndMove);
    }

    void OnEndMove()
    {
        isMoving = false;
    }
}
