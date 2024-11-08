using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceCutsceneManager : Singleton<SequenceCutsceneManager>
{
    private List<SequenceCutscene> lockCutscenes = new List<SequenceCutscene>();

    public void LockForCutscene(SequenceCutscene cutscene)
    {
        if(!lockCutscenes.Contains(cutscene))
        {
            lockCutscenes.Add(cutscene);

            if(lockCutscenes.Count == 1)
            {
                PlayerActionManager.Instance.AddLock(this);
                EnnemyBattleController.Instance.AddLock(this);
                RoundManager.Instance.AddLock(this);
            }
        }
    }

    public void UnlockForCutscene(SequenceCutscene cutscene)
    {
        if (lockCutscenes.Contains(cutscene))
        {
            lockCutscenes.Remove(cutscene);

            if (lockCutscenes.Count == 0)
            {
                PlayerActionManager.Instance.RemoveLock(this);
                EnnemyBattleController.Instance.RemoveLock(this);
                RoundManager.Instance.RemoveLock(this);
            }
        }
    }
}
