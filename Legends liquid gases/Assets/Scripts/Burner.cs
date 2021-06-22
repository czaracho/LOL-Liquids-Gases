using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public Water2D.Water2D_Spawner SmokeSpawner;
    bool isActive = false;
    float counter = 0f;


    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {
            collision.gameObject.SetActive(false);
            SmokeSpawner.StartSmokeSpawner();
        }
    }


}
