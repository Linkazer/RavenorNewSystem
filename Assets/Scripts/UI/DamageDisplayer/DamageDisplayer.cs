using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageDisplayer : MonoBehaviour
{
    [SerializeField] private Color damageColor;
    [SerializeField] private Color healColor;

    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpPower;
    [SerializeField] private float displayDuration;
    [SerializeField] private Vector2 leftPosition;
    [SerializeField] private Vector2 rightPosition;

    [SerializeField] private RectTransform textTransform;
    [SerializeField] private TextMeshProUGUI textHolder;

    public void Display(int amountToDisplay, bool isDamage)
    {
        if(isDamage)
        {
            textHolder.color = damageColor;
        }
        else
        {
            textHolder.color = healColor;
        }
        textHolder.text = amountToDisplay.ToString();

        textTransform.DOJumpAnchorPos(textTransform.anchoredPosition - new Vector2(Random.Range(leftPosition.x, rightPosition.x), Random.Range(leftPosition.y, rightPosition.x)), jumpPower, 1, displayDuration).SetEase(jumpCurve).OnComplete(CompleteTween);
    }

    private void CompleteTween()
    {
        Destroy(gameObject);//TODO : Pool
    }
}
