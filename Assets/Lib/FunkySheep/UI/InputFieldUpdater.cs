using UnityEngine;
using UnityEngine.UI;
using FunkySheep.Variables;

namespace FunkySheep.UI
{
    [RequireComponent(typeof(UnityEngine.UI.InputField), typeof(FunkySheep.Events.GameEventListener))][AddComponentMenu("FunkySheep/UI/SetInputField")]
    public class InputFieldUpdater : MonoBehaviour {
        public GenericVariable variable;

        void Start() {
          SetComponentText();
        }

        public void SetComponentText() {
            InputField inputField = gameObject.GetComponent<InputField>();
            inputField.SetTextWithoutNotify(variable.GetString());
        }

        public void SetVariable(string value) {
            variable.setFromString(value);
        }
    }    
}