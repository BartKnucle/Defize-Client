using UnityEngine;
using FunkySheep.OSM;

namespace FunkySheep.Procedural.Buildings
{
  [AddComponentMenu("FunkySheep/Procedural/Buildings")]
  public class Manager : FunkySheep.Manager
  {
    public void Build(Data data)
    {
      (so as SO).Build(this, data);
    }
  }  
}
