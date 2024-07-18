using System;
using DatLycan.Packages.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace DatLycan.Packages.BlackboardSystem {
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey> {
        private readonly string name;
        private readonly int hashedKey;

        public BlackboardKey(string name) {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }
        
        public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;
        public override bool Equals(object @object) => @object is BlackboardKey other && Equals(other);
        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;
        
        public static bool operator == (BlackboardKey first, BlackboardKey second) => first.hashedKey == second.hashedKey;
        public static bool operator !=(BlackboardKey first, BlackboardKey second) => !(first == second);
    }

    [Serializable]
    public class BlackboardEntry<T> {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }
        

        public BlackboardEntry(BlackboardKey key, T value) {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object @object) => @object is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }
    
    [Serializable]
    public class Blackboard {
        private Dictionary<string, BlackboardKey> keyRegistry = new();
        private Dictionary<BlackboardKey, object> entries = new();

        public List<Action> PassedActions { get; } = new();
        
        public void AddAction(Action action) {
            Preconditions.CheckNotNull(action);
            PassedActions.Add(action);
        }

        public void ClearActions() => PassedActions.Clear();
        
        public void Log() {
            foreach (var entry in entries) {
                Type entryType = entry.Value.GetType();
                if (!entryType.IsGenericType || 
                    entryType.GetGenericTypeDefinition() != typeof(BlackboardEntry<>)) continue;
                
                PropertyInfo valueProperty = entryType.GetProperty("Value");
                if (valueProperty == null) continue;
                object value = valueProperty.GetValue(entry.Value);
                UnityEngine.Debug.Log($"Key: {entry.Key}, Value: {value}");
            }
        }

        public bool TryGetValue<T>(BlackboardKey key, out T value) {
            if (entries.TryGetValue(key, out object entry) && entry is BlackboardEntry<T> castedEntry) {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value) {
            entries[key] = new BlackboardEntry<T>(key, value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName) {
            Preconditions.CheckNotNull(keyName);

            if (!keyRegistry.TryGetValue(keyName, out BlackboardKey key)) {
                key = new BlackboardKey(keyName);
                keyRegistry[keyName] = key;
            }

            return key;
        }
    
        public bool ContainsKey(BlackboardKey key) => entries.ContainsKey(key);

        public void Remove(BlackboardKey key) => entries.Remove(key);
    }
}

