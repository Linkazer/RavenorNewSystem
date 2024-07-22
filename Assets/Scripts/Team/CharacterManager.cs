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

    public void UpdateCharacterHostility(Entity toUpdate)
    {
        BattleManager.Instance.OnCharacterUpdateHostility(toUpdate);
    }

    public static int AreCharacterAlly(Entity e1, Entity e2)
    {
        if(e1.Hostility == CharacterHostility.Neutral || e2.Hostility == CharacterHostility.Neutral)
        {
            return 0;
        }

        switch(e1.Hostility)
        {
            case CharacterHostility.Ally:
                switch(e2.Hostility)
                {
                    case CharacterHostility.Ally:
                        return 1;
                    case CharacterHostility.Hostile:
                        return -1;
                }
                break;
            case CharacterHostility.Hostile:
                switch (e2.Hostility)
                {
                    case CharacterHostility.Ally:
                        return -1;
                    case CharacterHostility.Hostile:
                        return 1;
                }
                break;
        }

        return 0;
    }
}
