using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RVN_Text
{
    [SerializeField, TextArea(2,5)] private string frenchText = "";
    [SerializeField, TextArea(2,5)] private string englishText = "";

    public string GetText()
    {
        switch(LanguageManager.Language)
        {
            case PossibleLanguage.Francais:
                if (frenchText != "")
                {
                    return frenchText;
                }
                break;
            case PossibleLanguage.English:
                if (englishText != "")
                {
                    return englishText;
                }
                break;
        }

        return "";
    }
}
