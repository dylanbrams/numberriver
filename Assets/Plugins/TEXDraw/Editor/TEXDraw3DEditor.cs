using TexDrawLib;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TEXDraw3D))]
[CanEditMultipleObjects]
public class TEXDraw3DEditor : Editor
{
    private SerializedProperty _mText;
    private SerializedProperty _mSize;
    private SerializedProperty _mAlign;
    private SerializedProperty _mColor;
    private SerializedProperty _mPadding;
    private SerializedProperty _mScrollArea;
    private SerializedProperty _mMaterial;
    private SerializedProperty _mPixelsPerUnit;

    static bool foldExpand = false;

    // Use this for initialization
    private void OnEnable()
    {
        _mText = serializedObject.FindProperty("m_Text");
        _mSize = serializedObject.FindProperty("m_Size");
        _mColor = serializedObject.FindProperty("m_Color");
        _mAlign = serializedObject.FindProperty("m_Alignment");
        _mPadding = serializedObject.FindProperty("m_Padding");
        _mScrollArea = serializedObject.FindProperty("m_ScrollArea");
        _mMaterial = serializedObject.FindProperty("m_Material");
        _mPixelsPerUnit = serializedObject.FindProperty("m_PixelsPerUnit");
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
#if !TEXDRAW_TMP
            EditorGUILayout.PropertyField(_mPixelsPerUnit);
#endif
            EditorGUI.indentLevel--;
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            Redraw();
        }

    }

    public void Redraw()
    {
        foreach (TEXDraw3D i in (serializedObject.targetObjects))
        {
            i.Redraw();
            i.Repaint();
        }
    }

    [MenuItem("GameObject/3D Object/TEXDraw 3D", false, 3300)]
    public static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        TEXPreference.Initialize();
        // Create a custom game object
        GameObject go = new GameObject("TEXDraw 3D");
        go.AddComponent<TEXDraw3D>();
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
