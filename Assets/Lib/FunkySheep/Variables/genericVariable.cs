using UnityEngine;

namespace FunkySheep.Variables
{
    public abstract class GenericVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
      public bool reset = false;

      public string DatabaseFieldName;

      public abstract string GetString();

      public abstract void setFromString(string value);
      public abstract void OnEnable();
    }
}