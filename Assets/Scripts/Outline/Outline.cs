using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField, Tooltip("The color that will be changed")] private Color targetColor = Color.black;
    [SerializeField] private Color outlineColor = Color.white;

    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer[] seconderyRenderers;

    public void SetOutline()
    {
        renderer.material.SetColor("_Color1in", targetColor);
        renderer.material.SetColor("_Color1out", outlineColor);

        foreach(SpriteRenderer rnd in seconderyRenderers)
        {
            rnd.material.SetColor("_Color1in", targetColor);
            rnd.material.SetColor("_Color1out", outlineColor);
        }
    }

    public void UnsetOutline()
    {
        renderer.material.SetColor("_Color1in", Color.white);
        renderer.material.SetColor("_Color1out", Color.white);

        foreach (SpriteRenderer rnd in seconderyRenderers)
        {
            rnd.material.SetColor("_Color1in", Color.white);
            rnd.material.SetColor("_Color1out", Color.white);
        }
    }
}
