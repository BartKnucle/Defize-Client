using UnityEngine;
using UnityEngine.UI;
using FunkySheep.Variables;

namespace FunkySheep.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Text), typeof(FunkySheep.Events.GameEventListener))][AddComponentMenu("FunkySheep/UI/SetText")]
    public class TextUpdater : MonoBehaviour {
        public GenericVariable variable;

        public void SetText() {
            gameObject.GetComponent<Text>().text = variable.GetString();
        }
    }    
}