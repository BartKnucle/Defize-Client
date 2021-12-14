using System;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Map
{
    [Serializable]
    [CreateAssetMenu(menuName = "FunkySheep/Map/Layer")]
    public class Layer : ScriptableObject
    {
        public FunkySheep.Types.String url;
        //List of the tiles [zoom, x, y]
        public List<Tile> tiles = new List<Tile>();

        /// <summary>
        /// Create a tile at eh given position or get existing one
        /// </summary>
        /// <param name="zoom">The zoom level</param>
        /// <param name="position">The tile position</param>
        /// <returns>The tile created</returns>
        public Tile CreateTile(int zoom, Vector2Int position)
        {
            string url = InterpolatedUrl(zoom, new Vector2Int(position.x, position.y));
            string id = Tile.GetId(url);
            Tile tile = tiles.Find(tile => tile.id == id);

            if (tile == null)
            {
                tile = new Tile(url);
                tile.position = position;
                tile.zoom = zoom;
                tiles.Add(tile);
            }

            return tile;
        }

        /// <summary>
        /// Interpolate the url inserting the coordinates and zoom values
        /// </summary>
        /// <param name="zoom">The zoom value</param>
        /// <param name="position">The coordinates</param>
        /// <returns>The interpolated Url</returns>
        public string InterpolatedUrl(int zoom, Vector2Int position)
        {
            string [] parameters = new string[3];
            string [] parametersNames = new string[3];

            parameters[0] = zoom.ToString();
            parametersNames[0] = "zoom";
            
            parameters[1] = position.x.ToString();
            parametersNames[1] = "position.x";
            
            parameters[2] = position.y.ToString();
            parametersNames[2] = "position.y";

            return url.Interpolate(parameters, parametersNames);
        }

        /// <summary>
        /// Clear all the layer tiles
        /// </summary>
        public void ClearCache()
        {
            foreach (Tile tile in tiles)
            {
                tile.ClearCache();
                
            }
            tiles.Clear();
        }
    }
}
