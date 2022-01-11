using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.World.Tiles;

namespace FunkySheep.World
{
  [CreateAssetMenu(menuName="FunkySheep/World/Building")]
  public class WorldSO : ScriptableObject
  {
    public FunkySheep.Types.Double latitude;
    public FunkySheep.Types.Double longitude;
    public FunkySheep.Types.Int zoom;
    // The earth world position
    public FunkySheep.Types.Vector2Int mapPosition;
    Vector2Int _lastMapPosition;
    //  The virtual position
    public FunkySheep.Types.Vector2Int gridPosition;
    Vector2Int _initialGridPosition;
    public FunkySheep.Events.GameEvent onPositionChanged;
    public int cacheSize = 0;
    public bool enabled = false;

    private void OnEnable() {
      enabled = false;
    }
    public void UpdatePosition()
    {
      if (!enabled)
      {
        _initialGridPosition = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, longitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, latitude.Value));
      }

      mapPosition.Value = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, longitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, latitude.Value));
      
      gridPosition.Value = mapPosition.Value - _initialGridPosition;
      // Invert Y position since reverted from mercator
      gridPosition.Value *= new Vector2Int(1, -1);

      if (_lastMapPosition != mapPosition.Value || !enabled)
      {
        onPositionChanged.Raise();
        _lastMapPosition = mapPosition.Value;
      }

      enabled = true;
    }
  }
}
