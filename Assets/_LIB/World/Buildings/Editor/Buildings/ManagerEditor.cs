 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FunkySheep.World.Buildings
{
    [CustomEditor(typeof(Manager), editorForChildClasses: true)]
    public class ManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Manager e = target as Manager;

            if (GUILayout.Button("Clear cache"))
                e.ClearCache();
        }
    }
}
#endif