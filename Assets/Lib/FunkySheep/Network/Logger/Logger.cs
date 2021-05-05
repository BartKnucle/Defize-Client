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
      msg.body["service"] = "/api/logger";
      msg.body["request"] = "create";
      msg.body["data"]["message"] = logString;
      msg.body["data"]["type"] = type.ToString();
      msg.body["data"]["stackTrace"] = stackTrace;

      msg.Send();
    }
  }
}

