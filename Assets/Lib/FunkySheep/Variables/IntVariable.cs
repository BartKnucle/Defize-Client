using System;
using UnityEngine;
using SimpleJSON;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/IntVariable")]
    public class IntVariable : GenericVariable
    {
        [SerializeField] private int _value;
        public int Value
        {
          get { return _value; }   // get method
          set { SetValue(value); }  // set method
        }

        virtual public bool SetValue(int value)
        {
          if (this._value != value) {
            this._value = value;
            return true;
          } else {
            return false;
          }
        }

        override public JSONNode toJSONNode() {
          return (JSONNode)this._value;
        }
        override public void fromJSONNode(JSONNode node) {
          this.SetValue(node);
        }

        override public string ToString() {
          return Value.ToString();
        }

        public override void OnEnable ()
        {
        }
    }
}