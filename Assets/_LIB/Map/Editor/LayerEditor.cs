 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FunkySheep.Map
{
    [CustomEditor(typeof(Layer), editorForChildClasses: true)]
    public class LayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Layer e = target as Layer;
            if (GUILayout.Button("Clear Cache"))
                e.ClearCache();
        }
    }
}
#endif