using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace Game.Building.Door
{
    public class Manager : MonoBehaviour
    {
        public Material material;
        private int seed = 0;
        public float width = 1;
        public float height = 2;

        public float heightPosition;
        public float frontPosition;
        public float thickness = 0.2f;
        Game.Building.Walls.Wall.Manager wall;

        private void Awake()
        {
            heightPosition = Mathf.Infinity;
            frontPosition = Mathf.Infinity;
        }
        
        public void SetSeed(int seed)
        {
            this.seed = seed;
            Random.InitState(seed);
        }

        public bool Create(Game.Building.Walls.Wall.Manager wall, int seed = 0)
        {
            if (Evaluate())
            {
                this.wall = wall;
                SetSeed(seed);
                height = wall.height - 0.5f;
                width = Random.Range(1f, Vector3.Distance(wall.start, wall.end) - 0.5f);
                thickness = thickness = width * 0.1f;

                CreateStairs();
                CreateFrame();
                CreateBorders();
                return true;
            } else {
                return false;
            }
        }

        public bool Evaluate()
        {
            GetHeightPosition();
            if (heightPosition < 3 && ChechNeightbours())
            {
                return true;
            } else {
                return false;
            }
        }

        void GetHeightPosition()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 10;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position -transform.forward * 0.2f, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position -transform.forward * 0.2f, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                heightPosition = hit.distance;
            } else {
                Debug.Log("Cannot find terrain");
            }
        }

        bool ChechNeightbours()
        {
            bool valid = true;
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 20;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            Debug.DrawRay(transform.position + transform.up * 3, (transform.TransformDirection(Vector3.forward - Vector3.up * 2)) * 20, Color.magenta, 600);
            if (Physics.Raycast(transform.position + transform.up * 3, (transform.TransformDirection(Vector3.forward - Vector3.up * 2)), out hit, Mathf.Infinity, layerMask))
            {
                return false;
            }

            return valid;       
        }

        public void CreateFrame()
        {
            CreateFrameHead();
            CreateFrameJambs();
        }

        public void CreateStairs()
        {
            Vector3 size = new Vector3(width, heightPosition, heightPosition);
            ProBuilderMesh stairs = ShapeGenerator.GenerateStair(PivotLocation.Center, size, Mathf.CeilToInt(heightPosition / 0.2f), true);
            stairs.gameObject.transform.parent = transform;
            stairs.gameObject.transform.localRotation = Quaternion.identity;
            stairs.gameObject.transform.Rotate(Vector3.up * 180);
            stairs.gameObject.transform.localPosition =  new Vector3(0, -heightPosition * 0.5f, heightPosition / 2);
            stairs.GetComponent<MeshRenderer>().material = material;
            stairs.gameObject.AddComponent<MeshCollider>();
        }

        public void CreateFrameHead()
        {
            ProBuilderMesh frameHead = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(width, thickness, thickness));
            frameHead.gameObject.AddComponent<MeshCollider>();
            frameHead.transform.parent = transform;
            frameHead.transform.localPosition = new Vector3(0, wall.height -thickness / 2 , -thickness / 2);
            frameHead.gameObject.transform.localRotation = Quaternion.identity;
            frameHead.gameObject.GetComponent<MeshRenderer>().material = material;
            frameHead.gameObject.AddComponent<MeshCollider>();
        }

        public void CreateFrameJambs()
        {
            ProBuilderMesh frameJambLeft = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, wall.height, thickness));
            frameJambLeft.transform.parent = transform;
            frameJambLeft.transform.localPosition = Vector3.zero + new Vector3(-width * 0.5f + thickness / 2, wall.height * 0.5f, -thickness / 2);
            frameJambLeft.gameObject.transform.localRotation = Quaternion.identity;
            frameJambLeft.gameObject.GetComponent<MeshRenderer>().material = material;
            frameJambLeft.gameObject.AddComponent<MeshCollider>();

            ProBuilderMesh frameRightJambe = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, wall.height, thickness));
            frameRightJambe.transform.parent = transform;
            frameRightJambe.transform.localPosition = Vector3.zero + new Vector3(width * 0.5f - thickness / 2, wall.height * 0.5f, -thickness / 2);
            frameRightJambe.gameObject.transform.localRotation = Quaternion.identity;
            frameRightJambe.gameObject.GetComponent<MeshRenderer>().material = material;
            frameRightJambe.gameObject.AddComponent<MeshCollider>();
        }

        public void CreateBorders()
        {
            Vector3 center = (wall.end - wall.start) / 2;
            Vector3 start = wall.start - center;
            Vector3 startInside = start + (wall.startInside - wall.start);
            Vector3 end = wall.end - center;
            Vector3 endInside = end + (wall.endInside - wall.end);

            IList<Vector3> leftVect = new List<Vector3> {
                Vector3.zero - transform.right * width / 2,
                start,
                startInside,
                -transform.forward * thickness - transform.right * width / 2
            };

            ProBuilderMesh borderLeft = ProBuilderMesh.Create();
            borderLeft.CreateShapeFromPolygon(leftVect, wall.height, false);
            borderLeft.transform.parent = transform;
            borderLeft.transform.localPosition = Vector3.zero;
            borderLeft.gameObject.GetComponent<MeshRenderer>().material = material;
            borderLeft.gameObject.AddComponent<MeshCollider>();

            IList<Vector3> rightVect = new List<Vector3> {
                Vector3.zero + transform.right * width / 2,
                end,
                endInside,
                -transform.forward * thickness + transform.right * width / 2
            };

            ProBuilderMesh borderRight = ProBuilderMesh.Create();
            borderRight.CreateShapeFromPolygon(rightVect, wall.height, false);
            borderRight.transform.parent = transform;
            borderRight.transform.localPosition = Vector3.zero;
            borderRight.gameObject.GetComponent<MeshRenderer>().material = material;
            borderRight.gameObject.AddComponent<MeshCollider>();

        }
    }    
}
