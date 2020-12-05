using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        private static readonly Dictionary<string, SaveableEntity> GlobalLookup = new Dictionary<string, SaveableEntity>();
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string,object> state = new Dictionary<string, object>();
            foreach (var saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string,object> stateDict = (Dictionary<string, object>)state;
            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;

            var serializedObject = new SerializedObject(this);
            var property = serializedObject.FindProperty("uniqueIdentifier");
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            GlobalLookup[property.stringValue] = this;
            
        }
        #endif
        private bool IsUnique(string candidate)
        {
            if (!GlobalLookup.ContainsKey(candidate)) return true;

            if (GlobalLookup[candidate] == this) return true;

            if (GlobalLookup[candidate] == null)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                GlobalLookup.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}