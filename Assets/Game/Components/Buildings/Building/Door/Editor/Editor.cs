 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Game.Building.Door
{
    [CustomEditor(typeof(Manager), editorForChildClasses: true)]
    public class DoorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            Manager e = target as Manager;
            if (GUILayout.Button("Change seed"))
                e.SetSeed((int)Random.Range(-2147483648, 2147483648));
        }
    }
}
#endif