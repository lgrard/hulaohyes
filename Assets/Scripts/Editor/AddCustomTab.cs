using UnityEngine;
using UnityEditor;

public class AddCustomTab : MonoBehaviour
{
    [MenuItem("HulaOhYes/Player")] static void newPlayer() => newItemFromPrefab("Prefabs/Player");
    [MenuItem("HulaOhYes/Managers/GameManager")] static void newGameManager() => newItemFromPrefab("Prefabs/Managers/GameManager");
    [MenuItem("HulaOhYes/Managers/CameraManager")] static void newCameraManager() => newItemFromPrefab("Prefabs/Managers/CameraManager");
    [MenuItem("HulaOhYes/Cameras/Look At Camera")] static void newLookAtCamera() => newItemFromPrefab("Prefabs/Cameras/LookAtCamera");
    [MenuItem("HulaOhYes/Cameras/Angled Camera")] static void newAngledCamera() => newItemFromPrefab("Prefabs/Cameras/AngledCamera");


    static void newItemFromPrefab(string pFilePath)
    {
        Object lItem = Resources.Load(pFilePath);
        if (lItem != null) PrefabUtility.InstantiatePrefab(lItem);
        else Debug.LogError(pFilePath + " is not a valid pathname");
    }
}
