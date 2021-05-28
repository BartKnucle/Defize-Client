using UnityEngine;
using SimpleJSON;

namespace FunkySheep.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/StringVariable")]
    public class StringVariable : GenericVariable
    {
        [SerializeField] private string _value;
        public string Value
        {
          get { return _value; }   // get method
          set { SetValue(value); }  // set method
        }

        virtual public bool SetValue(string value)
        {
          if (this._value != value) {
            this._value = value;
            return true;
          } else {
            return false;
          }
        }

        override public string ToString() {
          return Value;
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
            this.SetValue(null);
          }
        }
    }
}