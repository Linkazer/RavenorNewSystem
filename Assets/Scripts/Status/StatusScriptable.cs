using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusScriptable //Obsolète ?
{
    [SerializeField] private StatusData status;
    [SerializeField] private float duration;

    public StatusData Status => status;
    public float Duration => duration;
}
