using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    public static CatAnimationController instance;
    private CatAnim[] catAnims;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        catAnims = FindObjectsOfType<CatAnim>();
        EventManager.instance.PlayCatAnimationTrigger += AnimateCat;
    }

    private void OnDestroy()
    {
        EventManager.instance.PlayCatAnimationTrigger -= AnimateCat;
    }

    public void AnimateCat(string animation)
    {
        for (int i = 0; i < catAnims.Length; i++) {

            catAnims[i].SetCharacterState(animation);
        }
    }
}