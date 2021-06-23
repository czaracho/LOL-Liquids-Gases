using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchOnOff : MonoBehaviour
{
    public GameObject onSprite;
    public GameObject offSprite;
    private bool switchOn = false;
    SpriteRenderer sprtBurnerOn;

    private void Awake()
    {
        sprtBurnerOn = onSprite.GetComponent<SpriteRenderer>();
    }

    public void SwitchOn() {

        if (switchOn == false) {
            switchOn = true;
            offSprite.SetActive(false);
            onSprite.SetActive(true);
        }
    }

    public void TurnOffSwitch() {
        offSprite.SetActive(true);
        onSprite.SetActive(false);
    }

    public void BurnerGlow() {
        Sequence s = DOTween.Sequence();
        sprtBurnerOn.DOFade(1, 0.85f).SetLoops(-1, LoopType.Yoyo).SetId("glow" + this.gameObject.transform.name);
    }

    public void BurnerGlowFade() {
        DOTween.Kill("glow" + this.gameObject.transform.name);
        sprtBurnerOn.DOFade(0, 1);

    }
}
