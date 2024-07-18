using System;
using System.Collections.Generic;
using UnityEngine;

namespace DatLycan.Packages.BlackboardSystem {
    [CreateAssetMenu(fileName = "BlackboardObject", menuName = "Blackboard/Empty Blackboard Object")]
    public class BlackboardObject : ScriptableObject {
        public List<BlackBoardEntryData> entries = new();

        public void SetValuesOnBlackboard(Blackboard blackboard) {
            entries.ForEach(entry => entry.SetValueOnBlackboard(blackboard));
        }
    }

    [Serializable]
    public class BlackBoardEntryData : ISerializationCallbackReceiver {
        public string keyName;
        public AnyValue.ValueType valueType;
        public AnyValue value;

        public void SetValueOnBlackboard(Blackboard blackboard) {
            BlackboardKey key = blackboard.GetOrRegisterKey(keyName);
            setValueDispatchTable[value.type](blackboard, key, value);
        }
        
        // Dispatch table to set different types values
        private static Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> setValueDispatchTable = new() {
            { AnyValue.ValueType.Bool, (blackboard, key, anyValue) => blackboard.SetValue<bool>(key, anyValue) },
            { AnyValue.ValueType.Int, (blackboard, key, anyValue) => blackboard.SetValue<int>(key, anyValue) },
            { AnyValue.ValueType.Float, (blackboard, key, anyValue) => blackboard.SetValue<float>(key, anyValue) },
            { AnyValue.ValueType.String, (blackboard, key, anyValue) => blackboard.SetValue<string>(key, anyValue) },
            { AnyValue.ValueType.Vector2, (blackboard, key, anyValue) => blackboard.SetValue<Vector2>(key, anyValue) },
            { AnyValue.ValueType.Vector2Int, (blackboard, key, anyValue) => blackboard.SetValue<Vector2Int>(key, anyValue) },
            { AnyValue.ValueType.Vector3, (blackboard, key, anyValue) => blackboard.SetValue<Vector3>(key, anyValue) },
            { AnyValue.ValueType.Vector3Int, (blackboard, key, anyValue) => blackboard.SetValue<Vector3Int>(key, anyValue) },
            { AnyValue.ValueType.Quaternion, (blackboard, key, anyValue) => blackboard.SetValue<Quaternion>(key, anyValue) }
        };
        
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => value.type = valueType;
    }

    [Serializable]
    public struct AnyValue {
        public enum ValueType {
            Bool,
            Int,
            Float,
            String,
            Vector2,
            Vector2Int,
            Vector3,
            Vector3Int,
            Quaternion
        }
        public ValueType type;
        
        // Storage for different types of value
        public bool boolValue;
        public int intValue;
        public float floatValue;
        public string stringValue;
        public Vector2 vector2Value;
        public Vector2Int vector2IntValue;
        public Vector3 vector3Value;
        public Vector3Int vector3IntValue;
        public Quaternion quaternionValue;
        
        // Implicit conversion operators
        public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
        public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
        public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
        public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
        public static implicit operator Vector2(AnyValue value) => value.ConvertValue<Vector2>();
        public static implicit operator Vector2Int(AnyValue value) => value.ConvertValue<Vector2Int>();
        public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();
        public static implicit operator Vector3Int(AnyValue value) => value.ConvertValue<Vector3Int>();
        public static implicit operator Quaternion(AnyValue value) => value.ConvertValue<Quaternion>();

        T ConvertValue<T>() {
            return type switch {
                ValueType.Bool => AsBool<T>(boolValue),
                ValueType.Int => AsInt<T>(intValue),
                ValueType.Float => AsFloat<T>(floatValue),
                ValueType.String => AsString<T>(stringValue),
                ValueType.Vector2 => AsVector2<T>(vector2Value),
                ValueType.Vector2Int => AsVector2Int<T>(vector2IntValue),
                ValueType.Vector3 => AsVector3<T>(vector3Value),
                ValueType.Vector3Int => AsVector3Int<T>(vector3IntValue),
                ValueType.Quaternion => AsQuaternion<T>(quaternionValue),
                _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
            };
        }

        // Methods to convert primitive types to generic types
        private T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
        private T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
        private T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        private T AsString<T>(string value) => typeof(T) == typeof(string) && value is T correctType ? correctType : default;
        private T AsVector2<T>(Vector2 value) => typeof(T) == typeof(Vector2) && value is T correctType ? correctType : default;
        private T AsVector2Int<T>(Vector2Int value) => typeof(T) == typeof(Vector2Int) && value is T correctType ? correctType : default;
        private T AsVector3<T>(Vector3 value) => typeof(T) == typeof(Vector3) && value is T correctType ? correctType : default;
        private T AsVector3Int<T>(Vector3Int value) => typeof(T) == typeof(Vector3Int) && value is T correctType ? correctType : default;
        private T AsQuaternion<T>(Quaternion value) => typeof(T) == typeof(Quaternion) && value is T correctType ? correctType : default;
    }
}