using UnityEngine;
using UnityEngine.UI;
using FunkySheep.Variables;

namespace FunkySheep.UI
{
    [RequireComponent(typeof(UnityEngine.UI.InputField), typeof(FunkySheep.Events.GameEventListener))][AddComponentMenu("FunkySheep/UI/SetInputField")]
    public class InputFieldUpdater : MonoBehaviour {
        public StringVariable variable;

        void Start() {
          SetComponentText();
        }

        public void SetComponentText() {
            InputField inputField = gameObject.GetComponent<InputField>();
            inputField.SetTextWithoutNotify(variable.ToString());
        }

        public void SetVariable(string value) {
            variable.SetValue(value);
        }
    }    
}