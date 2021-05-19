using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hulaohyes.Assets.Scripts.Actionables
{
    public abstract class Actionable : MonoBehaviour
    {
        public virtual void DoAction() { }
        public virtual void CancelAction() { }
    }
}
