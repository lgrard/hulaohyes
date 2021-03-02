using UnityEngine;
using UnityEditor;
using hulaohyes.debugtool;
using hulaohyes.levelbrick.door;

public class AddCustomTab : MonoBehaviour
{
    [MenuItem("HulaOhYes/Player")] static void newPlayer() => newItemFromPrefab("Prefabs/Player");
    [MenuItem("HulaOhYes/Managers/GameManager")] static void newGameManager() => newItemFromPrefab("Prefabs/Managers/GameManager");
    [MenuItem("HulaOhYes/Managers/CameraManager")] static void newCameraManager() => newItemFromPrefab("Prefabs/Managers/CameraManager");
    [MenuItem("HulaOhYes/Cameras/Look At Camera")] static void newLookAtCamera() => newItemFromPrefab("Prefabs/Cameras/LookAtCamera");
    [MenuItem("HulaOhYes/Cameras/Angled Camera")] static void newAngledCamera() => newItemFromPrefab("Prefabs/Cameras/AngledCamera");
    [MenuItem("HulaOhYes/Bricks/Unit Cube Spawner")] static void newUnitCubeSpawner() => newItemFromPrefab("Prefabs/Bricks/UnitCubeSpawner");
    [MenuItem("HulaOhYes/Bricks/Door Group")] static void newDoorGroup()
    {
        GameObject lDoorGroup = newItemFromPrefab("Prefabs/Bricks/DoorGroup");
        GameObject lDoor = newItemFromPrefab("Prefabs/Bricks/Door");
        lDoor.transform.parent = lDoorGroup.transform;
        lDoor.name = "0_Door";
        Slab lSlab = newItemFromPrefab("Prefabs/Bricks/Slab").GetComponent<Slab>();
        lSlab.transform.parent = lDoorGroup.transform;
        lSlab.name = "0_Slab";
        if (lDoorGroup.TryGetComponent<DoorManager>(out DoorManager lDoorManager))
        {
            lDoorManager.DoorList.Add(lDoor);
            lDoorManager.SlabList.Add(lSlab);
        }
        Selection.activeGameObject = lDoorGroup as GameObject;
    }

    [MenuItem("HulaOhYes/DebugTools/Input Switcher")] static void newInputSwitcher()
    {
        GameObject lDebugToolObject = createDebugTool();
        if (lDebugToolObject.TryGetComponent<InputDebugTool>(out InputDebugTool pInputDebugTool))
            Debug.LogError("An input switcher is already on the current scene");

        else lDebugToolObject.AddComponent<InputDebugTool>();
    }


    static GameObject newItemFromPrefab(string pFilePath)
    {
        Object lItem = Resources.Load(pFilePath);
        if (lItem != null)
        {
            Object lPrefab = PrefabUtility.InstantiatePrefab(lItem);
            Selection.activeGameObject = lPrefab as GameObject;
            return lPrefab as GameObject;
        }
        else
        {
            Debug.LogError(pFilePath + " is not a valid pathname");
            return null;
        }
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
