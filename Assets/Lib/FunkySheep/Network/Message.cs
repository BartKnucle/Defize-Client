using SimpleJSON;
using FunkySheep;

namespace FunkySheep.Network {
    public class Message
    {
      public JSONNode body = JSON.Parse("{}");
      public Message () {
        body["sentAt"] = Time.Now();
      }

      public void Send() {
        Manager.Instance.messages.Enqueue(body.ToString());
      }
    }
}

