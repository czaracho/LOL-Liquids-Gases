using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public Piece piece;
    [HideInInspector]
    public bool touched = false;
    [HideInInspector]
    public string direction = "down";
    [HideInInspector]
    public int angle = 0;


    public string steamDirection() {

        switch (angle)
        {
            case 0:
                direction = "down";
                break;
            case 90:
                direction = "right";
                break;
            case 180:
                direction = "up";
                break;
            case -90:
                direction = "left";
                break;
            default:
                break;
        }

        return direction;
    }

}
