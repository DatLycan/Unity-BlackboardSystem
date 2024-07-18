using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DatLycan.Packages.BlackboardSystem {
    [CustomEditor(typeof(BlackboardObject))]
    public class BlackBoardObject : Editor {
        private ReorderableList entryList;

        // ReSharper disable once CognitiveComplexity
        private void OnEnable() {
            entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"), true, true, true, true) {
                drawHeaderCallback = rect => {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Key");
                    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.3f + 10, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Type");
                    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.6f + 5, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight), "Value");
                }
            };

            entryList.drawElementCallback = (rect, index, _, _) => {
                SerializedProperty element = entryList.serializedProperty.GetArrayElementAtIndex(index);
                
                rect.y += 2;
                SerializedProperty keyName = element.FindPropertyRelative("keyName");
                SerializedProperty valueType = element.FindPropertyRelative("valueType");
                SerializedProperty value = element.FindPropertyRelative("value");
                
                Rect keyNameRect = new(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                Rect valueTypeRect = new(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                Rect valueRect = new(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight);
                
                EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
                EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);

                switch ((AnyValue.ValueType) valueType.enumValueIndex) {
                    case AnyValue.ValueType.Bool:
                        SerializedProperty boolValue = value.FindPropertyRelative("boolValue");
                        EditorGUI.PropertyField(valueRect, boolValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Int:
                        SerializedProperty intValue = value.FindPropertyRelative("intValue");
                        EditorGUI.PropertyField(valueRect, intValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Float:
                        SerializedProperty floatValue = value.FindPropertyRelative("floatValue");
                        EditorGUI.PropertyField(valueRect, floatValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.String:
                        SerializedProperty stringValue = value.FindPropertyRelative("stringValue");
                        EditorGUI.PropertyField(valueRect, stringValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Vector2:
                        SerializedProperty vector2Value = value.FindPropertyRelative("vector2Value");
                        EditorGUI.PropertyField(valueRect, vector2Value, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Vector2Int:
                        SerializedProperty vector2IntValue = value.FindPropertyRelative("vector2IntValue");
                        EditorGUI.PropertyField(valueRect, vector2IntValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Vector3:
                        SerializedProperty vector3Value = value.FindPropertyRelative("vector3Value");
                        EditorGUI.PropertyField(valueRect, vector3Value, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Vector3Int:
                        SerializedProperty vector3IntValue = value.FindPropertyRelative("vector3IntValue");
                        EditorGUI.PropertyField(valueRect, vector3IntValue, GUIContent.none);
                        break;
                    case AnyValue.ValueType.Quaternion:
                        SerializedProperty quaternionValue = value.FindPropertyRelative("quaternionValue");
                        EditorGUI.PropertyField(valueRect, quaternionValue, GUIContent.none);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            entryList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
