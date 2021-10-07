using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SlideAnim : MonoBehaviour
{
    public SkeletonAnimation slideSkeleton;
    Skeleton sr;
    public float fadeDuration = 1f;
    private float t = 0;
    [HideInInspector]
    public bool fadeIn = true;
    public bool isStartingSlideImg = false;

    private void Start()
    {
        sr = slideSkeleton.GetComponent<Spine.Unity.SkeletonRenderer>().Skeleton;
        if (!isStartingSlideImg)
        {
            sr.A = 0;
            FadeIn();
        }
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (!isStartingSlideImg) {
                FadeIn();
            }            
        }
        else { 
            FadeOut();
        }
        
    }


    public void FadeIn() {

        Color currentColor = Color.Lerp(new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), t);

        sr.SetColor(currentColor);

        if (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
        }
    }


    public void FadeOut() {
        
        Color currentColor = Color.Lerp(new Color(1, 1, 1, 1), new Color(0, 0, 0, 0), t * 3f);

        sr.SetColor(currentColor);

        if (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
        }
    }

    public void ResetTime() {
        t = 0;
    }
}
