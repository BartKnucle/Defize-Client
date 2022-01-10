using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Status : MonoBehaviour
    {
        UIDocument document;
        [Range(1, 10)]
        public float delay = 1;
        public float _lastUpdate = 0;

        public Queue _status = new Queue();

        private void Awake() {
            document = GetComponent<UIDocument>();
        }


        public void Add(string status)
        {
            _status.Enqueue(status);
        }

        private void Update() {
            if (_lastUpdate >= delay && _status.Count != 0)
            {
                if (document.rootVisualElement.childCount > 5) {
                    document.rootVisualElement.RemoveAt(4);
                }

                Label lblStatus = new Label(_status.Dequeue().ToString());
                document.rootVisualElement.Insert(0, lblStatus);

                _lastUpdate = 0;
            } else if (_lastUpdate >= delay && document.rootVisualElement.childCount > 0)
            {
                document.rootVisualElement.RemoveAt(document.rootVisualElement.childCount - 1);
                _lastUpdate = 0;
            }

            _lastUpdate += Time.deltaTime;
        }
    }    
}
