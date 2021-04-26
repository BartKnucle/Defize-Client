using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/Variables Lists/StringList")]
    public class StringList : ScriptableObject
    {
        public string DatabaseFieldName;
        public string DatabaseSubObjectField;
        public List<string> Value;

        public void SetValue(List<string> value)
        {
            Value = value;
        }
    }
}