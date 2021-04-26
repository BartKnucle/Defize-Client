using UnityEngine;
using UnityEngine.Events;

namespace FunkySheep.Events
{
    [AddComponentMenu("FunkySheep/Events/Listener")]
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        void Awake() {
          if (!Event) {
            Event = ScriptableObject.CreateInstance<GameEvent>();
          }

          if (Response == null) {
            Response = new UnityEvent();
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

        public void OnEventRaised()
        {
          Response.Invoke();
        }
    }
}