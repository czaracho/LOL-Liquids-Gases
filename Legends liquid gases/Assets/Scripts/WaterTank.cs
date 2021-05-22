using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour
{
    public LevelManager levelManager;
    public int waterDropLimit = 10;
    private int currentDropQuantity = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {
            if (currentDropQuantity <= waterDropLimit) {
                levelManager.AddWaterDrop();
                currentDropQuantity++;
            }
        }
    }
}
