using UnityEngine;
using UnityEngine.Events;

namespace FunkySheep.Maps
{
    [AddComponentMenu("FunkySheep/Maps/Events/AddedTileListener")]
    public class AddedTileListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public AddedTileEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent<FunkySheep.Maps.Manager, Tile> Response;

        void Awake() {
          if (!Event) {
            Event = ScriptableObject.CreateInstance<AddedTileEvent>();
          }

          if (Response == null) {
            Response = new UnityEvent<FunkySheep.Maps.Manager, Tile>();
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

        public void OnEventRaised(FunkySheep.Maps.Manager manager, Tile tile)
        {
          Response.Invoke(manager, tile);
        }
    }
}