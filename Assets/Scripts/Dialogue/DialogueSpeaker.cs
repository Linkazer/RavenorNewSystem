using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker Scriptable", menuName = "Dialogue/New Speaker")]
public class DialogueSpeaker : ScriptableObject
{
    [SerializeField] private RVN_Text speakerName;
    [SerializeField] private Color speakerNameColor = Color.black;
    [SerializeField] private Sprite speakerPortrait;

    public string Name => speakerName.GetText();
    public Color NameColor => speakerNameColor;
    public Sprite Portrait => speakerPortrait;
}
