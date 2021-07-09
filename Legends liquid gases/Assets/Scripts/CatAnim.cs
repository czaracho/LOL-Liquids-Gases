using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CatAnim : MonoBehaviour
{
    public string catName;
    public SkeletonAnimation catSkeleton;
    public AnimationReferenceAsset idleAnim;
    public AnimationReferenceAsset readingAnim;
    public AnimationReferenceAsset celebrationAnim;
    public AnimationReferenceAsset movingAnim;


    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale) {
        TrackEntry entry = catSkeleton.state.SetAnimation(0, animation, loop);

        if (animation != celebrationAnim) {
            entry.Complete += AnimationEntry_Complete;
        }
    }

    public void SetCharacterState(string state) {

        switch (state)
        {
            case "idle":
                SetAnimation(idleAnim, true, 1f);
                break;
            case "reading":
                if (catName == "lana") {
                    SetAnimation(readingAnim, true, 1f);
                }
                break;
            case "celebration":
                SetAnimation(celebrationAnim, true, 1f);
                break;
            case "moving":
                if (catName == "pebbles") {
                    SetAnimation(movingAnim, false, 1f);
                }
                break;
            default:
                break;
        }
    }

    private void AnimationEntry_Complete(TrackEntry trackEntry) {
        catSkeleton.state.SetAnimation(1, idleAnim, true).TimeScale = 1;
    }
}
