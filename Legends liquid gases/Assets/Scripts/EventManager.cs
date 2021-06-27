using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action WaitingForClickTrigger;
    public event Action WaitForNextLevelTrigger;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void OnWaitingForClickTrigger()
    {
        WaitingForClickTrigger?.Invoke();
    }

    public void OnWaitForNextLevelTrigger()
    {
        WaitForNextLevelTrigger?.Invoke();
    }
}
