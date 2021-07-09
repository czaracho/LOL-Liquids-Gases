using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank : MonoBehaviour
{
    GameManagerMain levelManager;
    public int waterDropLimit = 10;
    private int currentDropQuantity = 0;
    public SwitchOnOff switchOn;
    private bool tankIsActive = false;
    AudioSource audioSource;
    bool waterIsFalling;
    float counter = 0f;
    float checkRate = 0.4f;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        GameObject lvlMan = GameObject.Find("LevelManager");
        levelManager = lvlMan.GetComponent<GameManagerMain>();
    }

    private void Start()
    {
        EventManager.instance.StopWaterTankSoundTrigger += StopBubblesSound;
    }

    private void OnDestroy()
    {
        EventManager.instance.StopWaterTankSoundTrigger -= StopBubblesSound;
    }

    private void Update()
    {
        if (waterIsFalling)
        {
            if (counter >= checkRate)
            {
                waterIsFalling = false;
                StopBubblesSound();
            }

            counter = counter + Time.deltaTime;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid" || collision.tag == "Smoke") {

            //Restart the counter only if the water is flowing and the level isn't completed
            if (currentDropQuantity < GameManagerMain.instance.requiredDropQuantity) {
                counter = 0;
            }

            if (!waterIsFalling) {
                PlayBubblesSound();
            }

            waterIsFalling = true;

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

    void PlayBubblesSound() {
        audioSource.Play();
    }

    void StopBubblesSound() {
        audioSource.Stop();
    }
}
