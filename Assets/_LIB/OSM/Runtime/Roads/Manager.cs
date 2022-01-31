using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.OSM.Roads
{
  [AddComponentMenu("FunkySheep/OSM/Roads")]
  public class Manager : FunkySheep.Manager
  {
    public Queue<byte[]> files = new Queue<byte[]>();
    private void Update() {
      if (files.Count != 0)
      {
        StartCoroutine(FunkySheep.OSM.Parser.Parse(files.Dequeue(), (data) => {
          (so as SO).onOSMRoadsDownloaded.Raise(data);
        }));
      }
    }
  }  
}
