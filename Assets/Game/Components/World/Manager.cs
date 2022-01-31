using UnityEngine;

namespace FunkySheep.Game.World
{
  [AddComponentMenu("FunkySheep/Game/World")]
  public class Manager : FunkySheep.Manager
  {
    public void Init()
    {
      (so as SO).Init(this);
      (so as SO).AddTile(this);
    }

    public void CalculatePositions()
    {
      (so as SO).CalculatePositions(this);
      (so as SO).AddTile(this);
    }
  }
}