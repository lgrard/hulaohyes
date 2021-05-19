using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// Returns the closest target to the origin
    /// <param name="pOrigin">The origin to compare</param>
    /// <param name="pColliders">The array of collider to compare with the origin</param>
    /// <returns>Closest target in array (null if array is empty)</returns>
    public static Transform GetClosestTarget(Transform pOrigin, Collider[] pColliders)
    {
        Transform lClosestTarget = null;
        float lClosestDistance = 0;

        foreach (Collider lTarget in pColliders)
        {
            if (!lTarget.isTrigger)
            {
                float lTargetDistance = Vector3.Distance(pOrigin.transform.position, lTarget.transform.position);

                if (lClosestTarget == null)
                {
                    lClosestTarget = lTarget.transform;
                    lClosestDistance = lTargetDistance;
                }
                else if (lClosestTarget != null && lTargetDistance < lClosestDistance) lClosestTarget = lTarget.transform;
            }
        }

        return lClosestTarget;
    }
}
