using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private List<CharacterEntity> activeCharacters = new List<CharacterEntity>();

    public List<CharacterEntity> ActiveCharacters => activeCharacters;

    public void AddActiveCharacter(CharacterEntity toAdd)
    {
        if(!activeCharacters.Contains(toAdd))
        {
            activeCharacters.Add(toAdd);

            if (BattleRoundManager.Instance.IsBattleRunning)
            {
                BattleRoundManager.Instance.AddCharacterInBattle(toAdd);
            }
            else if(toAdd.Hostility == CharacterHostility.Hostile)
            {
                BattleRoundManager.Instance.StartBattle();
            }
        }
    }

    public void RemoveActiveCharacter(Entity toRemove)
    {
        if (toRemove is CharacterEntity)
        {
            if (activeCharacters.Contains(toRemove as CharacterEntity))
            {
                activeCharacters.Remove(toRemove as CharacterEntity);

                if (BattleRoundManager.Instance.IsBattleRunning)
                {
                    BattleRoundManager.Instance.RemoveCharacterFromBattle(toRemove);
                }
            }
        }
    }

    public void UpdateCharacterHostility(CharacterEntity toUpdate)
    {
        BattleRoundManager.Instance.OnCharacterUpdateHostility(toUpdate);
    }
}
