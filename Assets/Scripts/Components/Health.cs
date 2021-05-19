using System.Collections;
using UnityEngine;

namespace hulaohyes.Assets.Scripts.Components
{
    public class Health : MonoBehaviour
    {
        private int currentHp;

        public void TakeDamage(int pDamage)
        {
            currentHp -= pDamage;

            if (currentHp < 0)
            {
                ///Die
            }

            else
                return;
        }
    }
}