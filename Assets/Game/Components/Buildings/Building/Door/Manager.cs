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

        private void Awake()
        {
            heightPosition = Mathf.Infinity;
            frontPosition = Mathf.Infinity;
            material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.green);
        }
        
        public void SetSeed(int seed)
        {
            this.seed = seed;
            Random.InitState(seed);
        }

        public void Create(int seed = 0)
        {
            GetHeightPosition();
            SetSeed(seed);
            height = Random.Range(2.5f, 3.5f);
            width = height * Random.Range(0.5f, 1);
            thickness = Random.Range(0.1f, 0.5f);

            CreateStairs();
            CreateFrame();
            Evaluate();
        }

        public bool Evaluate()
        {
            if (heightPosition < 3 && ChechNeightbours())
            {
                material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.green);
                return true;
            } else {
                material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.red);
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
            Debug.DrawRay(transform.position + transform.up, (transform.TransformDirection(Vector3.forward - Vector3.up * 2)) * 20, Color.magenta, 600);
            if (Physics.Raycast(transform.position + transform.up, (transform.TransformDirection(Vector3.forward - Vector3.up * 2)), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.distance < 5)
                {
                    return false;
                }
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
        }

        public void CreateFrameHead()
        {
            ProBuilderMesh frameHead = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(width, width * 0.5f, thickness));
            frameHead.transform.parent = transform;
            frameHead.transform.localPosition = new Vector3(0, height - width * 0.25f , -thickness / 2);
            frameHead.gameObject.transform.localRotation = Quaternion.identity;
            frameHead.gameObject.GetComponent<MeshRenderer>().material = material;

            int count = (int)Random.Range(3, 10);
            float archThickness = thickness * 1.2f;
            ProBuilderMesh frameHeadArch = ShapeGenerator.GenerateArch(PivotLocation.Center, 180, width * 0.5f, width * 0.5f, archThickness, count, true, true, true, true, true);
            frameHeadArch.transform.parent = transform;
            frameHeadArch.transform.localPosition = new Vector3(0, height - width * 0.25f , -thickness / 2 + (archThickness - thickness));
            frameHeadArch.gameObject.transform.localRotation = Quaternion.identity;
            frameHeadArch.gameObject.GetComponent<MeshRenderer>().material = material;
        }

        public void CreateFrameJambs()
        {
            ProBuilderMesh frameJambLeft = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, height, thickness));
            frameJambLeft.transform.parent = transform;
            frameJambLeft.transform.localPosition = Vector3.zero + new Vector3(-width * 0.5f, height * 0.5f, -thickness / 2);
            frameJambLeft.gameObject.transform.localRotation = Quaternion.identity;
            frameJambLeft.gameObject.GetComponent<MeshRenderer>().material = material;

            ProBuilderMesh frameRightJambe = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, height, thickness));
            frameRightJambe.transform.parent = transform;
            frameRightJambe.transform.localPosition = Vector3.zero + new Vector3(width * 0.5f, height * 0.5f, -thickness / 2);
            frameRightJambe.gameObject.transform.localRotation = Quaternion.identity;
            frameRightJambe.gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }    
}
