using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridTool : EditorWindow
{
    private Material headerMat;
    private Texture headerImage;

    private Color previewColor = Color.green;
    private Color redColor = new Color(0.7f,0.2f,0.2f);
    private Color greenColor = new Color(0.2f, 0.7f, 0.2f);
    private Color yellowColor = Color.yellow;
    private Color baseColor;

    private GUIStyle titleStyle;
    private GUIStyle fileStyle;
    private Vector2 scrollView;

    private GridList currentGridList;
    private string currentFile;

    private Vector2Int cellNumber = new Vector2Int(1,1);
    private Vector2 cellPosition;

    [MenuItem("HulaOhYes/Grid tool &END")] private static void ShowWindow()
    {
        EditorWindow lWindow = EditorWindow.GetWindow(typeof(GridTool));
        lWindow.titleContent.text = "Grid tool";
    }

    private void OnEnable()
    {
        #region styles
        headerImage = (Texture)Resources.Load("Editor/CM_header");
        headerMat = (Material)Resources.Load("Editor/M_EditorSprite");
        baseColor = GUI.color;

        fileStyle = new GUIStyle();
        fileStyle.fontSize = 9;
        fileStyle.fontStyle = FontStyle.Italic;

        titleStyle = new GUIStyle();
        titleStyle.fontSize = 15;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        #endregion

        SceneView.duringSceneGui += OnSceneGUI;
        if(currentGridList == null) currentGridList = new GridList();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        if (headerImage != null && position.height > 400)
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(256, 50), headerImage, headerMat, ScaleMode.ScaleAndCrop);

        EditorGUILayout.BeginVertical();

        GUILayout.Space(15);
        GUILayout.Label("Grid tool", titleStyle);

        EditorGUILayout.LabelField("Cell preview");
        cellNumber = EditorGUILayout.Vector2IntField("", cellNumber);
        
        GUILayout.Space(20);
        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        
        if(currentGridList.gridData != null)
        {
            foreach(GridData lGridData in currentGridList.gridData)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("Select")) cellNumber = lGridData.size;
                if (GUILayout.Button("Reset")) lGridData.ResetSize();
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                lGridData.id = EditorGUILayout.TextField(lGridData.id);
                if (lGridData.size.x != lGridData.x || lGridData.size.y != lGridData.y) GUI.color = yellowColor;
                lGridData.size = EditorGUILayout.Vector2IntField("", lGridData.size);
                GUI.color = baseColor;
                EditorGUILayout.EndVertical();
                GUI.color = redColor;
                if (GUILayout.Button("-"))
                {
                    currentGridList.gridData.Remove(lGridData);
                    break;
                }
                GUI.color = baseColor;
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }
            GUILayout.Space(15);

            GUI.color = greenColor;
            if (GUILayout.Button("+"))
            {
                GridData lNewData = new GridData();
                lNewData.size = cellNumber;
                currentGridList.gridData.Add(lNewData);
            }
            GUI.color = baseColor;
        }

        EditorGUILayout.EndScrollView();


        EditorGUILayout.EndVertical();
        previewColor = EditorGUILayout.ColorField("Preview Color",previewColor);
        EditorGUILayout.LabelField("Loaded file: " + currentFile, fileStyle);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Load",GUILayout.Height(40f))) LoadData();
        if(currentGridList.gridData != null)
            if(GUILayout.Button("Save",GUILayout.Height(40f))) SaveData();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("Snap selection to grid - (Shift+S)", GUILayout.Height(40f))) SnapToGrid();
        GUILayout.Space(10);
    }

    void OnSceneGUI(SceneView pSceneView)
    {
        Handles.color = previewColor;

        Vector2 lHandlePosition = new Vector2(Mathf.FloorToInt(cellPosition.x), Mathf.FloorToInt(cellPosition.y));
        cellPosition = Handles.PositionHandle(lHandlePosition, Quaternion.identity);

        Vector2 lCellSize = cellNumber;
        Vector2 lCellPosition = cellPosition + new Vector2(lCellSize.x/2, lCellSize.y/2);
        Handles.DrawWireCube(lCellPosition, lCellSize);
    }

    void LoadData()
    {
        string lPath = EditorUtility.OpenFilePanel("Load grid datas", "Assets/Editor/Data", "json");
        if (lPath == null)
            return;
        currentFile = lPath;
        currentGridList = JsonUtility.FromJson<GridList>(System.IO.File.ReadAllText(lPath));

        foreach (GridData lGridData in currentGridList.gridData)
            lGridData.size = new Vector2Int(lGridData.x, lGridData.y);
    }

    void SaveData()
    {
        foreach (GridData lGridData in currentGridList.gridData)
            lGridData.ApplySize();

        string lPath = EditorUtility.SaveFilePanel("Save grid datas", "Assets/Editor/Data", "GridData_","json");
        string lNewJson = JsonUtility.ToJson(currentGridList);
        System.IO.File.WriteAllText(lPath, lNewJson);
    }

    [MenuItem("HulaOhYes/Snap to grid #S")]
    public static void SnapToGrid()
    {
        foreach (Transform lObjects in Selection.transforms)
        {
            float lNewX = Mathf.RoundToInt(lObjects.position.x);
            float lNewY = Mathf.RoundToInt(lObjects.position.y);
            lObjects.position = new Vector3(lNewX, lNewY, lObjects.position.z);
        }
    }
}

[System.Serializable]
public class GridData
{
    public string id;
    public int x;
    public int y;

    public Vector2Int size;

    public void ResetSize() => size = new Vector2Int(x, y);
    public void ApplySize()
    {
        x = size.x;
        y = size.y;
    }
}

[System.Serializable]
public class GridList
{
    public List<GridData> gridData;
}
