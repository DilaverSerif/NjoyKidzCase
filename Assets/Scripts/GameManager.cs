using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Time;
    private void OnEnable()
    {
        EventManager.FirstTouch += StartTimer;
    }

    private void OnDisable()
    {
        EventManager.FirstTouch -= StartTimer;
    }
    
    private void StartTimer()
    {
        StartCoroutine(Timer());
    }
    
    private IEnumerator Timer()
    {
        while (Time > 0)
        {
            yield return new WaitForSeconds(1);
            Time--;
        }
        
        Base.FinisGame(GameStat.Win);
    }
}
