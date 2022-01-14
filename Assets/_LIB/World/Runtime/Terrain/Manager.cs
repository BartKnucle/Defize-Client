using System;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.World.OldTerrain
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
                Vector3[] tileBoundaries = ElevationLayer.tiles[i].Boundaries();
                float sizeX = tileBoundaries[1].x - tileBoundaries[0].x;
                float sizeZ = tileBoundaries[1].z - tileBoundaries[0].z;

                float tileSize = (float)FunkySheep.Map.Utils.TileSize(ElevationLayer.tiles[i].zoom);

                go.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
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
