﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action WaitingForClickTrigger;
    public event Action WaitForNextLevelTrigger;
    public event Action HideBubbleTrigger;
    public event Action<GameObject> ButtonClickAnimTrigger;
    public event Action StopWaterTankSoundTrigger;
    public event Action<string> PlayCatAnimationTrigger;


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

    public void OnHideBubbleTrigger() {
        HideBubbleTrigger?.Invoke();
    }

    public void OnButtonSimpleClick(GameObject button) {
        ButtonClickAnimTrigger?.Invoke(button);
    }

    public void OnStopWaterTankSoundTrigger() {
        StopWaterTankSoundTrigger?.Invoke();
    }

    public void OnPlayCatAnimationTrigger(string animation) {
        PlayCatAnimationTrigger?.Invoke(animation);
    }
}
