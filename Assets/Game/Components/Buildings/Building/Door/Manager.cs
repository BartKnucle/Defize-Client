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
        public GameObject player;
        public Vector3 start;
        public Vector3 end;
        public Material material;
        private int seed = 0;
        public float width = 1;
        public float height = 2;
        public float thickness = 0.2f;
        ProBuilderMesh mesh;

        Vector3 _lastPositon;
        private void Awake()
        {
            _lastPositon = transform.position;
            material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.green);
            SetSeed(1);
        }

        private void Update() {
            Debug.DrawLine(player.transform.position, player.transform.position + player.transform.forward * 10 );
        }

        private void FixedUpdate() {
            if (_lastPositon != transform.position)
            {
                Evaluate();
                _lastPositon = transform.position;
            }
        }
        
        public void SetSeed(int seed)
        {
            this.seed = seed;
            Random.InitState(seed);
            Create();
        }

        public void Create()
        {
            height = Random.Range(2.5f, 3.5f);
            width = height * Random.Range(0.5f, 1);
            thickness = Random.Range(0.1f, 0.5f);
            
            if (mesh != null)
            {
                Destroy(mesh.gameObject);
            }

            CreateStairs();
            CreateFrame();
        }

        public bool Evaluate()
        {
            bool ok = false;
            if (GetHeight() < 1 && GetFront() >= 2)
            {
                ok = true;
                material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.green);
            } else {
                material.SetColor("Color_0c948efe52be4ddeb4993b6f87bd244e", Color.red);
            }

            return ok;
        }

        float GetHeight()
        {
            float distance = 0;
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position -transform.forward * 0.2f, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position -transform.forward * 0.2f, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
                distance = hit.distance;
            } else {
                distance = 1;
            }

            return distance;
        }

        float GetFront()
        {
            float distance = 2;
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position - transform.up * 0.1f, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position - transform.up * 0.1f, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                distance = hit.distance;
            }

            return distance;
        }

        public void CreateFrame()
        {
            CreateFrameHead();
            CreateFrameJambs();
        }

        public void CreateStairs()
        {
            float distance = GetHeight();
            float depth = 1;
            //float stairsHeight = 1;
            Vector3 size = new Vector3(width, distance, depth);
            mesh = ShapeGenerator.GenerateStair(PivotLocation.Center, size, Mathf.CeilToInt(distance / 0.2f), true);
            mesh.transform.parent = transform;
            mesh.transform.Rotate(Vector3.up * 180);
            mesh.transform.position = new Vector3(0, -distance * 0.5f, depth / 2);
            mesh.gameObject.GetComponent<MeshRenderer>().material = material;
        }

        public void CreateFrameHead()
        {
            ProBuilderMesh frameHead = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(width, width * 0.5f, thickness));
            frameHead.transform.parent = mesh.transform;
            frameHead.transform.position = new Vector3(0, height - width * 0.25f , -thickness / 2);
            frameHead.gameObject.GetComponent<MeshRenderer>().material = material;

            int count = (int)Random.Range(3, 10);
            float archThickness = thickness * 1.2f;
            ProBuilderMesh frameHeadArch = ShapeGenerator.GenerateArch(PivotLocation.Center, 180, width * 0.5f, width * 0.5f, archThickness, count, true, true, true, true, true);
            frameHeadArch.transform.parent = mesh.transform;
            frameHeadArch.transform.position = new Vector3(0, height - width * 0.25f , -thickness / 2 + (archThickness - thickness));
            frameHeadArch.gameObject.GetComponent<MeshRenderer>().material = material;
        }

        public void CreateFrameJambs()
        {
            ProBuilderMesh frameJambLeft = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, height, thickness));
            frameJambLeft.transform.parent = mesh.transform;
            frameJambLeft.transform.position = Vector3.zero + new Vector3(-width * 0.5f, height * 0.5f, -thickness / 2);
            frameJambLeft.gameObject.GetComponent<MeshRenderer>().material = material;

            ProBuilderMesh frameRightJambe = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, height, thickness));
            frameRightJambe.transform.parent = mesh.transform;
            frameRightJambe.transform.position = Vector3.zero + new Vector3(width * 0.5f, height * 0.5f, -thickness / 2);;
            frameRightJambe.gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }    
}
