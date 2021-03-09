using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hulaohyes.levelbrick.door;

[CustomEditor(typeof(DoorManager))]
public class DoorManager_Editor : Editor
{
    private Color greenColor = new Color(0.2f, 0.65f, 0.2f, 1);
    private Color redColor = new Color(0.65f, 0.2f, 0.2f, 1);

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DoorManager lDoorManager = (DoorManager)target;

        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUI.color = greenColor;
        if (GUILayout.Button("+ Add Slab x1"))
            AddSlab(1,lDoorManager);
        if (GUILayout.Button("+ Add Slab x2"))
            AddSlab(2, lDoorManager);
        if (GUILayout.Button("+ Add Door x1"))
            AddDoor(1, lDoorManager);
        if (GUILayout.Button("+ Add Door x2"))
            AddDoor(2, lDoorManager);
        GUI.color = Color.grey;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.color = redColor;
        if (GUILayout.Button("x Clear slab list"))
        {
            foreach (Slab lSlab in lDoorManager.SlabList) if(lSlab != null) DestroyImmediate(lSlab.gameObject);
            lDoorManager.SlabList.Clear();
            Debug.Log(lDoorManager.gameObject.name + " slab list has been cleared");
            EditorUtility.SetDirty(lDoorManager);
        }
        GUI.color = redColor;
        if (GUILayout.Button("x Clear door List"))
        {
            foreach (GameObject lDoor in lDoorManager.DoorList) if (lDoor != null) DestroyImmediate(lDoor.GetComponentInParent<Door>().gameObject);
            lDoorManager.DoorList.Clear();
            Debug.Log(lDoorManager.gameObject.name + " door list has been cleared");
            EditorUtility.SetDirty(lDoorManager);
        }
        GUILayout.EndHorizontal();

        GUI.color = Color.yellow;
        if (GUILayout.Button("Update lists"))
        {
            lDoorManager.SlabList.Clear();
            lDoorManager.DoorList.Clear();
            foreach (Door lDoor in lDoorManager.GetComponentsInChildren<Door>()) lDoorManager.DoorList.Add(lDoor.moveDoor);
            foreach (Slab lSlab in lDoorManager.GetComponentsInChildren<Slab>()) lDoorManager.SlabList.Add(lSlab);
            EditorUtility.SetDirty(lDoorManager);
        }

        GUILayout.Space(10f);
        GUIStyle lRedText = new GUIStyle();
        lRedText.normal.textColor = Color.red;
        if (lDoorManager.SlabList.Count == 0) GUILayout.Label("You need to create/assign at list 1 slab", lRedText);
        if (lDoorManager.DoorList.Count == 0) GUILayout.Label("You need to assign a door to this group", lRedText);
    }

    void AddSlab(int pNumber, DoorManager pDoorManager)
    {
        GameObject lSlabObject = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Bricks/Slab_x"+pNumber.ToString()), pDoorManager.transform);
        lSlabObject.name = pDoorManager.SlabList.Count.ToString() + "_Slab_x"+pNumber.ToString();
        Slab lSlab = lSlabObject.GetComponent<Slab>();
        pDoorManager.SlabList.Add(lSlab);
        Selection.activeGameObject = lSlabObject as GameObject;
        Debug.Log("A new slab has been added to " + pDoorManager.gameObject.name);
        EditorUtility.SetDirty(pDoorManager);
    }

    void AddDoor(int pNumber, DoorManager pDoorManager)
    {
        GameObject lDoorObject = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Bricks/Door_x"+ pNumber.ToString()), pDoorManager.transform);
        lDoorObject.name = pDoorManager.DoorList.Count.ToString() + "_Door_x"+ pNumber.ToString();
        pDoorManager.DoorList.Add(lDoorObject.GetComponent<Door>().moveDoor);
        Selection.activeGameObject = lDoorObject as GameObject;
        Debug.Log("A new door has been added to " + pDoorManager.gameObject.name);
        EditorUtility.SetDirty(pDoorManager);
    }
}
