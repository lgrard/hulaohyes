using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.effects
{
    public class Effects
    {
        public static IEnumerator HitStop(Animator pPlayerAnimator, Animator pTargetAnimator, float pDuration, float pSlowAmount)
        {
            pPlayerAnimator.speed = pSlowAmount;
            pTargetAnimator.speed = pSlowAmount;
            yield return new WaitForSeconds(pDuration);
            pPlayerAnimator.speed = 1;
            pTargetAnimator.speed = 1;
        }

        public static IEnumerator ScreenShake(float pMagnitude, float pDuration, Transform pCamera)
        {
            //Set camera shake
            yield return new WaitForSeconds(pDuration);
            //disable camera shake
        }
    }
}
