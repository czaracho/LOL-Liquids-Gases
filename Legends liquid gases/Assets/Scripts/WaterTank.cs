using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour
{
    LevelManager levelManager;
    public int waterDropLimit = 10;
    private int currentDropQuantity = 0;
    public SwitchOnOff switchOn;
    private bool tankIsActive = false;

    private void Awake()
    {
        GameObject lvlMan = GameObject.Find("LevelManager");
        levelManager = lvlMan.GetComponent<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid" || collision.tag == "Smoke") {

            Debug.Log("Entramos al cosito bro");

            if (currentDropQuantity <= waterDropLimit) {
                levelManager.AddWaterDrop();
                currentDropQuantity++;
            }

            if (tankIsActive == false)
            {
                tankIsActive = true;
                switchOn.SwitchOn();
            }
        }
    }
}
