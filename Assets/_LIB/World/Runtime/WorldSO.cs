using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World
{
  [CreateAssetMenu(menuName="FunkySheep/World/Building")]
  public class WorldSO : ScriptableObject
  {
    public FunkySheep.Types.Double currentlatitude;
    public FunkySheep.Types.Double currentlongitude;
    public Vector3 initialOffset;
    public Vector3 tileRealSize;
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

    /// <summary>
    /// Create the world
    /// </summary>
    /// <param name="world">The world manager that deleguate</param>
    public void Create(Manager world)
    {
      foreach (LayerSO layerSO in layersSO)
      {
        AddLayer(layerSO, world);
      }

      UpdatePosition(world);
    }

    /// <summary>
    /// Add a layer to the world
    /// </summary>
    /// <param name="layerSO">Layer Scriptable Object</param>
    /// <param name="world">The world where to create manager</param>
    /// <returns></returns>
    public GameObject AddLayer(LayerSO layerSO, Manager world)
    {
      GameObject go = new GameObject(layerSO.name);
      layerSO.CreateManager(go, world);
      go.transform.parent = world.transform;
      return go;
    }

    /// <summary>
    /// Update the player position on the grid
    /// </summary>
    public void UpdatePosition(Manager world)
    {
      if (!active)
      {
        _initialGridPosition = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, currentlongitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, currentlatitude.Value));

        // The initial offset inside a tile (from 0 to 1)
        initialOffset = new Vector3(
          Utils.LongitudeToInsideX(zoom.Value, currentlongitude.Value),
          0,
          Utils.LatitudeToInsideZ(zoom.Value, currentlatitude.Value)
        );

        // The tile size in meters
        tileRealSize = new Vector3(
          (float)Utils.TileSize(zoom.Value),
          (float)Utils.TileSize(zoom.Value),
          (float)Utils.TileSize(zoom.Value));
      }

      mapPosition.Value = new Vector2Int(FunkySheep.Map.Utils.LongitudeToX(zoom.Value, currentlongitude.Value), FunkySheep.Map.Utils.LatitudeToZ(zoom.Value, currentlatitude.Value));
      
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
        Tile tile = tiles.Find(tile => (tile.mapPosition == mapPosition && tile.layer == layer));
        if (tile == null)
        {
          tile = layer.layerSO.AddTile(world, layer);
          tiles.Add(tile);
        }
      }
    }
  }
}
