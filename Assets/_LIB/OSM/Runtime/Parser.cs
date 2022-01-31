using System;
using System.Collections;
using FunkySheep.JSON;
using UnityEngine;

namespace FunkySheep.OSM
{
  public static class Parser 
  {
    public static IEnumerator Parse(JSONNode jsonData, Action<Data> callBack)
    {
      JSONArray elements = jsonData["elements"].AsArray;
      Data data = new Data();

      for (int i = 0; i < elements.Count; i++)
      {
        data.AddElement(elements[i]);
        yield return null;
      }

      callBack(data);
      yield break;
    }

    public static IEnumerator Parse(string textData, Action<Data> callBack)
    {
      JSONNode jsonData = JSONNode.Parse(textData);
      return Parse(jsonData, callBack);
    }

    public static IEnumerator Parse(byte[] rawData, Action<Data> callBack)
    {
      string textData = System.Text.Encoding.UTF8.GetString(rawData);
      return Parse(textData, callBack);
    }
  }  
}
