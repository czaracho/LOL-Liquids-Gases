using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;



public class Tile : MonoBehaviour
{
    PipePlacementManager pipePlacementManager;

    private void Start()
    {
        pipePlacementManager = PipePlacementManager.instance;
    }

    private void OnMouseDown()
    {
        PlacePiece();
    }

    public void PlacePiece()
    {
        if (pipePlacementManager.getPiece() == null)
            return;

        Piece piece = pipePlacementManager.getPiece();
        piece.MoveToTile(this.transform);
    }

}
