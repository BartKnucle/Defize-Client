using UnityEngine;
using UnityEngine.Events;

namespace FunkySheep.OSM.Events
{
    [AddComponentMenu("FunkySheep/OSM/Events/Listener Data")]
    public class OSMEventListenerData : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public OSMEventData Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<Data> Response;

        void Awake() {
          if (!Event) {
            Event = ScriptableObject.CreateInstance<OSMEventData>();
          }

          if (Response == null) {
            Response = new UnityEvent<Data>();
          }
        }

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Data data)
        {
          Response.Invoke(data);
        }
    }
}