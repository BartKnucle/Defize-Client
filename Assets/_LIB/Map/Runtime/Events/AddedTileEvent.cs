using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Maps
{
    [CreateAssetMenu(menuName = "FunkySheep/Maps/Events/AddedTile")]
    public class AddedTileEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<AddedTileListener> eventListeners;

        void Awake() {
          if (eventListeners == null) {
            eventListeners = new List<AddedTileListener>();
          }
        }

        public void Raise(FunkySheep.Maps.Manager manager, Tile tile)
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                if (eventListeners[i] != null) {
                    eventListeners[i].OnEventRaised(manager, tile);
                } else {
                    UnregisterListener(eventListeners[i]);
                }
        }

        public void RegisterListener(AddedTileListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(AddedTileListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}