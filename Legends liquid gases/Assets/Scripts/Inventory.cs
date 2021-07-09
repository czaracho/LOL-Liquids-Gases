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
    public GameObject condenser;
    Transform[] inventorySlots;
    public GameObject dialogBubble;
    public float bubbleScale = 1f;
    public float bubbleScaleSpeed = 0.25f;
    public Piece.PieceType[] pieceType;
    //GameObject[] pipes = new GameObject[6];
    List<GameObject> pipes = new List<GameObject>();

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
        EventManager.instance.HideBubbleTrigger += HideDialogBubble;

        bubbleScale = 1 + (bubbleScale / 100);
        dialogBubble.transform.DOScale(dialogBubble.transform.localScale.x * bubbleScale, bubbleScaleSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased().SetId("bubble"); ;
        
        for (int i = 0; i < pieceType.Length; i++)
        {
            instantiatePipes(pieceType[i], inventorySlots[i].transform.position, i);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.HideBubbleTrigger -= HideDialogBubble;
    }

    public void instantiatePipes(Piece.PieceType type, Vector3 position, int arrayPos) {

        switch (type) {
            case Piece.PieceType.straight:
                pipes.Add((GameObject)Instantiate(pipeStraight, position, Quaternion.identity));
                break;
            case Piece.PieceType.pipeL:
                pipes.Add((GameObject)Instantiate(pipeL, position, Quaternion.identity));
                break;
            case Piece.PieceType.pipeT:
                pipes.Add((GameObject)Instantiate(pipeT, position, Quaternion.identity));
                break;
            case Piece.PieceType.burner:
                pipes.Add((GameObject)Instantiate(burner, position, Quaternion.identity));
                break;
            case Piece.PieceType.condenser:
                pipes.Add((GameObject)Instantiate(condenser, position, Quaternion.identity));
                break;
        }
    }

    public void HideDialogBubble() {

        foreach (GameObject pipe in pipes) {

            if (pipe.GetComponent<PipeImpulser>().pieceIsPlaced == false) {
                pipe.SetActive(false);
            }
        }

        Sequence s = DOTween.Sequence();
        bubbleScale = 1 + (bubbleScale / 100);
        //s.Append(dialogBubble.transform.DOScale(dialogBubble.transform.localScale.x * bubbleScale * 1.1f, 0.35f));
        s.Append(dialogBubble.transform.DOScale(0, 0.25f));
        DOTween.Kill("bubble");
    }

}
