using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FunkySheep.Game.World
{
  [CreateAssetMenu(menuName = "FunkySheep/Game/World")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Types.Int zoom;

    public FunkySheep.Procedural.Earth.SO earthSO;
    public FunkySheep.Maps.SO osmSO;
    public FunkySheep.OSM.Buildings.SO buildingsSO;
    public FunkySheep.Types.Double latitude;
    public FunkySheep.Types.Double longitude;
    public FunkySheep.Types.Vector2 tileSize;
    
    public FunkySheep.Types.Vector2 initialOffset;
    public FunkySheep.Types.Vector2 initialDisplacement;
    public Vector2Int currentMapPosition;
    public Vector2Int? lastMapPosition;
    public List<Vector2> tiles;

    private void OnEnable() {
      tiles = new List<Vector2>();
    }

    public override void Create (FunkySheep.Manager manager)
    {
      base.Create(manager);
    }

    /// <summary>
    /// Set the defaults values
    /// </summary>
    /// <param name="manager"></param>
    public void Init(FunkySheep.Manager manager)
    {
      tileSize.Value = new Vector2
      (
        (float)FunkySheep.Maps.Utils.TileSize(zoom.Value),
        (float)FunkySheep.Maps.Utils.TileSize(zoom.Value)
      );

      initialOffset.Value = new Vector2(
        -FunkySheep.Maps.Utils.LongitudeToInsideX(zoom.Value, longitude.Value),
        -1 + FunkySheep.Maps.Utils.LatitudeToInsideZ(zoom.Value, latitude.Value)
      );

      initialDisplacement.Value = new Vector2(
        initialOffset.Value.x * tileSize.Value.x,
        initialOffset.Value.y * tileSize.Value.y
      );

      earthSO.terrainSize = new Vector3(tileSize.Value.x, 9000, tileSize.Value.y);
      (earthSO.Get(manager, earthSO) as FunkySheep.Procedural.Earth.Manager).root.transform.localPosition = new Vector3(
        initialOffset.Value.x * tileSize.Value.x,
        0,
        initialOffset.Value.y * tileSize.Value.y
      );

      FunkySheep.Maps.Manager height = (earthSO.heightSO.Get(manager, earthSO.heightSO) as FunkySheep.Maps.Manager);      
      height.root.transform.Rotate(new Vector3(90, 0, 0));
      height.root.GetComponent<Grid>().cellSize = new Vector3(256f, 256f, 0f);
      Tilemap heightTilemap = height.root.GetComponent<Tilemap>();
      heightTilemap.tileAnchor = new Vector3(
          initialOffset.Value.x,
          initialOffset.Value.y,
          0
      );

      heightTilemap.transform.localScale = new Vector3(
          tileSize.Value.x / 256f,
          tileSize.Value.y / 256f,
      1f);

      FunkySheep.Maps.Manager normal = (earthSO.normalSO.Get(manager, earthSO.normalSO) as FunkySheep.Maps.Manager);
      normal.root.transform.Rotate(new Vector3(90, 0, 0));
      normal.root.GetComponent<Grid>().cellSize = new Vector3(256f, 256f, 0f);
      Tilemap normalTilemap = normal.root.GetComponent<Tilemap>();
      normalTilemap.tileAnchor = new Vector3(
          initialOffset.Value.x,
          initialOffset.Value.y,
          0
      );

      normalTilemap.transform.localScale = new Vector3(
          tileSize.Value.x / 256f,
          tileSize.Value.y / 256f,
      1f);

      FunkySheep.Maps.Manager osm = (osmSO.Get(manager, osmSO) as FunkySheep.Maps.Manager);
      osm.root.transform.Rotate(new Vector3(90, 0, 0));
      osm.root.GetComponent<Grid>().cellSize = new Vector3(256f, 256f, 0f);
      Tilemap osmTilemap = osm.root.GetComponent<Tilemap>();
      osmTilemap.tileAnchor = new Vector3(
          initialOffset.Value.x,
          initialOffset.Value.y,
          0
      );

      osmTilemap.transform.localScale = new Vector3(
          tileSize.Value.x / 256f,
          tileSize.Value.y / 256f,
      1f);

      CalculatePositions(manager);
      AddTile(manager);
    }

    public void CalculatePositions(FunkySheep.Manager manager)
    {
      currentMapPosition = new Vector2Int(
        FunkySheep.Maps.Utils.LongitudeToX(zoom.Value, longitude.Value),
        FunkySheep.Maps.Utils.LatitudeToZ(zoom.Value, latitude.Value)
      );

      if (!lastMapPosition.HasValue)
      {
        lastMapPosition = currentMapPosition;
      }

      if (lastMapPosition != currentMapPosition)
      {
        AddTile(manager);
        lastMapPosition = currentMapPosition;
      }
    }

    public void AddTile(FunkySheep.Manager manager)
    {
      if (!tiles.Contains(currentMapPosition))
      {
        earthSO.AddTile(Get(manager, earthSO) as FunkySheep.Procedural.Earth.Manager, currentMapPosition);
        osmSO.AddTile(Get(manager, osmSO) as FunkySheep.Maps.Manager, currentMapPosition);
        double[] boundaries = FunkySheep.Maps.Utils.CaclulateGpsBoundaries(zoom.Value, latitude.Value, longitude.Value);
        buildingsSO.Download(Get(manager, buildingsSO) as FunkySheep.OSM.Buildings.Manager, boundaries);
        tiles.Add(currentMapPosition);
      }
    }
  }
}
