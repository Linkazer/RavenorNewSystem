using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScriptable //Obsol�te ?
{
    [SerializeField] private StatusData status;
    [SerializeField] private float duration;

    public StatusData Status => status;
    public float Duration => duration;
}
