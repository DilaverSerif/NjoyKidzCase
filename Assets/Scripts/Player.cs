using System;
using Lean.Touch;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Action<Vector3> OnClickCell;
    
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
    }
    
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
    }
    
    private void OnFingerDown(LeanFinger obj)
    {
        var pos = obj.GetWorldPosition(5);
        OnClickCell?.Invoke(pos);
    }
}
