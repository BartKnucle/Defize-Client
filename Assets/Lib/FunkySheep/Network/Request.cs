using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using FunkySheep.Variables;
using FunkySheep.Events;

namespace FunkySheep.Network {
    [CreateAssetMenu(menuName = "FunkySheep/Network/Request")]
    public class Request : ScriptableObject
    {
      public string ServerRequestName;
      public Service service;
      public List<FloatVariable> floatVariables = new List<FloatVariable>();
      public List<IntVariable> intVariables = new List<IntVariable>();
      public List<StringVariable> stringVariables = new List<StringVariable>();
      public List<StringList> stringListVariables = new List<StringList>();
      public List<BoolVariable> boolVariables = new List<BoolVariable>();

      public GameEvent onReception;

      public void execute() {
        Message msg = new Message();
        JSONNode dataJSON = JSON.Parse("{}");
        dataJSON["service"] = service.api;
        dataJSON["request"] = this.ServerRequestName;

        foreach (var item in floatVariables)
        {
          dataJSON[item.DatabaseFieldName] = item.Value;
        }

        foreach (var item in intVariables)
        {
          dataJSON[item.DatabaseFieldName] = item.Value;
        }

        foreach (var item in stringVariables)
        {
          dataJSON[item.DatabaseFieldName] = item.Value;
        }

        foreach (var list in stringListVariables)
        {
          for (int i = 0; i < list.Value.Count; i++)
          {
              dataJSON[list.DatabaseFieldName][i] = list.Value[i];
          }
        }

        foreach (var item in stringVariables)
        {
          dataJSON[item.DatabaseFieldName] = item.Value;
        }

        foreach (var item in boolVariables)
        {
          dataJSON[item.DatabaseFieldName] = item.Value;
        }

        string msgString = dataJSON.ToString();
        msg.Send(msgString);
      }
    }
}

