using System;
using UnityEngine;
using SimpleJSON;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/FloatVariable")]
    public class FloatVariable : GenericVariable
    {
        [SerializeField] private float _value;
        public float Value
        {
          get { return _value; }   // get method
          set { SetValue(value); }  // set method
        }

        virtual public bool SetValue(float value)
        {
          if (this._value != value) {
            this._value = value;
            return true;
          } else {
            return false;
          }
        }

        override public string ToString() {
          return Value.ToString();
        }

        override public JSONNode toJSONNode() {
          return (JSONNode)this._value;
        }
        override public void fromJSONNode(JSONNode node) {
          this.SetValue(node);
        }

        public override void OnEnable ()
        {
            if (reset) {
              _value = 0;
            }
        }
    }
}