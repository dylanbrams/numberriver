#if UNITY_EDITOR

using TexDrawLib;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(TEXDraw))]
[CanEditMultipleObjects]
public class TEXDrawEditor : Editor
{
    private SerializedProperty _mText;
    private SerializedProperty _mSize;
    private SerializedProperty _mAlign;
    private SerializedProperty _mColor;
    private SerializedProperty _mPadding;
    private SerializedProperty _mScrollArea;
    private SerializedProperty _mMaterial;

    private SerializedProperty _mRaycastTarget;
    static bool foldExpand = false;

    // Use this for initialization
    private void OnEnable()
    {
        _mText = serializedObject.FindProperty("m_Text");
        _mSize = serializedObject.FindProperty("m_Size");
        _mColor = serializedObject.FindProperty("m_Color");
        _mPadding = serializedObject.FindProperty("m_Padding");
        _mScrollArea = serializedObject.FindProperty("m_ScrollArea");
        _mMaterial = serializedObject.FindProperty("m_Material");
        _mAlign = serializedObject.FindProperty("m_Alignment");
        _mRaycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        Undo.undoRedoPerformed += Redraw;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed -= Redraw;
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        TEXBoxHighlighting.DrawText(_mText);

        EditorGUILayout.PropertyField(_mSize);
        foldExpand = EditorGUILayout.Foldout(foldExpand, "More Properties");
        if (foldExpand)
        {
            EditorGUI.indentLevel++;

            TEXSharedEditor.DoTextAligmentControl(EditorGUILayout.GetControlRect(), _mAlign);
            EditorGUILayout.PropertyField(_mScrollArea);
            EditorGUILayout.PropertyField(_mPadding, true);
            EditorGUILayout.PropertyField(_mColor);
            TEXSharedEditor.DoMaterialGUI(_mMaterial, (ITEXDraw)target);
            EditorGUILayout.PropertyField(_mRaycastTarget);
            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck())
            Redraw();

        serializedObject.ApplyModifiedProperties();
    }

    public void Redraw()
    {
        foreach (TEXDraw i in (serializedObject.targetObjects))
        {
            i.SetTextDirty();
            //i.SetVerticesDirty();
            i.SetLayoutDirty();
        }
    }

    #region Adding Stuff...

    [MenuItem("GameObject/UI/TEXDraw", false, 3300)]
    private static void CreateTEXDraw(MenuCommand menuCommand)
    {
        TEXPreference.Initialize();
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(TEXPreference.main.MainFolderPath + "/Template-TEXDraw.prefab");
        GameObject go;
        if (!prefab)
        {
            go = new GameObject("TEXDraw");
            go.AddComponent<TEXDraw>();
        }
        else
        {
            go = Instantiate(prefab);
            go.name = "TEXDraw";
        }
        PlaceUIElementRoot(go, menuCommand);
    }

    static public GameObject GetOrCreateCanvasGameObject()
    {
        GameObject selectedGo = Selection.activeGameObject;

        // Try to find a gameobject that is the selected GO or one if its parents.
        Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in selection or its parents? Then use just any canvas..
        canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas != null && canvas.gameObject.activeInHierarchy)
            return canvas.gameObject;

        // No canvas in the scene at all? Then create a new one.
        return CreateNewUI();
    }

    private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
    {
        GameObject parent = menuCommand.context as GameObject;
        if (parent == null || parent.GetComponentInParent<Canvas>() == null)
        {
            parent = GetOrCreateCanvasGameObject();
        }

        string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
        element.name = uniqueName;
        Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
        Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
        GameObjectUtility.SetParentAndAlign(element, parent);
        if (parent != menuCommand.context) // not a context click, so center in sceneview
            SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

        Selection.activeGameObject = element;
    }

    private const string kUILayerName = "UI";

    static public GameObject CreateNewUI()
    {
        // Root for the UI
        var root = new GameObject("Canvas");
        root.layer = LayerMask.NameToLayer(kUILayerName);
        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        root.AddComponent<CanvasScaler>();
        root.AddComponent<GraphicRaycaster>();
        Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

        // if there is no event system add one...
        CreateEventSystem(false, null);
        return root;
    }

    private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
    {
        // Find the best scene view
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null && SceneView.sceneViews.Count > 0)
            sceneView = SceneView.sceneViews[0] as SceneView;

        // Couldn't find a SceneView. Don't set position.
        if (sceneView == null || sceneView.camera == null)
            return;

        // Create world space Plane from canvas position.
        Vector2 localPlanePosition;
        Camera camera = sceneView.camera;
        Vector3 position = Vector3.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
        {
            // Adjust for canvas pivot
            localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
            localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

            localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
            localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

            // Adjust for anchoring
            position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
            position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

            Vector3 minLocalPosition;
            minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
            minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

            Vector3 maxLocalPosition;
            maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
            maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

            position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
            position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
        }

        itemTransform.anchoredPosition = position;
        itemTransform.sizeDelta = new Vector2(200, 100);
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.localScale = Vector3.one;
    }

    private static void CreateEventSystem(bool select, GameObject parent)
    {
        var esys = FindObjectOfType<EventSystem>();
        if (esys == null)
        {
            var eventSystem = new GameObject("EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystem, parent);
            esys = eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
        }

        if (select && esys != null)
        {
            Selection.activeGameObject = esys.gameObject;
        }
    }

    #endregion
}

#endif
