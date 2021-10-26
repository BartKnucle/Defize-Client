using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunkySheep.Variables;
using FunkySheep.Network;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections.LowLevel.Unsafe;


    public class ARPoint {
    
        StringVariable cloudID;
        StringVariable pointID;
        public FloatVariable x;
        public FloatVariable y;
        public FloatVariable z;
        public StringVariable color;
        public FloatVariable confidence;

        public ARPoint(string cloudId, string pointId, Vector3 pos, Color color, float confidence)
        {
            this.cloudID = ScriptableObject.CreateInstance<StringVariable>();
            this.pointID = ScriptableObject.CreateInstance<StringVariable>();
            this.x = ScriptableObject.CreateInstance<FloatVariable>();
            this.y = ScriptableObject.CreateInstance<FloatVariable>();
            this.z = ScriptableObject.CreateInstance<FloatVariable>();
            this.color = ScriptableObject.CreateInstance<StringVariable>();
            this.confidence = ScriptableObject.CreateInstance<FloatVariable>();

            this.cloudID.Value = cloudId;
            this.pointID.Value = cloudId + "-" + pointId;
            this.color.Value = ColorUtility.ToHtmlStringRGB(color);
            this.confidence.Value = confidence;

            this.x.Value = pos.x;
            this.y.Value = pos.y;
            this.z.Value = pos.z;
        }

        public List<ServiceField> toServiceFieds() {
            List<ServiceField> list = new List<ServiceField>();
            list.Add(new ServiceField("_id", pointID));
            list.Add(new ServiceField("cloudId", cloudID));
            list.Add(new ServiceField("x", x));
            list.Add(new ServiceField("y", y));
            list.Add(new ServiceField("z", z));
            list.Add(new ServiceField("color", color));
            list.Add(new ServiceField("confidence", confidence));

            return list;
        }
    }

public class PointClouds : MonoBehaviour
{
    public Service pointsCloudService;
    public ARPointCloudManager cloudManager;

    public List<ARPoint> points = new List<ARPoint>();
    public ParticleSystem viewer;
    public bool recording = true;
    public float precision = 0.5f;

    public ARCameraManager cameraManager;
    public Camera cam;
    Texture2D m_CameraTexture;

    void Start()
    {
        if (cameraManager && cloudManager && recording) {
            cloudManager.pointCloudsChanged += _cloudChanged;
            cameraManager.frameReceived += OnCameraFrameReceived;
        }

        if (pointsCloudService && !recording) {
            pointsCloudService.FindRecords();
        }
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        UpdateCameraImage();
    }

    unsafe void UpdateCameraImage()
    {
                    // Attempt to get the latest camera image. If this method succeeds,
            // it acquires a native resource that must be disposed (see below).
            if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                return;
            }

            // Display some information about the camera image
            Debug.Log(string.Format(
                "Image info:\n\twidth: {0}\n\theight: {1}\n\tplaneCount: {2}\n\ttimestamp: {3}\n\tformat: {4}",
                image.width, image.height, image.planeCount, image.timestamp, image.format));

            // Once we have a valid XRCpuImage, we can access the individual image "planes"
            // (the separate channels in the image). XRCpuImage.GetPlane provides
            // low-overhead access to this data. This could then be passed to a
            // computer vision algorithm. Here, we will convert the camera image
            // to an RGBA texture and draw it on the screen.

            // Choose an RGBA format.
            // See XRCpuImage.FormatSupported for a complete list of supported formats.
            var format = TextureFormat.RGBA32;

            if (m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height)
            {
                m_CameraTexture = new Texture2D(image.width, image.height, format, false);
            }

            // Convert the image to format, flipping the image across the Y axis.
            // We can also get a sub rectangle, but we'll get the full image here.
            var conversionParams = new XRCpuImage.ConversionParams(image, format);

            // Texture2D allows us write directly to the raw texture data
            // This allows us to do the conversion in-place without making any copies.
            var rawTextureData = m_CameraTexture.GetRawTextureData<byte>();
            try
            {
                image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
            finally
            {
                // We must dispose of the XRCpuImage after we're finished
                // with it to avoid leaking native resources.
                image.Dispose();
            }

            // Apply the updated texture data to our texture
            m_CameraTexture.Apply();
    }

    private  void _cloudChanged (ARPointCloudChangedEventArgs obj) {
        foreach (ARPointCloud pointCloud in obj.added)
        {
            for (int i = 0; i < pointCloud.identifiers.Value.Length; i++)
            {
                if (pointCloud.confidenceValues.Value[i] >= precision) {
                    Vector3 pixelPos = cam.WorldToScreenPoint(pointCloud.positions.Value[i]);
                    ARPoint point = new ARPoint(
                        pointCloud.trackableId.ToString(),
                        pointCloud.identifiers.Value[i].ToString(),
                        pointCloud.positions.Value[i],
                        m_CameraTexture.GetPixel((int)pixelPos.x * m_CameraTexture.width / Screen.width, (int)pixelPos.y * m_CameraTexture.height / Screen.height),
                        pointCloud.confidenceValues.Value[i]
                    );

                    pointsCloudService.fields = point.toServiceFieds();
                    pointsCloudService.CreateRecords();
                }
            }
        }

        foreach (ARPointCloud pointCloud in obj.updated)
        {
            for (int i = 0; i < pointCloud.identifiers.Value.Length; i++)
            {
                if (pointCloud.confidenceValues.Value[i] >= precision) {
                    Vector3 pixelPos = cam.WorldToScreenPoint(pointCloud.positions.Value[i]);
                    ARPoint point = new ARPoint(
                        pointCloud.trackableId.ToString(),
                        pointCloud.identifiers.Value[i].ToString(),
                        pointCloud.positions.Value[i],
                        m_CameraTexture.GetPixel((int)pixelPos.x * m_CameraTexture.width / Screen.width, (int)pixelPos.y * m_CameraTexture.height / Screen.height),
                        pointCloud.confidenceValues.Value[i]
                    );

                    pointsCloudService.fields = point.toServiceFieds();
                    pointsCloudService.CreateRecords();
                }
            }
        }
    }

    // Fill the point cloud with database
    public void fill() {
        SimpleJSON.JSONArray points = pointsCloudService.lastRawMsg["data"]["data"].AsArray;
        for (int i = 0; i < points.Count; i++)
        {
            GameObject cloud;
            if (transform.Find(points[i]["cloudId"]))
            {
                cloud = transform.Find(points[i]["cloudId"]).gameObject;
            } else {
                cloud = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cloud.transform.parent = transform;
                cloud.name = points[i]["cloudId"];
            }
                
            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.parent = cloud.transform;
            point.transform.position = new Vector3(
                points[i]["x"],
                points[i]["y"],
                points[i]["z"]
            );
            
            ColorUtility.TryParseHtmlString("#" + points[i]["color"], out Color color );

            point.GetComponent<MeshRenderer>().material.SetColor("_Color", color);

            point.transform.localScale *= 0.01f;
        }
    }
}
