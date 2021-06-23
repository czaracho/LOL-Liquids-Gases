using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Inventory : MonoBehaviour
{
    public GameObject pipeStraight;
    public GameObject pipeL;
    public GameObject pipeT;
    public GameObject burner;
    Transform[] inventorySlots;
    public GameObject dialogBubble;
    public float bubbleScale = 1f;
    public float bubbleScaleSpeed = 0.25f;
    public Piece.PieceType[] pieceType;



    private void Awake()
    {
        inventorySlots = new Transform[transform.GetChild(0).childCount];

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = transform.GetChild(0).GetChild(i);
        }
    }

    private void Start()
    {
        bubbleScale = 1 + (bubbleScale / 100);
        dialogBubble.transform.DOScale(dialogBubble.transform.localScale.x * bubbleScale, bubbleScaleSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
        
        for (int i = 0; i < pieceType.Length; i++)
        {
            instantiatePipes(pieceType[i], inventorySlots[i].transform.position);
        }
    }

    public void instantiatePipes(Piece.PieceType type, Vector3 position) {

        switch (type) {
            case Piece.PieceType.straight:
                GameObject pipe = (GameObject)Instantiate(pipeStraight, position, Quaternion.identity);
                break;
            case Piece.PieceType.pipeL:
                GameObject pipeLshape = (GameObject)Instantiate(pipeL, position, Quaternion.identity);
                break;
            case Piece.PieceType.pipeT:
                GameObject pipeTshape = (GameObject)Instantiate(pipeT, position, Quaternion.identity);
                break;
            case Piece.PieceType.burner:
                GameObject burnerShape = (GameObject)Instantiate(burner, position, Quaternion.identity);
                break;

        }
    }

}
