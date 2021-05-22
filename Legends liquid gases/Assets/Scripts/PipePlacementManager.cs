using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipePlacementManager : MonoBehaviour
{
    public static PipePlacementManager instance;
    public Piece pieceSelected;

    private void Awake()
    {
        if (instance != null) {
            return;
        }

        instance = this;
    }

    public Piece getPiece() {
        return pieceSelected;
    }

    public void resetPiece() {
        pieceSelected = null;
    }
}
