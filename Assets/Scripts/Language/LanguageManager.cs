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
}
