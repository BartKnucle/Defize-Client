using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.OSM.Events
{
    [CreateAssetMenu(menuName = "FunkySheep/OSM/Events/Data")]
    public class OSMEventData : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private List<OSMEventListenerData> eventListeners;

        void Awake() {
          if (eventListeners == null) {
            eventListeners = new List<OSMEventListenerData>();
          }
        }

        public void Raise(Data data)
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                if (eventListeners[i] != null) {
                    eventListeners[i].OnEventRaised(data);
                } else {
                    UnregisterListener(eventListeners[i]);
                }
        }

        public void RegisterListener(OSMEventListenerData listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(OSMEventListenerData listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}