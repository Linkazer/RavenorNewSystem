using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_ActivateGameObject : SequenceAction
{
    [SerializeField] private GameObject[] objectsToHandle;
    [SerializeField] private bool doesActivate;

    protected override void OnStartAction()
    {
        foreach (GameObject gameObject in objectsToHandle)
        {
            gameObject.SetActive(doesActivate);
        }

        EndAction();
    }

    protected override void OnEndAction()
    {

    }

    protected override void OnSkipAction()
    {

    }
}