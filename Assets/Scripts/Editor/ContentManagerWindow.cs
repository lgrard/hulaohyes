using UnityEngine;
using UnityEditor;

class ContentManagerWindow : EditorWindow
{
    private float _buttonWidth;
    private float _buttonHeight;
    private float _labelWidth;
    private Vector2 _scrollView;
    private GUIStyle _indicationStyle;
    private GUIStyle _titleStyle;
    private GUIStyle _labelStyle;
    private GUIStyle _fieldStyle;
    private Object _objectToLoad;
    private GameObject _lastObject;
    private string _overridenObjectName;

    [MenuItem("HulaOhYes/Content Manager &HOME")] private static void ShowWindow() => EditorWindow.GetWindow(typeof(ContentManagerWindow)).titleContent.text = "Content Manager";

    void OnFocus()
    {
        // Remove delegate listener if it has previously been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;

        _indicationStyle = new GUIStyle();
        _indicationStyle.fontSize = 10;
        _indicationStyle.fontStyle = FontStyle.BoldAndItalic;
        _indicationStyle.alignment = TextAnchor.LowerCenter;

        _titleStyle = new GUIStyle();
        _titleStyle.fontSize = 15;
        _titleStyle.fontStyle = FontStyle.Bold;
        _titleStyle.alignment = TextAnchor.MiddleCenter;

        _labelStyle = new GUIStyle();
        _labelStyle.fontSize = 12;
        _labelStyle.fontStyle = FontStyle.Italic;
        _labelStyle.alignment = TextAnchor.MiddleRight;
        _labelStyle.padding.right = 10;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        _objectToLoad = null;
        _lastObject = null;
    }

    private void OnGUI()
    {
        _buttonHeight = position.height / 12f;
        _buttonWidth = position.width / 1.5f;
        _labelWidth = position.width / 4f;


        GUILayout.Space(15);
        GUILayout.Label("Content Manager", _titleStyle);
        GUILayout.Space(15);

        _scrollView = GUILayout.BeginScrollView(_scrollView);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Players", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("Player_0", GUILayout.Width(_buttonWidth/2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Player_0");
        if(GUILayout.Button("Player_1", GUILayout.Width(_buttonWidth/2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Player_1");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Managers", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("GameManager", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Managers/GameManager");
        if (GUILayout.Button("CameraManager", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Managers/CameraManager");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cameras", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("LookAtCamera", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Cameras/LookAtCamera");
        if (GUILayout.Button("AngledCamera", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Cameras/AngledCamera");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enemies", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("Walker", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Enemy/Enemy_Walker");
        if (GUILayout.Button("Turret", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Enemy/Enemy_Turret");
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Bricks", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("DoorGroup", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Bricks/DoorGroup");
        if (GUILayout.Button("UnitCubeSpawner", GUILayout.Width(_buttonWidth / 2), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/Bricks/UnitCubeSpawner");
        GUILayout.EndHorizontal();

        GUILayout.Space(20f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Fence", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("x0", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x0");
        if (GUILayout.Button("x1", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x1");
        if (GUILayout.Button("x2", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Fence_x2");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Outline", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("x0", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x0");
        if (GUILayout.Button("x1", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x1");
        if (GUILayout.Button("x2", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Outline_x2");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Plank", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("x1", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x1");
        if (GUILayout.Button("x2", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x2");
        if (GUILayout.Button("x4", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Plank_x4");
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Pole", _labelStyle, GUILayout.Width(_labelWidth));
        if (GUILayout.Button("x1", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x1");
        if (GUILayout.Button("x2", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x2");
        if (GUILayout.Button("x4", GUILayout.Width(_buttonWidth / 3), GUILayout.Height(_buttonHeight))) LoadObject("Prefabs/LD/SM_Pole_x4");
        GUILayout.EndHorizontal();


        GUILayout.EndScrollView();

        GUILayout.Space(15f);
        if (_objectToLoad != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Object name",_labelStyle, GUILayout.Width(position.width / 4));
            _overridenObjectName = GUILayout.TextField(_overridenObjectName, GUILayout.Width(position.width/1.5f));
            GUILayout.EndHorizontal();
            
            GUILayout.Space(5f);

            GUILayout.Label("Click anywhere to create a new " + _objectToLoad.name, _indicationStyle);
        }
        else GUILayout.Label("Select an object", _indicationStyle);

        GUILayout.Space(15f);
        if (_lastObject != null)
            if (GUILayout.Button("Undo", GUILayout.Height(_buttonHeight))) Undo();
        GUILayout.Space(5f);
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Ray lRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        Vector3 lPosition = Vector3.zero;
        if (Physics.Raycast(lRay, out hit) && !hit.collider.isTrigger) lPosition = hit.point;
        
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && _objectToLoad!=null)
        {
            GameObject lNewObject;

            if (_objectToLoad == ContentAdder.getItemFromPrefab("Prefabs/Bricks/DoorGroup"))
                lNewObject = ContentAdder.SpawnDoorGroup();

            else
                lNewObject = PrefabUtility.InstantiatePrefab(_objectToLoad) as GameObject;

            lNewObject.transform.position = lPosition;
            _objectToLoad = null;
            _lastObject = lNewObject;
            lNewObject.name = _overridenObjectName;
            GUIUtility.ExitGUI();
            Selection.activeObject = lNewObject;
        }
    }

    void LoadObject(string pPath)
    {
        _objectToLoad = ContentAdder.getItemFromPrefab(pPath);
        _overridenObjectName = _objectToLoad.name;
    }

    void Undo()
    {
        DestroyImmediate(_lastObject);
        _lastObject = null;
    }
}