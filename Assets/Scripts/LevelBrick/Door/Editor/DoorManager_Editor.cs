﻿using System.Collections;
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
        if (GUILayout.Button("+ Add a new Slab"))
        {
            GameObject lSlabObject = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Bricks/Slab"), lDoorManager.transform);
            lSlabObject.name = lDoorManager.SlabList.Count.ToString()+"_Slab";
            Slab lSlab = lSlabObject.GetComponent<Slab>();
            lDoorManager.SlabList.Add(lSlab);
            Selection.activeGameObject = lSlabObject as GameObject;
            Debug.Log("A new slab has been added to " + lDoorManager.gameObject.name);
            EditorUtility.SetDirty(lDoorManager);
        }
        if (GUILayout.Button("+ Add a new Door"))
        {
            GameObject lDoorObject = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/Bricks/Door"), lDoorManager.transform);
            lDoorObject.name = lDoorManager.DoorList.Count.ToString() + "_Door";
            lDoorManager.DoorList.Add(lDoorObject);
            Selection.activeGameObject = lDoorObject as GameObject;
            Debug.Log("A new door has been added to " + lDoorManager.gameObject.name);
            EditorUtility.SetDirty(lDoorManager);
        }
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
            foreach (GameObject lDoor in lDoorManager.DoorList) if (lDoor != null) DestroyImmediate(lDoor);
            lDoorManager.DoorList.Clear();
            Debug.Log(lDoorManager.gameObject.name + " door list has been cleared");
            EditorUtility.SetDirty(lDoorManager);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);
        GUIStyle lRedText = new GUIStyle();
        lRedText.normal.textColor = Color.red;
        if (lDoorManager.SlabList.Count == 0) GUILayout.Label("You need to create/assign at list 1 slab", lRedText);
        if (lDoorManager.DoorList.Count == 0) GUILayout.Label("You need to assign a door to this group", lRedText);
    }
}
