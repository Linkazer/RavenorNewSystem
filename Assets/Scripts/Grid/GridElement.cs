using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    [SerializeField] private bool walkable;

    public bool Walkable => walkable;

    public Vector2 FlatPosition => transform.position;

    public Vector3 WorldPosition => transform.position;
}
