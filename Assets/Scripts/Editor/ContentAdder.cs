﻿using UnityEngine;
using UnityEditor;
using hulaohyes.levelbrick.door;

public class ContentAdder : MonoBehaviour
{
    public static GameObject getItemFromPrefab(string pFilePath)
    {
        Object lItem = Resources.Load(pFilePath);
        if (lItem != null)
            return lItem as GameObject;

        else
            return null;
    }

    public static GameObject SpawnDoorGroup()
    {
        GameObject lDoorGroup = PrefabUtility.InstantiatePrefab(getItemFromPrefab("Prefabs/Bricks/DoorGroup")) as GameObject;
        GameObject lDoor = PrefabUtility.InstantiatePrefab(getItemFromPrefab("Prefabs/Bricks/Door")) as GameObject;
        lDoor.transform.parent = lDoorGroup.transform;
        lDoor.name = "0_Door";

        GameObject lSlabObject = PrefabUtility.InstantiatePrefab(getItemFromPrefab("Prefabs/Bricks/Slab")) as GameObject;
        Slab lSlab = lSlabObject.GetComponent<Slab>();
        lSlabObject.transform.parent = lDoorGroup.transform;
        lSlabObject.name = "0_Slab";

        if (lDoorGroup.TryGetComponent<DoorManager>(out DoorManager lDoorManager))
        {
            lDoorManager.DoorList.Add(lDoor);
            lDoorManager.SlabList.Add(lSlab);
        }

        return lDoorGroup;
    }
}
