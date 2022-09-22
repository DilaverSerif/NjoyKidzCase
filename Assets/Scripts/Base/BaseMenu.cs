using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour
{
    protected Transform BG;

    protected virtual void Awake()
    {
        BG = transform.Find("BG");

        Hide();
    }

    internal void Show()
    {
        BG.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        BG.gameObject.SetActive(false);
    }
}
