using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeImpulser : MonoBehaviour
{
    public Piece piece;
    public bool pieceHasFixedPlace = false;
    public bool pieceIsPlaced = false;
    [HideInInspector]
    public string direction = "down";

    public void adjustDirection(int newAngle)
    {
        switch (newAngle)
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
    }
}
