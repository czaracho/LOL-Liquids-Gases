using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{
    public enum PieceType { straight, pipeL, pipeT}
    public PieceType piece;

    private bool isSelected = false;
    public bool isPlaced = false;
    private bool isRotating = false;
    private float currentAngle = 0f;
    private float nextAngle = 90f;
    public  float[] rotationLimits;
    PipePlacementManager pipePlacementManager;

    private void Start()
    {
        pipePlacementManager = PipePlacementManager.instance;

    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Metaball_liquid") {
            switch (piece){
                case PieceType.straight:                  
                    break;
                case PieceType.pipeL:
                    break;
                case PieceType.pipeT:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!isPlaced)
        {
            isSelected = true;
            pipePlacementManager.pieceSelected = this;
            transform.DOScale(3.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
        }
        else
        {
            if (!isRotating)
            {
                RotatePiece();
            }
        }
    }

    public void MoveToTile(Transform tile) {
        DOTween.KillAll();
        transform.localScale = new Vector3(3, 3, 3);
        isPlaced = true;
        transform.DOMove(new Vector2(tile.position.x, tile.position.y), 0.8f);
        pipePlacementManager.resetPiece();
    }

    public void RotatePiece()
    {
        isRotating = true;
        transform.DORotate(new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z + 90f), 0.4f, RotateMode.WorldAxisAdd);
        StartCoroutine(allowRotation());
    }

    IEnumerator allowRotation() {
        yield return new WaitForSeconds(0.7f);
        transform.rotation = Quaternion.Euler(0, 0, nextAngle);

        switch (nextAngle)
        {
            case 0f:
                nextAngle = 90f;
                break;
            case 90f:
                nextAngle = 180f;
                break;
            case 180f:
                nextAngle = -90f;
                break;
            case -90f:
                nextAngle = 0f;
                break;
            default:
                break;
        }

        isRotating = false;
    }
}
