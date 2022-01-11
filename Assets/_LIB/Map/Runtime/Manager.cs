using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

namespace FunkySheep.Map
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(TilemapRenderer))]
    [RequireComponent(typeof(Grid))]
    [AddComponentMenu("FunkySheep/Map/Manager")]
    public class Manager : MonoBehaviour
    {
        // The square around a tile to download
        public FunkySheep.Types.Int cacheSize;
        public  FunkySheep.Types.Double latitude;
        public  FunkySheep.Types.Double longitude;
        public  FunkySheep.Types.Vector2Int initialPosition;
        public  FunkySheep.Types.Vector2Int currentPosition;
        public FunkySheep.Types.Int zoom;
        public Layer[] layers;
        Tilemap tilemap;

        private void Awake() {
            // The zero value in Z axis fix the gap between tiles
            GetComponent<Grid>().cellSize = new Vector3(256f, 256f, 0f);
            this.tilemap = GetComponent<Tilemap>();
        }

        private void Start() {
            // Set the position inside the tile
            tilemap.tileAnchor = new Vector3(
                -Utils.LongitudeToInsideX(zoom.Value, longitude.Value),
                -1 + Utils.LatitudeToInsideZ(zoom.Value, latitude.Value),
                0
            );

            // Set the scale depending on the zoom
            tilemap.transform.localScale = new Vector3(
                (float)Utils.TileSize(zoom.Value) / 256f,
                (float)Utils.TileSize(zoom.Value) / 256f,
            1f);

            this.initialPosition.Value = new Vector2Int(Utils.LongitudeToX(zoom.Value, longitude.Value), Utils.LatitudeToZ(zoom.Value, latitude.Value));
            Download(this.initialPosition.Value);
        }

        private void Update() {
          this.currentPosition.Value = new Vector2Int(Utils.LongitudeToX(zoom.Value, longitude.Value), Utils.LatitudeToZ(zoom.Value, latitude.Value));
        }

        /// <summary>
        /// Start a download of all the tiles, cached tiles for all the layers
        /// </summary>
        /// <param name="position"></param>
        public void Download(Vector2Int realWorldPosition2D)
        {
            Vector2Int gridPosition2D = realWorldPosition2D - initialPosition.Value;
            
            for (int i = 0; i < layers.Length; i++)
            {
                Vector3Int realWorldPosition = new Vector3Int(
                    realWorldPosition2D.x,
                    i,
                    realWorldPosition2D.y
                );

                Vector3Int gridPosition3D = new Vector3Int(
                    gridPosition2D.x,
                    gridPosition2D.y,
                    i
                );
             
                // For each cached tiles
                for (int x = -cacheSize.Value; x <= cacheSize.Value; x++)
                {
                    for (int y = -cacheSize.Value; y <= cacheSize.Value; y++)
                    {
                        DownloadTile(
                            layers[i],
                            realWorldPosition + new Vector3Int(x, 0 , y),
                            gridPosition3D + new Vector3Int(x, y, 0)
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Download a tile and insert it in the tilemap
        /// </summary>
        /// <param name="layer">The layer used for download</param>
        /// <param name="realWorldPosition">The real OSM calculated world position</param>
        /// <param name="gridPosition">The grid position</param>
        public void DownloadTile(Layer layer, Vector3Int realWorldPosition, Vector3Int gridPosition)
        {
            //Invert Y axis since Unity and OSM positions are inverted
            gridPosition *= new Vector3Int(1, -1, 1);
            if (!tilemap.HasTile(gridPosition))
            {
                Tile tile = layer.CreateTile(zoom.Value, new Vector2Int(realWorldPosition.x, realWorldPosition.z));
                StartCoroutine(tile.DownLoad(() => {
                    tilemap.SetTile(gridPosition, tile.data);
                    tilemap.RefreshTile(gridPosition);
                }));
            }
        }

        public void NextLayer()
        {
            Layer[] newLayer = new Layer[layers.Length];

            // Slide the new layers array
            for (int i = 0; i < layers.Length; i++)
            {
                newLayer[(i + 1) % layers.Length] = layers[i];
            }

            // Switch the values in the tilemap
            for (int i = 0; i < layers.Length; i++)
            {
                foreach (Tile tile in layers[i].tiles)
                {
                    tilemap.SwapTile(
                        tile.data,
                        newLayer[i].tiles[layers[i].tiles.IndexOf(tile)].data
                    );
                }
            }

            layers = newLayer;
        }

        /// <summary>
        /// Clear all the layers cache
        /// </summary>
        public void ClearCache()
        {
            foreach (Layer layer in layers)
            {
                layer.ClearCache();
            }
        }
    }
}
