using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //static variables
    public static GameMaster instance;
    public int total_stars_earned = 0;
    public int current_stars_earned = 0;
    public int current_levels_unlocked = 0;
    public string last_played_level = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
