using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using FunkySheep.OSM;

[RequireComponent(typeof(OSMManager))]
public class MapManager : MonoBehaviour
{
    public UIDocument document;
    OSMManager oSMManager;

    public FunkySheep.Types.Float headingValue;
    FunkySheep.Types.Int size;
    VisualElement heading;
    VisualElement map;
    public Tilemap tilemap;
    
    void Start()
    {
        oSMManager = GetComponent<OSMManager>();
        size = oSMManager.size;
        map = document.rootVisualElement.Q<VisualElement>("ve-map-background");
        heading = document.rootVisualElement.Q<VisualElement>("ve-map-arrow");
    }

    public void Refresh() {
        int center = ( oSMManager.size.Value - 1 ) / 2;

        tilemap.ClearAllTiles();

        for (int x = 0; x < oSMManager.size.Value; x++)
        {
            for (int y = 0; y < oSMManager.size.Value; y++)
            {
                Vector3Int position = new Vector3Int(x, -y, 0);

                tilemap.SetTile(position, oSMManager.tiles[x, y].tile);
                tilemap.RefreshTile(position);

                if (x == center && y == center)
                    map.style.backgroundImage = new StyleBackground((Texture2D)oSMManager.tiles[x, y].texture);
            }
        }
    }

    private void Update() {
        tilemap.tileAnchor = new Vector3(-oSMManager.xInsidePosition.Value, oSMManager.yInsidePosition.Value, 0);
        Rotate(heading, headingValue.Value);
    }

    /// <summary>
    /// Rotate the compass
    /// </summary>
    /// <param name="item"></param>
    /// <param name="angleDegrees"></param>
    public void Rotate(VisualElement item, float angleDegrees)
    {
        if (angleDegrees != 0) {
            // Get the x/y location of the center note - these are constant unless the parent rescales; i.e., they don't change with rotation.
            float x0 = item.contentRect.center.x;
            float y0 = item.contentRect.center.y;
    
            // Convert Cartesian to Polar
            float r = Mathf.Sqrt(x0 * x0 + y0 * y0);
            float theta0 = Mathf.Atan2(y0, x0);
    
            // Calculate the location of the center of the VisualElement after rotating
            // Note: The rotation you want is *in addition* to the "default" polar angle from origin to the center
            float x = r * Mathf.Cos(theta0 + (Mathf.Deg2Rad * angleDegrees));
            float y = r * Mathf.Sin(theta0 + (Mathf.Deg2Rad * angleDegrees));
    
            // Actually do the requested rotation
            item.transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
    
            // Finally, rotation happens about the upper-left corner of the VisualElement, so you need to shift the position
            // to get the rotated center to be coincident with the un-rotated center.
            float xDelta = x0 - x;
            float yDelta = y0 - y;
            item.transform.position = new Vector3(xDelta, yDelta, 0f);
        }
    }
}
