using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using SimpleJSON;

namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Logger")][RequireComponent(typeof(Manager))]
  public class Logger : GenericSingletonClass<Logger>
  {
    // Start is called before the first frame update
    new void Awake() {
      base.Awake();
      Application.logMessageReceivedThreaded += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
      Message msg = new Message();
      JSONNode dataJSON = JSON.Parse("{}");
      dataJSON["service"] = "/api/logger";
      dataJSON["request"] = "log";
      dataJSON["message"] = logString;
      dataJSON["type"] = type.ToString();
      dataJSON["stackTrace"] = stackTrace;

      string msgString = dataJSON.ToString();
      msg.Send(msgString);
    }
  }
}

