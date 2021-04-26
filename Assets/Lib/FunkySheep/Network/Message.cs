using System;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Network {
    public class Message
    {
      public void Send(string data) {
        Manager.Instance.messages.Enqueue(data);
      }
    }
}

