using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int CurrentTotalStars = 0;
    public static GameController instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
}
