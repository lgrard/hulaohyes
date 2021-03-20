using UnityEngine;
using UnityEditor;

class ContentManagerWindow : EditorWindow
{
    private Material headerMat;
    private Texture headerImage; 
    private float buttonWidth;
    private float buttonHeight = 35;
    private float labelWidth;
    private Vector2 scrollView;
    private GUIStyle indicationStyle;
    private GUIStyle titleStyle;
    private GUIStyle labelStyle;
    private GUIStyle fieldStyle;
    private Object objectToLoad;
    private GameObject lastObject;
    private string overridenObjectName;
    private Editor objectEditor;

    [MenuItem("HulaOhYes/Content Manager &HOME")] private static void ShowWindow() => EditorWindow.GetWindow(typeof(ContentManagerWindow)).titleContent.text = "Content Manager";

    void OnFocus()
    {
        // Remove delegate listener if it has previously been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;

        indicationStyle = new GUIStyle();
        indicationStyle.fontSize = 10;
        indicationStyle.fontStyle = FontStyle.BoldAndItalic;
        indicationStyle.alignment = TextAnchor.LowerCenter;

        titleStyle = new GUIStyle();
        titleStyle.fontSize = 15;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        labelStyle = new GUIStyle();
        labelStyle.fontSize = 12;
        labelStyle.fontStyle = FontStyle.Italic;
        labelStyle.alignment = TextAnchor.MiddleRight;
        labelStyle.padding.right = 10;

        headerImage = (Texture)Resources.Load("Editor/CM_header");
        headerMat = (Material)Resources.Load("Editor/M_EditorSprite");
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        objectToLoad = null;
        lastObject = null;
    }

    private void OnGUI()
    {
        buttonWidth = position.width / 1.5f;
        labelWidth = position.width / 4f;

        if(headerImage != null && position.height> 400)
        EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(256, 50), headerImage, headerMat, ScaleMode.ScaleAndCrop);

        GUILayout.Space(15);
        GUILayout.Label("Content Manager", titleStyle);
        GUILayout.Space(15);

        scrollView = GUILayout.BeginScrollView(scrollView);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Players", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("Player_0", GUILayout.Width(buttonWidth/2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Player_0");
        if(GUILayout.Button("Player_1", GUILayout.Width(buttonWidth/2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Player_1");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Managers", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("GameManager", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Managers/GameManager");
        if (GUILayout.Button("CameraManager", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Managers/CameraManager");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cameras", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("LookAtCamera", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Cameras/LookAtCamera");
        if (GUILayout.Button("AngledCamera", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Cameras/AngledCamera");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enemies", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("Walker", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Enemy/Enemy_Walker");
        if (GUILayout.Button("Turret", GUILayout.Width(buttonWidth / 2), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Enemy/Enemy_Turret");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Bricks", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("DoorGroup", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/DoorGroup");
        if (GUILayout.Button("UnitCubeSpawner", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/UnitCubeSpawner");
        if (GUILayout.Button("Barrel", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Barrel");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Gears", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("Gear", GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Gear");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Checkpoints", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("PlayerStart", GUILayout.Width(buttonWidth / 4), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Checkpoints/PlayerStart");
        if (GUILayout.Button("Checkpoint", GUILayout.Width(buttonWidth / 4), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Checkpoints/Checkpoint");
        if (GUILayout.Button("BigCheckpoint", GUILayout.Width(buttonWidth / 4), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Checkpoints/BigCheckpoint");
        if (GUILayout.Button("LevelEnd", GUILayout.Width(buttonWidth / 4), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/Bricks/Checkpoints/LevelEnd");
        GUILayout.EndHorizontal();

        GUILayout.Space(30f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Fence", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("x0", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x0");
        if (GUILayout.Button("x1", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x1");
        if (GUILayout.Button("x2", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x2");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Outline", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("x0", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x0");
        if (GUILayout.Button("x1", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x1");
        if (GUILayout.Button("x2", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x2");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Plank", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("x1", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x1");
        if (GUILayout.Button("x2", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x2");
        if (GUILayout.Button("x4", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x4");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pole", labelStyle, GUILayout.Width(labelWidth));
        if (GUILayout.Button("x1", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x1");
        if (GUILayout.Button("x2", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x2");
        if (GUILayout.Button("x4", GUILayout.Width(buttonWidth / 3), GUILayout.Height(buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x4");
        GUILayout.EndHorizontal();


        GUILayout.EndScrollView();

        GUILayout.Space(15f);
        if (objectToLoad != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Object name",labelStyle, GUILayout.Width(position.width / 4));
            overridenObjectName = GUILayout.TextField(overridenObjectName, GUILayout.Width(position.width/1.5f));
            GUILayout.EndHorizontal();
            
            GUILayout.Space(5f);

            GUILayout.Label("Click anywhere to create a new " + objectToLoad.name, indicationStyle);
        }
        else GUILayout.Label("Select an object", indicationStyle);

        GUILayout.Space(15f);
        if (lastObject != null)
            if (GUILayout.Button("Undo", GUILayout.Height(buttonHeight))) Undo();
        GUILayout.Space(5f);

        if (objectToLoad != null)
        {
            if(objectEditor == null)
                objectEditor = Editor.CreateEditor(objectToLoad);
            objectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 150), new GUIStyle());
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Ray lRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        Vector3 lPosition = Vector3.zero;
        if (Physics.Raycast(lRay, out hit) && !hit.collider.isTrigger) lPosition = hit.point;
        
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && objectToLoad!=null)
        {
            GameObject lNewObject;

            if (objectToLoad == ContentAdder.getItemFromPrefab("Prefabs/Bricks/DoorGroup"))
                lNewObject = ContentAdder.SpawnDoorGroup();

            else
                lNewObject = PrefabUtility.InstantiatePrefab(objectToLoad) as GameObject;

            lNewObject.transform.position = lPosition;
            objectToLoad = null;
            lastObject = lNewObject;
            lNewObject.name = overridenObjectName;
            GUIUtility.ExitGUI();
            Selection.activeObject = lNewObject;
        }
    }

    void LoadObject(string pPath)
    {
        objectToLoad = ContentAdder.getItemFromPrefab(pPath);
        overridenObjectName = objectToLoad.name;
        objectEditor = Editor.CreateEditor(objectToLoad);
    }

    void Undo()
    {
        DestroyImmediate(lastObject);
        lastObject = null;
    }
}