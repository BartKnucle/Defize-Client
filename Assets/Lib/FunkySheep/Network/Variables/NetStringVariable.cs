using UnityEngine;
using SimpleJSON;
using FunkySheep.Variables;

namespace FunkySheep.Network.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/Network/NetStringVariable")]
    public class NetStringVariable : StringVariable {

      public bool autoSend = false;
      public Service service;

      override public bool SetValue(string value)
      {
          bool updated = base.SetValue(value);
          if (autoSend && updated) {
            Send();
            return true;
          } else {
            return false;
          }
      }

      public void Send() {
        Message msg = new Message();
        JSONNode dataJSON = JSON.Parse("{}");
        dataJSON["service"] = service.api;
        if (service._id) {
          dataJSON["_id"] = service._id.Value;
        }
        dataJSON[this.DatabaseFieldName] = this.Value;
        string msgString = dataJSON.ToString();
        msg.Send(msgString);
      }
    }
}