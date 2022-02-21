using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.SimpleJSON;

namespace FunkySheep.OSM.Buildings
{
  public class Parse
  {
    byte[] data;
    FunkySheep.OSM.Events.OSMEventData onThreadEnd;

    public Parse(byte[] data, Queue<Data> datas)
    {
      this.data = data;
      new Thread(this.Run).Start(datas);
    }

    public void Run(object data)
    {
      try
      {
        Data parsedData = FunkySheep.OSM.Parser.Parse(this.data);
        (data as Queue<Data>).Enqueue(parsedData);
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
    }
  }


  [AddComponentMenu("FunkySheep/OSM/Buildings")]
  public class Manager : FunkySheep.Manager
  {
    Queue<Data> datas = new Queue<Data>();
    public Queue<byte[]> files = new Queue<byte[]>();

    private void Update() {
      if (files.Count != 0)
      {
        new Parse(files.Dequeue(), datas);
      }
      if (datas.Count != 0)
      {
        (so as SO).onOSMBuildingsDownloaded.Raise(datas.Dequeue());
      } 
    }
  }  
}

