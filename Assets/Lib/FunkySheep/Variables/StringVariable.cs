using UnityEngine;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/StringVariable")]
    public class StringVariable : GenericVariable
    {
        private string value;
        public string Value
        {
          get { return value; }   // get method
          set { SetValue(value); }  // set method
        }

        virtual public bool SetValue(string value)
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
          SetValue(value);
        }
        public override void OnEnable ()
        {
          if (reset) {
            this.value = null;
          }
        }
    }
}