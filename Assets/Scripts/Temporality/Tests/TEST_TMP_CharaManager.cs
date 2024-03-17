using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TMP_CharaManager : MonoBehaviour
{
    [SerializeField] private List<TEST_TMP_Chara> charas;

    private bool isInCombat = false;

    private int currentCharacterIndex;

    private void Start()
    {
        RoundManager.Instance.SetRoundMode(RoundMode.RealTime);
    }

    public void ChangeCombatState()
    {
        if (!isInCombat)
        {
            foreach(TEST_TMP_Chara chara in charas)
            {
                RoundManager.Instance.RemoveHandler(chara);
            }

            isInCombat = true;
            RoundManager.Instance.SetRoundMode(RoundMode.Round);
            currentCharacterIndex = 0;
            StartCharaTurn();
        }
        else
        {
            foreach (TEST_TMP_Chara chara in charas)
            {
                RoundManager.Instance.AddHandler(chara);
            }

            isInCombat = false;
            RoundManager.Instance.SetRoundMode(RoundMode.RealTime);
        }
    }

    public void StartCharaTurn()
    {
        //charas[currentCharacterIndex].StartRound();
    }

    public void EndCharaTurn()
    {
        //charas[currentCharacterIndex].EndRound();

        currentCharacterIndex++;
        if(currentCharacterIndex >= charas.Count)
        {
            RoundManager.Instance.EndGlobalRound();
            currentCharacterIndex = 0;
        }

        StartCharaTurn();
    }

    public void AddEffect()
    {
        TEST_TMP_Effect newEffect = new TEST_TMP_Effect();

        newEffect.Apply(charas[currentCharacterIndex]);

        charas[currentCharacterIndex].effects = newEffect;
    }
}
