using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_DIAL_DialogueLauncher : MonoBehaviour
{
    [SerializeField] private DialogueScriptable scriptable;

    [ContextMenu("Play Dialogue")]
    public void LaunchDialogue()
    {
        DialogueManager.Instance.PlayDialogue(scriptable, OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        Debug.Log("dialogue ended");
    }
}
