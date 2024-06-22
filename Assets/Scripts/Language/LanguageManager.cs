using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleLanguage
{
    Francais,
    English
}

public class LanguageManager : Singleton<LanguageManager>
{
    [SerializeField] private PossibleLanguage currentLanguage;

    public static PossibleLanguage Language => instance.currentLanguage;

    public static void SetLanguage(int languageIndex)
    {
        instance.currentLanguage = (PossibleLanguage)languageIndex;
    }

    //Code Review : Voir comment on pourrait faire pour mettre � jour les langues automatiquement.
    //              Pas forc�ment n�cessaire. Si on met le choix de langue uniquement dans le menu principal, on aura qu'� recharger le menu principal
}
