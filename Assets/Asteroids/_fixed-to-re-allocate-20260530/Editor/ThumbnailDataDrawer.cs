using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ThumbnailData))]
public class ThumbnailDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //var nameProp = property.FindPropertyRelative("displayName");
        //string labelText = string.IsNullOrEmpty(nameProp.stringValue)
        //    ? label.text
        //    : nameProp.stringValue;

        var objProp = property.FindPropertyRelative("thumbnailObject");
        string labelText = objProp.objectReferenceValue != null
            ? objProp.objectReferenceValue.name
            : label.text;

        EditorGUI.PropertyField(position, property, new GUIContent(labelText), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
