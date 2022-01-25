using UnityEngine;

namespace FunkySheep.Game.World
{
  [AddComponentMenu("FunkySheep/Game/World")]
  public class Manager : FunkySheep.Manager
  {
    public void Init()
    {
      (so as SO).Init(this);
    }

    public void CalculatePositions()
    {
      (so as SO).CalculatePositions(this);
    }
  }
}