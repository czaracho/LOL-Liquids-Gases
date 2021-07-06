using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CatAnim : MonoBehaviour
{
    public SkeletonAnimation catSkeleton;
    public AnimationReferenceAsset idleAnim;
    public AnimationReferenceAsset readingAnim;
    public AnimationReferenceAsset happyAnim;
    public AnimationReferenceAsset movingAnim;
    public string currentState;


    private void Start()
    {
        EventManager.instance.PlayCatAnimationTrigger += SetCharacterState;
    }

    private void OnDestroy()
    {
        EventManager.instance.PlayCatAnimationTrigger -= SetCharacterState;
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale) {
        TrackEntry entry = catSkeleton.state.SetAnimation(1, animation, loop);

        if (animation != happyAnim) {
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
                SetAnimation(readingAnim, true, 1f);
                break;
            case "happy":
                SetAnimation(happyAnim, true, 1f);
                break;
            case "moving":
                SetAnimation(movingAnim, false, 1f);
                break;
            default:
                break;
        }
    }

    private void AnimationEntry_Complete(TrackEntry trackEntry) {
        catSkeleton.state.SetAnimation(1, idleAnim, true).TimeScale = 1;
    }
}
