using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour
{
    public enum PumpDirection { up, down, left, right }
    public PumpDirection pumpDirection;
    public GameObject PumpEnd;

    private void Awake()
    {
        //this.gameObject.transform.GetChild(0).name = pumpDirection.ToString();
        this.gameObject.name = pumpDirection.ToString(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {
            collision.gameObject.transform.position = PumpEnd.transform.position;
        }
    }
}
