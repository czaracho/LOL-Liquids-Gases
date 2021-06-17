using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Inventory : MonoBehaviour
{
    public Transform piecesContainer;
    Transform[] inventorySlots;
    Transform[] pieces;
    public GameObject dialogBubble;

    private void Awake()
    {
        inventorySlots = new Transform[transform.childCount];
        pieces = new Transform[piecesContainer.childCount];

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = transform.GetChild(i);
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = piecesContainer.transform.GetChild(i);
            pieces[i].transform.position = inventorySlots[i].transform.position;
            pieces[i].gameObject.SetActive(true);

        }
    }

    private void Start()
    {
        dialogBubble.transform.DOScale(dialogBubble.transform.localScale.x * 1.05f, 0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
    }

}
