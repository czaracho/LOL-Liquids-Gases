using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public Water2D.Water2D_Spawner SmokeSpawner;
    public SwitchOnOff switchOnOff;
    bool burnerIsActive = false;
    float counter = 0f;
    float checkRate = 0.4f;

    private void Update()
    {
        if (burnerIsActive)
        {
            if (counter >= checkRate)
            {
                switchOnOff.BurnerGlowFade();
                burnerIsActive = false;
                SmokeSpawner.StopSpawning();
            }

            counter = counter + Time.deltaTime;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid")
        {
            counter = 0;

            if (burnerIsActive == false)
            {
                switchOnOff.BurnerGlow();
                burnerIsActive = true;
                StartCoroutine(activateSpawner());
            }
        }
    }

    IEnumerator activateSpawner() {
        
        yield return new WaitForSeconds(0.35f);
        SmokeSpawner.StartSmokeSpawner();

    }


}
