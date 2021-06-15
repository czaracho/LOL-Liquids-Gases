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
    private int nextAngle;
    //public int startingAngle = 0;
    public int currentAngle = 0;
    PipePlacementManager pipePlacementManager;

    private void Start()
    {
        if (isPlaced)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else {
            transform.parent.localScale = new Vector3(transform.parent.localScale.x * 0.75f, transform.parent.localScale.x * 0.75f, 1);

        }

        pipePlacementManager = PipePlacementManager.instance;
        nextAngle = GetNextAngle(currentAngle);
    }

    private void Update()
    {
        //Debug.Log("NextAngle: " + nextAngle + " - Current Angle: " + currentAngle);
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
        if (LevelManager.instance.waterIsPumped) {
            return;
        }

        if (!isPlaced)
        {
            isSelected = true;
            pipePlacementManager.pieceSelected = this;
            transform.parent.DOScale(transform.parent.localScale.x * 1.1f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
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
        LevelManager.instance.addMoveCounter();
        DOTween.KillAll();
        transform.parent.parent = null;
        transform.parent.localScale = Vector3.one;
        isPlaced = true;
        transform.parent.DOMove(new Vector2(tile.position.x, tile.position.y), 0.8f);
        pipePlacementManager.resetPiece();
    }

    public void RotatePiece()
    {
        LevelManager.instance.addMoveCounter();
        isRotating = true;
        transform.parent.DORotate(new Vector3(transform.parent.rotation.x, transform.parent.rotation.y, transform.parent.rotation.z + 90f), 0.4f, RotateMode.WorldAxisAdd);
        StartCoroutine(AllowRotation());
    }

    IEnumerator AllowRotation() {
        yield return new WaitForSeconds(0.6f);
        transform.parent.rotation = Quaternion.Euler(0, 0, nextAngle);
        nextAngle = nextAngle + 90;
        
        if (nextAngle == 360) {
            nextAngle = 0;
        }
        
        isRotating = false;
    }

    private int GetNextAngle(int angle) {
        
        int newAngle = 0;

        switch (angle)
        {
            case 0:
                newAngle = 90;
                break;
            case 90:
                newAngle = 180;
                break;
            case 180:
                newAngle = -90;
                break;
            case -90:
                newAngle = 0;
                break;
            default:
                break;
        }

        return newAngle;
    }

    //Desactivamos el front collider de las piezas, para evitar tocar con el mouse
    public void DeactivatePiece() {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
