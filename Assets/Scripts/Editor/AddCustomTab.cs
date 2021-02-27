using UnityEngine;
using UnityEditor;
using hulaohyes.debugtool;

public class AddCustomTab : MonoBehaviour
{
    [MenuItem("HulaOhYes/Player")] static void newPlayer() => newItemFromPrefab("Prefabs/Player");
    [MenuItem("HulaOhYes/Managers/GameManager")] static void newGameManager() => newItemFromPrefab("Prefabs/Managers/GameManager");
    [MenuItem("HulaOhYes/Managers/CameraManager")] static void newCameraManager() => newItemFromPrefab("Prefabs/Managers/CameraManager");
    [MenuItem("HulaOhYes/Cameras/Look At Camera")] static void newLookAtCamera() => newItemFromPrefab("Prefabs/Cameras/LookAtCamera");
    [MenuItem("HulaOhYes/Cameras/Angled Camera")] static void newAngledCamera() => newItemFromPrefab("Prefabs/Cameras/AngledCamera");

    [MenuItem("HulaOhYes/DebugTools/Input Switcher")]
    static void newInputSwitcher()
    {
        GameObject lDebugToolObject = createDebugTool();
        if (lDebugToolObject.TryGetComponent<InputDebugTool>(out InputDebugTool pInputDebugTool))
            Debug.LogError("An input switcher is already on the current scene");

        else lDebugToolObject.AddComponent<InputDebugTool>();
    }


    static void newItemFromPrefab(string pFilePath)
    {
        Object lItem = Resources.Load(pFilePath);
        if (lItem != null)
        {
            Object lPrefab = PrefabUtility.InstantiatePrefab(lItem);
            Selection.activeGameObject = lPrefab as GameObject;
        }
        else Debug.LogError(pFilePath + " is not a valid pathname");
    }

    static GameObject createDebugTool()
    {
        GameObject lDebugToolObject = GameObject.FindGameObjectWithTag("Debug");

        if (GameObject.FindGameObjectWithTag("Debug") == null)
        {
            lDebugToolObject =  new GameObject();
            lDebugToolObject.name = "Debug_Tool";
            lDebugToolObject.tag = "Debug";
        }

        Selection.activeGameObject = lDebugToolObject;

        return lDebugToolObject;
    }
}
