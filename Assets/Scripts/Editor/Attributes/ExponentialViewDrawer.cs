using Common;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BigDigit))]
public class ExponentialViewDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);
        var mantissaRect = new Rect(position.x, position.y, position.width - 120, position.height);
        position.x = mantissaRect.position.x + mantissaRect.width;

        var tenRect = new Rect(position.x, position.y, 50, position.height);
        position.x = tenRect.position.x + tenRect.width - 10f;

        var exponentRect = new Rect(position.x, position.y, position.width - mantissaRect.width - tenRect.width - 10, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        property.FindPropertyRelative("count").floatValue = EditorGUI.FloatField(mantissaRect, property.FindPropertyRelative("count").floatValue);

        EditorGUI.LabelField(tenRect, "+E");
        property.FindPropertyRelative("e10").intValue = EditorGUI.IntField(exponentRect, property.FindPropertyRelative("e10").intValue);

        EditorGUI.EndProperty();
        property.serializedObject.ApplyModifiedProperties();
    }
}
