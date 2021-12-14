 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FunkySheep.Map
{
    [CustomEditor(typeof(Manager), editorForChildClasses: true)]
    public class ManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Manager e = target as Manager;
            if (GUILayout.Button("Next layer"))
                e.NextLayer();

            if (GUILayout.Button("Clear cache"))
                e.ClearCache();
        }
    }
}
#endif