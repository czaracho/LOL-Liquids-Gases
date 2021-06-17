using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Piece : MonoBehaviour
{
    public enum PieceType { straight, pipeL, pipeT }
    public PieceType piece;
    public bool pieceStartOnBoard = false;

    [HideInInspector]
    public bool isSelected = false;
    public bool isPlaced = false;
    private bool isRotating = false;
    private int nextAngle;
    public int currentAngle = 0;
    PipePlacementManager pipePlacementManager;
    private int[] startingAngles = { 0, 0, 25, 45, -45, -25 };
    private float[] startingDelay = { 0.25f, 0.5f, 0.75f};
    public Transform parentSlot;

    private void Start()
    {
        if (isPlaced)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else {

            int i = Random.Range(0, 5);
            int j = Random.Range(0,2);
            //es para que aparezca con un angulo random en la nube
            int randomAngle = startingAngles[i];
            //es para que aparezca con un delay en la animacion de subir arriba y abajo
            float randomDelay = startingDelay[j];

            transform.parent.rotation = Quaternion.Euler(0, 0, randomAngle);
            transform.parent.localScale = new Vector3(transform.parent.localScale.x * 0.85f, transform.parent.localScale.y * 0.85f, 1);

            StartCoroutine(InitTweenSequence(randomDelay));
        }

        pipePlacementManager = PipePlacementManager.instance;
        nextAngle = GetNextAngle(currentAngle);
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
        if (LevelManager.instance.waterIsPumped || pieceStartOnBoard) {
            return;
        }

        LevelManager.instance.RestartToNonSelectedPiece();

        isSelected = true;

        if (!isPlaced)
        {
            DOTween.Pause("float" + transform.parent.name);
            pipePlacementManager.pieceSelected = this;
            transform.parent.DOScale(transform.parent.localScale.x * 1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased().SetId("pulsing" + transform.parent.name);
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
        DOTween.Kill("float" + transform.parent.name);
        DOTween.Kill("pulsing" + transform.parent.name);
        transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        LevelManager.instance.addMoveCounter();
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

    IEnumerator InitTweenSequence(float randomDelay) {
        yield return new WaitForSeconds(randomDelay);
        transform.parent.DOMoveY(transform.parent.position.y + 0.25f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetId("float" + transform.parent.name).SetAutoKill(false);
    }

    public void CancelPulsatingAnimation() {
        DOTween.Kill("pulsing" + transform.parent.name);

        if (!isPlaced) {
            transform.parent.localScale = new Vector3(0.85f, 0.85f, 1);

        }
    }

    public void ResumeFloatAnimation() {
        DOTween.Play("float" + transform.parent.name);
    }

}
