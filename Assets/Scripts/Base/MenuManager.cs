using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
public class MenuManager : Singleton<MenuManager>
{
    public StartMenu StartMenu;
    public PlayTimeMenu PlayTimeMenu;
    public PauseMenu PauseMenu;
    public FinishMenu FinishMenu;
    public Canvas Canvas;

    public async Task<Task> Setup()
    {
        StartMenu = FindObjectOfType<StartMenu>();
        PlayTimeMenu = FindObjectOfType<PlayTimeMenu>();
        PauseMenu = FindObjectOfType<PauseMenu>();
        FinishMenu = FindObjectOfType<FinishMenu>();

        await Task.Delay(100);
        
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        return Task.CompletedTask;
    }

    private void OnEnable()
    {
        EventManager.FirstTouch += ShowPlayTimeMenu;
        EventManager.OnPause += Pause;
        EventManager.FinishGame += WhenFinish;
        EventManager.OnAfterLoadedLevel += ShowStartMenu;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventManager.FirstTouch -= ShowPlayTimeMenu;
        EventManager.FinishGame -= WhenFinish;
        EventManager.OnPause -= Pause;
        EventManager.OnAfterLoadedLevel -= ShowStartMenu;
    }
    public void ShowStartMenu()
    {
        StartMenu.Show();
    }

    public void ShowPlayTimeMenu()
    {
        PlayTimeMenu.Show();
    }

    private void WhenFinish(GameStat stat)
    {
        StartMenu.Hide();
        PlayTimeMenu.Hide();
        PauseMenu.Hide();
    }

    private void Pause(bool pause)
    {
        if (pause)
        {
            GameBase.Instance.ChangeStat(GameStat.Paused);
            Time.timeScale = 0;
            PauseMenu.Show();
        }
        else
        {
            GameBase.Instance.ChangeStat(GameStat.Playing);
            Time.timeScale = 1;
            PauseMenu.Hide();
        }
    }



}
