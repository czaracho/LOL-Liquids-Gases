using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawnerCustom : MonoBehaviour
{
    public Water2D.Water2D_Spawner waterSpawner;
    public SwitchOnOff switchOnOff;

    public void SpawnTheWater() {

        switchOnOff.SwitchOn();
        waterSpawner.instance.StartWaterSpawner();      
    }
}
