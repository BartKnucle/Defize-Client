using UnityEngine;
using System;
using SimpleJSON;

namespace FunkySheep.Variables
{
    public abstract class GenericVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
      public bool reset = false;
      public abstract JSONNode toJSONNode();
      public abstract void fromJSONNode(JSONNode node);
      public abstract void OnEnable();
    }
}