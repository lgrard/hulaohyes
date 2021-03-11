using UnityEngine;
using UnityEditor;
using hulaohyes.debugtool;

public class AddCustomTab : MonoBehaviour
{
    [MenuItem("HulaOhYes/Player/0")] static void newPlayer0() => newItemFromPrefab("Prefabs/Player_0");
    [MenuItem("HulaOhYes/Player/1")] static void newPlayer1() => newItemFromPrefab("Prefabs/Player_1");
    [MenuItem("HulaOhYes/Managers/GameManager")] static void newGameManager() => newItemFromPrefab("Prefabs/Managers/GameManager");
    [MenuItem("HulaOhYes/Managers/CameraManager")] static void newCameraManager() => newItemFromPrefab("Prefabs/Managers/CameraManager");
    [MenuItem("HulaOhYes/Cameras/Look At Camera")] static void newLookAtCamera() => newItemFromPrefab("Prefabs/Cameras/LookAtCamera");
    [MenuItem("HulaOhYes/Cameras/Angled Camera")] static void newAngledCamera() => newItemFromPrefab("Prefabs/Cameras/AngledCamera");
    [MenuItem("HulaOhYes/Enemy/Walker")] static void newEnemyWalker() => newItemFromPrefab("Prefabs/Enemy/Enemy_Walker");
    [MenuItem("HulaOhYes/Enemy/Turret")] static void newEnemyTurret() => newItemFromPrefab("Prefabs/Enemy/Enemy_Turret");
    [MenuItem("HulaOhYes/Bricks/Unit Cube Spawner")] static void newUnitCubeSpawner() => newItemFromPrefab("Prefabs/Bricks/UnitCubeSpawner");
    [MenuItem("HulaOhYes/Bricks/Checkpoints/Checkpoint")] static void newCheckPoint() => newItemFromPrefab("Prefabs/Bricks/Checkpoints/Checkpoint");
    [MenuItem("HulaOhYes/Bricks/Checkpoints/Checkpoint")] static void newLevelEnd() => newItemFromPrefab("Prefabs/Bricks/Checkpoints/LevelEnd");
    [MenuItem("HulaOhYes/Bricks/Checkpoints/Big Checkpoint")] static void newBigCheckPoint() => newItemFromPrefab("Prefabs/Bricks/Checkpoints/BigCheckpoint");
    [MenuItem("HulaOhYes/Bricks/Door Group")] static void newDoorGroup() => ContentAdder.SpawnDoorGroup();
    [MenuItem("HulaOhYes/DebugTools/Input Switcher")] static void newInputSwitcher()
    {
        GameObject lDebugToolObject = createDebugTool();
        if (lDebugToolObject.TryGetComponent<InputDebugTool>(out InputDebugTool pInputDebugTool))
            Debug.LogError("An input switcher is already on the current scene");

        else lDebugToolObject.AddComponent<InputDebugTool>();
    }
    [MenuItem("HulaOhYes/Bricks/Checkpoints/Player Start")] static void newPlayerStart()
    {
        GameObject lPlayerStart = GameObject.FindGameObjectWithTag("PlayerStart");

        if (lPlayerStart != null)
        {
            Selection.activeObject = lPlayerStart;
            Debug.LogError("A player Start is already on the current scene");
        }

        else newItemFromPrefab("Prefabs/Bricks/Checkpoints/PlayerStart");
    }

    public static GameObject newItemFromPrefab(string pFilePath)
    {
        GameObject lItem = ContentAdder.getItemFromPrefab(pFilePath);

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
