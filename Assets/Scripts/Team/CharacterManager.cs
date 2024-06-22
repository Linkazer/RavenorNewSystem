using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le stockage de tous les personnages actifs dans le niveau
/// </summary>
public class CharacterManager : Singleton<CharacterManager>
{
    private List<CharacterEntity> activeCharacters = new List<CharacterEntity>();

    public List<CharacterEntity> ActiveCharacters => activeCharacters;

    public void AddActiveCharacter(CharacterEntity toAdd)
    {
        if(!activeCharacters.Contains(toAdd))
        {
            activeCharacters.Add(toAdd);

            if (BattleManager.Instance.IsBattleRunning)
            {
                BattleManager.Instance.AddCharacterInBattle(toAdd);
            }
            else if(toAdd.Hostility == CharacterHostility.Hostile)
            {
                BattleManager.Instance.StartBattle();
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

                if (BattleManager.Instance.IsBattleRunning)
                {
                    BattleManager.Instance.RemoveCharacterFromBattle(toRemove);
                }
            }
        }
    }

    public void UpdateCharacterHostility(CharacterEntity toUpdate)
    {
        BattleManager.Instance.OnCharacterUpdateHostility(toUpdate);
    }
}
