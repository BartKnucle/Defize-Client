using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

namespace FunkySheep.World
{
    [Serializable]
    public class Tile
    {
       public string id;
       public string mapId;
       public string url;
       string filePath;
       public JSONNode data;

       public Tile(string mapId, string url)
       {
           this.id = FunkySheep.Crypto.Hash(url);
           this.mapId = mapId;
           this.url = url;

           filePath = Application.persistentDataPath + "/funkysheep/world/buildings/" + id;
       }

        /// <summary>
        /// Load a tile either from the disk or from the internet
        /// </summary>
       public IEnumerator Load(Action Callback)
       {
           if (File.Exists(filePath))
           {
               LoadFromDisk();
               Callback();
               yield break;
           } else {
               yield return DownLoad(Callback);
           }
       }

        /// <summary>
        /// Load tile file from the disk
        /// </summary>
       public void LoadFromDisk()
       {
           data = JSON.Parse(File.ReadAllText(filePath));
       }


        /// <summary>
        /// Download the tile
        /// </summary>
        /// <param name="callback">The callback to be run after download complete</param>
        /// <returns></returns>
       public IEnumerator DownLoad(Action callback) {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                data = JSON.Parse(request.downloadHandler.text);
                File.WriteAllText(filePath, request.downloadHandler.text);
                callback();
            }
       }
    }
}