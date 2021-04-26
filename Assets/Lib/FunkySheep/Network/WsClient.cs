using System.Text;
using UnityEngine;

using FunkySheep.Variables;

namespace FunkySheep.Network {
  [System.Serializable]
  [CreateAssetMenu(menuName = "FunkySheep/Network/Client")]
  public class WsClient : ScriptableObject
  {
      public StringReference address;
      public IntReference port;
  }
}
