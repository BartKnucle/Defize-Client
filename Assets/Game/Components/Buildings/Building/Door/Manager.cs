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
        public int width = 1;
        public int height = 2;
        public float thickness = 0.2f;
        GameObject frameHead;
        GameObject frameJambs;
        private void Awake()
        {
            SetSeed(1);
        }
        
        public void SetSeed(int seed)
        {
            this.seed = seed;
            Random.InitState(seed);
            Create();
        }

        public void Create()
        {
            width = Random.Range(1, 4);
            height = Random.Range(2, 4);
            thickness = Random.Range(0.1f, 0.5f);
            CreateFrame();
        }

        public void CreateFrame()
        {
            CreateFrameHead();
            CreateFrameJambs();
        }

        public void CreateFrameHead()
        {
            Destroy(frameHead);

            int count = (int)Random.Range(3, 10);
            frameHead = ShapeGenerator.GenerateArch(PivotLocation.Center, 180, width * 0.5f, thickness, thickness, count, true, true, true, true, true).gameObject;
            frameHead.transform.parent = transform;
            frameHead.transform.position += new Vector3(0, height / 2, -thickness / 2);
            frameHead.GetComponent<MeshRenderer>().material = material;
        }

        public void CreateFrameJambs()
        {
            Destroy(frameJambs);
            frameJambs = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(thickness, height, thickness)).gameObject;
            frameJambs.transform.parent = transform;
            frameJambs.transform.position -= new Vector3((width * 0.5f) - thickness / 2, 0, 0);
            frameJambs.GetComponent<MeshRenderer>().material = material;
        }
    }    
}
