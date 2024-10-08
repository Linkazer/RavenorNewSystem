using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor.TerrainTools;
using System;
using Unity.VisualScripting;

/// <summary>
/// Handle a Room.
/// </summary>
public class RoomHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera roomOpeningCamera;

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

        if (roomOpeningCamera != null)
        {
            roomOpeningCamera.enabled = true;

            yield return new WaitForSeconds(1f);
        }

        Debug.Log(openningCutscene);

        if (openningCutscene != null)
        {
            openningCutscene.PlaySequence(() => StartCoroutine(EndOpeningRoomSequence()));
        }
        else
        {
            StartCoroutine(EndOpeningRoomSequence());
        }
    }

    private IEnumerator EndOpeningRoomSequence()
    {
        if (roomOpeningCamera != null)
        {
            roomOpeningCamera.enabled = false;

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        PlayerActionManager.Instance.RemoveLock(this);
        endOpenCallback?.Invoke();
    }
}
