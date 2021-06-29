using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenerGeneric : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //escala del logo es 0.75f
        Sequence s = DOTween.Sequence();
        s.Append(gameObject.transform.DOScale(new Vector2(0.75f * 1.1f, 0.75f * 1.1f), 0.45f));
        s.Append(gameObject.transform.DOScale(new Vector2(0.75f, 0.75f), 0.45f));
        StartCoroutine(DoLogoAnimation());
    }

    IEnumerator DoLogoAnimation() {
        yield return new WaitForSeconds(1f);
        gameObject.transform.DOScale(new Vector2(gameObject.transform.localScale.x * 1.025f, gameObject.transform.localScale.y * 1.025f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

}
