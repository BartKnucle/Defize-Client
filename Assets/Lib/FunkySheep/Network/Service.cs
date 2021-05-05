using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using FunkySheep.Events;

namespace FunkySheep.Network {
    [CreateAssetMenu(menuName = "FunkySheep/Network/Service")]
    public class Service : ScriptableObject
    {
      public string api;
      public List<GenericVariable> fields;

      public GameEvent onReception;

      public void FindRecords(string Params = null) {
        Message msg = new Message();
        msg.body["service"] = this.api;
        msg.body["request"] = "find";
        msg.body["params"] = Params;
        msg.Send();
      }
      public void GetRecord(string Key = null, string Params = null) {
        Message msg = new Message();

        if (_setKey(msg, Key)) {
          msg.body["service"] = this.api;
          msg.body["request"] = "get";
          msg.body["params"] = Params;
          msg.Send();
        }
      }
      public void CreateRecords(string Params = null) {
        Message msg = new Message();
        msg.body["service"] = this.api;
        msg.body["request"] = "create";
        msg.body["params"] = Params;
        _fill(msg);
        msg.Send();
      }
      public void UpdateRecords(string Key = null , string Params = null) {
        Message msg = new Message();

        if (_setKey(msg, Key)) {
          msg.body["service"] = this.api;
          msg.body["request"] = "update";
          msg.body["params"] = Params;
          _fill(msg);
          msg.Send();
        }
      }
      public void PatchRecords(string Key = null, string Params = null) {
        Message msg = new Message();

        if (_setKey(msg, Key)) {
          msg.body["service"] = this.api;
          msg.body["request"] = "patch";
          msg.body["params"] = Params;
          _fill(msg);
          msg.Send();
        }
      }
      public void RemoveRecords(string Key, string Params = null) {
        Message msg = new Message();
        if (_setKey(msg, Key)) {
          msg.body["service"] = this.api;
          msg.body["request"] = "remove";
          msg.body["params"] = Params;
          msg.Send();
        }
      }

      private void _fill(Message msg) {
        fields.ForEach(field => {
          msg.body["data"][field.name] = field.toJSONNode();
        });
      }

      private bool _setKey(Message msg, string Key) {
        if (Key != null) {
          msg.body["key"] = Key;
          return true;
        } else if(_haveIdField() != null) {
          msg.body["key"] = _haveIdField();
          return true;
        } else {
          Debug.Log("You must provide a key or a _id field");
          return false;
        }
      }

      private string _haveIdField() {
        return fields.Find(field => field.name == "_id").ToString();
      }

    }
}

