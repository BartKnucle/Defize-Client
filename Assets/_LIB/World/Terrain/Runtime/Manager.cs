using System;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.Terrain
{
    [AddComponentMenu("FunkySheep/World/Terrain/Manager")]
    public class Manager : MonoBehaviour
    {
        public GameObject tilePrefab;
        public FunkySheep.Map.Layer ElevationLayer;
        public FunkySheep.Map.Layer NormalLayer;

        public void Create()
        {
            for (int i = 0; i < ElevationLayer.tiles.Count; i++)
            {
                GameObject go = Instantiate(tilePrefab);
                go.transform.parent = this.transform;
                go.name = ElevationLayer.tiles[i].id;
                Tile tile = go.GetComponent<Tile>();
                tile.ElevationTexture = ElevationLayer.tiles[i].texture;
                tile.NormalTexture = NormalLayer.tiles[i].texture;
                
                tile.CreateMesh();
            }
        }
    }    
}
