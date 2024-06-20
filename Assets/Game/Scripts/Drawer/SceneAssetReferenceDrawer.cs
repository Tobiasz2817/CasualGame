using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneAssetReference))]
public class SceneAssetReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty assetPathProp = property.FindPropertyRelative("assetPath");
        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPathProp.stringValue);
        
        Rect assetFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        SceneAsset newSceneAsset = EditorGUI.ObjectField(assetFieldRect, label, sceneAsset, typeof(SceneAsset), false) as SceneAsset;

        if (newSceneAsset != sceneAsset) {
            string newPath = newSceneAsset != null ? AssetDatabase.GetAssetPath(newSceneAsset) : "";
            assetPathProp.stringValue = newPath;
        }

        EditorGUI.EndProperty();
    }
}