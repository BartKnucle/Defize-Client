using System;
using UnityEngine;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/BoolVariable")]
    public class BoolVariable : GenericVariable
    {
        private bool value;
        public bool Value
        {
          get { return value; }   // get method
          set { SetValue(value); }  // set method
        }

        virtual public bool SetValue(bool value)
        {
          if (this.value != value) {
            this.value = value;
            return true;
          } else {
            return false;
          }
        }

        override public string GetString() {
          return Value.ToString();
        }
        override public void setFromString(string value) {
          SetValue(Convert.ToBoolean(value));
        }

        public override void OnEnable ()
        {
        }
    }
}