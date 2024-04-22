using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillRessourceHelper
{
    public static SkillRessource GetNewRessourceFromEnum(SkillRessourceType type)
    {
        switch(type)
        {
            case SkillRessourceType.None:
                return null;
            case SkillRessourceType.Maana:
                return new SKL_RSC_Maana();
            case SkillRessourceType.Chant:
                return new SKL_RSC_Chanting();
        }

        return null;
    }
}
