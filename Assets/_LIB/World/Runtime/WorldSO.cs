using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.World.Tiles;

namespace FunkySheep.World
{
  [CreateAssetMenu(menuName="FunkySheep/World/Building")]
  public class WorldSO : ScriptableObject
  {
    public FunkySheep.Types.Vector3 initialMercatorPosition;
    public FunkySheep.Types.Vector3 currentMercatorPosition;
    public FunkySheep.Types.Vector2Int Position;
    public List<TileSO> tileSOs;

    private void OnEnable() {
      UpdatePosition();
    }

    public void UpdatePosition()
    {
      Vector3 position3D = initialMercatorPosition.Value - currentMercatorPosition.Value;
      Position.Value = new Vector2Int((int)position3D.x, (int)position3D.z);
    }
  }
}
