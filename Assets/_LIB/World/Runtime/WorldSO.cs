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
    public Vector2 initialOffset;
    public Vector2 currentOffset;
    public Vector3 initialDisplacement;
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
        _initialGridPosition = new Vector2Int(FunkySheep.World.Utils.LongitudeToX(zoom.Value, currentlongitude.Value), FunkySheep.World.Utils.LatitudeToZ(zoom.Value, currentlatitude.Value));

        // The initial offset inside a tile (from 0 to 1)
        currentOffset = initialOffset = new Vector2(
          -Utils.LongitudeToInsideX(zoom.Value, currentlongitude.Value),
          -1 + Utils.LatitudeToInsideZ(zoom.Value, currentlatitude.Value)
        );

        // The tile size in meters
        tileRealSize = new Vector3(
          (float)Utils.TileSize(zoom.Value),
          (float)Utils.TileSize(zoom.Value),
          (float)Utils.TileSize(zoom.Value));

        initialDisplacement = new Vector3(
          initialOffset.x * tileRealSize.x,
          0,
          initialOffset.y * tileRealSize.y
        );
      }

      // The initial offset inside a tile (from 0 to 1)
      currentOffset = new Vector2(
        -Utils.LongitudeToInsideX(zoom.Value, currentlongitude.Value),
        -1 + Utils.LatitudeToInsideZ(zoom.Value, currentlatitude.Value)
      );

      mapPosition.Value = new Vector2Int(FunkySheep.World.Utils.LongitudeToX(zoom.Value, currentlongitude.Value), FunkySheep.World.Utils.LatitudeToZ(zoom.Value, currentlatitude.Value));
      
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
        if (layer.layerSO.activated == true)
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

    public Vector2 RelativeInsidePosition(Vector2 position)
    {
      //position.y = 1 - position.y;
      position.x %= tileRealSize.x;
      position.y %= tileRealSize.y;

      position.x /= tileRealSize.x;
      position.y /= tileRealSize.y;

      return position;
    }

    public Vector3 RealWorldPosition(Tile tile)
    {
      Vector3 position = new Vector3(
        tile.gridPosition.x * tileRealSize.x,
        0,
        tile.gridPosition.y * tileRealSize.z
      ) + initialDisplacement;

      return position;
    }
  }
}
