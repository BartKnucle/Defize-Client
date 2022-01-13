using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<LayerSO> layersSO;
    public List<Tile> tiles;
    bool active = false;

    private void OnEnable() {
      active = false;
      tiles = new List<Tile>();
    }

    public void Create(Manager world)
    {
      foreach (LayerSO layerSO in layersSO)
      {
        GameObject go = new GameObject(layerSO.name);
        layerSO.CreateManager(go, this);
        go.transform.parent = world.transform;
      }

      UpdatePosition(world);
    }

    /// <summary>
    /// Update the player position on the grid
    /// </summary>
    public void UpdatePosition(Manager world)
    {
      if (!active)
      {
        _initialGridPosition = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, longitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, latitude.Value));
      }

      mapPosition.Value = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, longitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, latitude.Value));
      
      gridPosition.Value = mapPosition.Value - _initialGridPosition;
      // Invert Y position since reverted from mercator
      gridPosition.Value *= new Vector2Int(1, -1);

      if (_lastMapPosition != mapPosition.Value || !active)
      {
        AddTiles(world, mapPosition.Value);
        onPositionChanged.Raise();
        _lastMapPosition = mapPosition.Value;
      }

      active = true;
    }

    /// <summary>
    /// Add a tile to the world for each layer
    /// </summary>
    /// <param name="mapPosition">The tile earth position</param>
    /// <returns></returns>
    public void AddTiles(Manager world, Vector2Int mapPosition)
    {
      foreach (Layer layer in world.transform.GetComponentsInChildren<Layer>())
      {
        Tile tile = tiles.Find(tile => (tile.mapPosition == mapPosition && tile.layerSO == layer.layerSO));
        if (tile == null)
        {
          tile = new Tile(this, layer.layerSO);
          layer.layerSO.AddTile(layer, tile);
          tiles.Add(tile);
        }
      }
    }
  }
}
