using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer xSpriteRenderer;
    public Color BaseColor, SuccColor;
    public bool isX;

    private void Awake()
    {
        xSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OpenX(bool open = true)
    {
        // xSpriteRenderer.DOFade(open ? 1 : 0, 0.5f);
        isX = open;
        
        if (open)
        {
            xSpriteRenderer.color = BaseColor;
            xSpriteRenderer.transform.localScale = Vector3.zero;
            xSpriteRenderer.DOFade(1, 0.5F);
            xSpriteRenderer.transform.DOScale(Vector3.one, 0.35f);
        }
        else
        {
            xSpriteRenderer.DOFade(0, 0.5F).SetDelay(0.25f);
            xSpriteRenderer.transform.DOPunchPosition(Vector3.one * 0.1f, 0.5F).SetDelay(0.25f);
            xSpriteRenderer.DOColor(SuccColor, 0.25f).SetDelay(0.25f);
        }
    }
}
