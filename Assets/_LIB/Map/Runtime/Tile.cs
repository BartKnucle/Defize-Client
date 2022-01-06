using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FunkySheep.Map
{
    [Serializable]
    public class Tile
    {
        static string filePath = Application.persistentDataPath + "/map/tiles/";
        public string id;
        public Texture2D texture;
        public string url;
        public Vector2Int position;
        public int zoom;
        public UnityEngine.Tilemaps.Tile data;

        public Tile(string url) {
            this.url = url;
            this.id = FunkySheep.Crypto.Hash(url);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        /// <summary>
        /// Download the tile
        /// </summary>
        public IEnumerator DownLoad(Action callback) {
            if (data != null)
            {
                yield break;
            }

            if (File.Exists(filePath + id))
            {
                byte[] fileData;
                fileData = File.ReadAllBytes(filePath + id);
                texture = new Texture2D(256, 256);
                texture.LoadImage(fileData);
                SetTile(texture);
                callback();
                yield break;
            }

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(request.error);
            }                
            else
            {
                texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                SetTile(texture);
                File.WriteAllBytes(filePath + id, request.downloadHandler.data);
                DownLoad(callback);
                callback();
            }
        }

        /// <summary>
        /// Set the tile object
        /// </summary>
        /// <param name="texture">The texture to set on the tile sprite</param>
        public void SetTile(Texture2D texture)
        {
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            data = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
            data.sprite = Sprite.Create((Texture2D) texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero, 1);
        }

        /// <summary>
        /// Delete the cached file
        /// </summary>
        public void ClearCache()
        {
            if (File.Exists(filePath + id))
            {
                File.Delete(filePath + id);
            }
        }

        /// <summary>
        /// Calculate the GPS boundaries of the tile
        /// </summary>
        /// <returns>A Double[4] containing [StartLatitude, StartLongitude, EndLatitude, EndLongitude]</returns>
        public Vector3[] Boundaries()
        {
            Double[] gpsBoundaries = GpsBoundaries();
            Vector3[] boundaries = new Vector3[2];

            boundaries[0] = FunkySheep.GPS.Utils.toCartesianVector(gpsBoundaries[0], gpsBoundaries[1]);
            boundaries[1] = FunkySheep.GPS.Utils.toCartesianVector(gpsBoundaries[2], gpsBoundaries[3]);

            return boundaries;

        }

        /// <summary>
        /// Calculate the GPS boundaries of the tile
        /// </summary>
        /// <returns>A Double[4] containing [StartLatitude, StartLongitude, EndLatitude, EndLongitude]</returns>
        public Double[] GpsBoundaries()
        {
            double latitude = Utils.tileZ2lat(zoom, position.y);
            double longitude = Utils.tileX2long(zoom, position.x);
            double nextLatitude = Utils.tileZ2lat(zoom, position.y + 1);
            double nextLongitude = Utils.tileX2long(zoom, position.x + 1);

            Double[] boundaries = new Double[4];
            
            boundaries[0] = nextLatitude;
            boundaries[1] = longitude;
            boundaries[2] = latitude;
            boundaries[3] = nextLongitude;

            return boundaries;

        }
    }
}
