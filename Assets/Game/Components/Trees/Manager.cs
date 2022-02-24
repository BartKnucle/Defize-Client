using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Tilemaps;

namespace Game.World.Trees
{
    public class Manager : MonoBehaviour
    {
        public List<FunkySheep.Procedural.Earth.Tile> earthTiles = new List<FunkySheep.Procedural.Earth.Tile>();
        public FunkySheep.Types.Vector2 initialDisplacement;
        List<Vector3> positions = new List<Vector3>();
        //public FunkySheep.Types.Vector2 tileSize;
        public int drawDistance = 100;
        public FunkySheep.Types.Vector3 drawPosition;
        public GameObject tree;

        private void Start() {
        }

        public void AddedTreeTile(FunkySheep.Maps.Manager manager, FunkySheep.Maps.Tile tile)
        {
            Color32[] pixels = tile.data.sprite.texture.GetPixels32();

            Vector3 cellScale = manager.root.transform.localScale;

            Thread thread = new Thread(() => this.ProcessImage(
                tile.tilemapPosition,
                cellScale,
                pixels)
            );

            thread.Start();

        }

        public void ProcessImage(Vector3Int mapPosition, Vector3 tileScale, Color32[] pixels)
        {
            Debug.Log("run");
            try
            {
                int lastX = -8;
                int lastY = -8;
                for (int i = 0; i < pixels.Length; i++)
                {
                    int x = i % 256;
                    int y = i / 256;

                    if ((x - lastX >= 8 || y - lastY >= 8) && pixels[i].g - pixels[i].r > 10)
                    {
                        positions.Add(
                        new Vector3(
                            initialDisplacement.Value.x + (mapPosition.x * tileScale.x * 256) + tileScale.x * x,
                            0,
                            initialDisplacement.Value.y +(mapPosition.y * tileScale.y * 256) + tileScale.y * y
                            )
                        );
                        lastX = x;
                        lastY = y;
                    }
                }
                Debug.Log("stop " + positions.Count);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void AddTree(Vector3 position)
        {
            GameObject go = GameObject.Instantiate(tree);
            go.transform.position = position;
            go.transform.localScale = new Vector3(5, 5, 5);
        }

        public void OnPlayerMovement()
        {
            List<Vector3> closeTrees = positions.FindAll(position => Vector3.Distance(
                position,
                drawPosition.Value
                ) < 500);
            foreach (Vector3 treePosition in closeTrees.ToList())
            {
                float? height = FunkySheep.Procedural.Earth.SO.GetHeight(treePosition);
                if (height != null)
                {
                    AddTree(
                        new Vector3(
                            treePosition.x,
                            height.Value,
                            treePosition.z
                        ));

                    positions.Remove(treePosition);
                }
            }
        }

        public void AddedEarthTile(FunkySheep.Procedural.Earth.Manager manager, FunkySheep.Procedural.Earth.Tile tile)
        {
            earthTiles.Add(tile);
        }
    }    
}
