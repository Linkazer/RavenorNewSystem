using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.TerrainTools;
using System;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera roomOpeningCamera;
    [SerializeField] private Entity[] entitiesToEnable;

    [SerializeField] private SequenceCutscene openningCutscene;

    private bool isOpen;
    private Action endOpenCallback;

    public void OpenRoom(Action callback)
    {
        if(!isOpen)
        {
            endOpenCallback = callback;
            isOpen = true;
            StartCoroutine(StartOpenningRoomSequence());
        }
        else
        {
            endOpenCallback?.Invoke();
        }
    }

    private IEnumerator StartOpenningRoomSequence()
    {
        PlayerActionManager.Instance.AddLock(this);
        roomOpeningCamera.enabled = true;

        foreach(Entity entity in entitiesToEnable)
        {
            entity.Activate();
        }

        yield return new WaitForSeconds(1f);

        openningCutscene.StartAction(() => StartCoroutine(EndOpeningRoomSequence()));
    }

    private IEnumerator EndOpeningRoomSequence()
    {
        roomOpeningCamera.enabled = false;

        yield return new WaitForSeconds(1f);

        PlayerActionManager.Instance.RemoveLock(this);
        endOpenCallback?.Invoke();
    }
}
