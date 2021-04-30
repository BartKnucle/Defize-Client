using UnityEngine;
using SimpleJSON;
using FunkySheep;
using FunkySheep.Variables;

namespace FunkySheep.Network.Variables
{
    [CreateAssetMenu(menuName = "FunkySheep/Network/NetFloatVariable")]
    public class NetFloatVariable : FloatVariable {

      public Service service;

      override public bool SetValue(float value)
      {
          return base.SetValue(value);
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