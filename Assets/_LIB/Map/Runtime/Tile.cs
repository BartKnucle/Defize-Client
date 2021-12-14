using System;
using System.IO;
using System.Collections;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

namespace FunkySheep.Map
{
    [Serializable]
    public class Tile
    {
        static string filePath = Application.persistentDataPath + "/funkysheep/map/tiles/";
        public string id;
        public Texture2D texture;
        public string url;
        public Vector2Int position;
        public int zoom;
        public UnityEngine.Tilemaps.Tile data;

        public Tile(string url) {
            this.url = url;
            this.id = GetId(url);

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
        /// Get the Id of an Url using MD5
        /// </summary>
        /// <param name="url"></param>
        /// <returns>The MD5 hash of the url</returns>
        public static string GetId(string url)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(url);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
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
        public Double[] GpsBoundaries()
        {
            Double tileSize = Utils.TileSize(zoom);
            double latitude = Utils.tileZ2lat(zoom, position.y);
            double longitude = Utils.tileX2long(zoom, position.x);
            Double latitudeSize = FunkySheep.GPS.Utils.yToLat(tileSize);
            Double longitudeSize = FunkySheep.GPS.Utils.xToLon(tileSize);

            Double[] boundaries = new Double[4];
            boundaries[0] = latitude - latitudeSize;
            boundaries[1] = longitude - longitudeSize;
            boundaries[2] = latitude + latitudeSize;
            boundaries[3] = longitude + longitudeSize;

            return boundaries;

        }
    }
}
