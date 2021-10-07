using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public Water2D.Water2D_Spawner SmokeSpawner;

    public enum ManipulatorType { burner, condenser };
    public ManipulatorType manipulatorType = ManipulatorType.burner;
    public SwitchOnOff switchOnOff;
    bool manipulatorIsActive = false;
    float counter = 0f;
    float checkRate = 0.4f;

    private void Update()
    {
        if (manipulatorIsActive)
        {
            if (counter >= checkRate)
            {
                switchOnOff.BurnerGlowFade();
                manipulatorIsActive = false;
                SmokeSpawner.StopSpawning();
            }

            counter = counter + Time.deltaTime;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {

            if (manipulatorType == ManipulatorType.burner) {
                ActivateRespawner();
            }
            if (manipulatorType == ManipulatorType.condenser)
            {
                //congelar la tuberia
                collision.gameObject.SetActive(false);
            }

        }
        else if (collision.tag == "Smoke") {
            ActivateRespawner();
        }
    }

    void ActivateRespawner() {

        counter = 0;

        if (manipulatorIsActive == false)
        {
            switchOnOff.BurnerGlow();
            manipulatorIsActive = true;
            StartCoroutine(waitToActivateRespawner());
        }
    }

    IEnumerator waitToActivateRespawner() {
        
        yield return new WaitForSeconds(0.35f);
        SmokeSpawner.StartSmokeSpawner();

    }


}
