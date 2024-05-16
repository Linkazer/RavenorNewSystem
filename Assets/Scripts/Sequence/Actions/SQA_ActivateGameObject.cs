using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ActivateGameObject : SequenceAction
{
    [SerializeField] private GameObject[] objectsToHandle;
    [SerializeField] private bool doesActivate;

    protected override void OnStartAction(SequenceContext context)
    {
        foreach (GameObject gameObject in objectsToHandle)
        {
            gameObject.SetActive(doesActivate);
        }

        EndAction(context);
    }

    protected override void OnEndAction(SequenceContext context)
    {

    }

    protected override void OnSkipAction(SequenceContext context)
    {

    }
}