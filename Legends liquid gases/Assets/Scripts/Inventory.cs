using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform piecesContainer;
    Transform[] inventorySlots;
    Transform[] pieces;

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

}
