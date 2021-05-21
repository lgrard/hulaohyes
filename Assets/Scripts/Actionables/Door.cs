using UnityEngine;
using System.Collections;

namespace hulaohyes.Assets.Scripts.Actionables
{
    public class Door : Actionable
    {
        public override void DoAction()
        {
            Debug.Log("une porte s'ouvre");
            base.DoAction();
        }
    }
}