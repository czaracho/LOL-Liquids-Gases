using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMouth : MonoBehaviour
{
    public int holeId = 0;
    public Piece parentPiece; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {
            //parentPiece.pipeImpulser.DeactivateHoles(this);
        }
    }
}
