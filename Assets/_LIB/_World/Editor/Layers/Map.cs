 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FunkySheep.World.Map
{
    [CustomEditor(typeof(LayerSO), editorForChildClasses: true)]
    public class EditorLayerSO : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LayerSO e = target as LayerSO;

            if (GUILayout.Button("Clear cache"))
                e.ClearCache();
        }
    }
}
#endif