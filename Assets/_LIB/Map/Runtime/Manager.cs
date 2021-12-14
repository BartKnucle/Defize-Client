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
        public FunkySheep.Types.Int zoom;
        //public List<Layer> layers;
        public Layer[] layers;
        public Tilemap tilemap;

        private void Awake() {
            this.tilemap = GetComponent<Tilemap>();
        }

        private void Start() {
            this.initialPosition.Value = new Vector2Int(Utils.LongitudeToX(zoom.Value, longitude.Value), Utils.LatitudeToZ(zoom.Value, latitude.Value));
            Download(this.initialPosition.Value);
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
                    i,
                    gridPosition2D.y
                );

                // Download the main tile
                DownloadTile(layers[i], realWorldPosition, gridPosition3D);
               
                // For each cached tiles
                for (int x = 1; x < cacheSize.Value + 1; x++)
                {
                    for (int y = 1; y < cacheSize.Value + 1; y++)
                    {
                        DownloadTile(
                            layers[i],
                            realWorldPosition + new Vector3Int(x, 0 , y),
                            gridPosition3D + new Vector3Int(x, 0, y)
                        );
                    }
                }
            }
        }

        public void DownloadTile(Layer layer, Vector3Int realWorldPosition, Vector3Int gridPosition)
        {
            if (!tilemap.HasTile(realWorldPosition))
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
