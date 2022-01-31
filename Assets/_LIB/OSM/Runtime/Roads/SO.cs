using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.OSM.Roads
{
  [CreateAssetMenu(menuName = "FunkySheep/OSM/Roads")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Types.String urlTemplate;
    public FunkySheep.OSM.Events.OSMEventData onOSMRoadsDownloaded;

    public void Download(Manager manager, double[] boundaries)
    {
      manager.StartCoroutine(FunkySheep.Network.Downloader.Download(InterpolatedUrl(boundaries), (fileID, file) => {
        manager.files.Enqueue(file);
      }));
    }

    /// <summary>
    /// Interpolate the url inserting the boundaries and the types of OSM data to download
    /// </summary>
    /// <param boundaries="boundaries">The gps boundaries to download in</param>
    /// <returns>The interpolated Url</returns>
    public string InterpolatedUrl(double[] boundaries)
    {
        string [] parameters = new string[5];
        string [] parametersNames = new string[5];

        parameters[0] = boundaries[0].ToString().Replace(',', '.');
        parametersNames[0] = "startLatitude";

        parameters[1] = boundaries[1].ToString().Replace(',', '.');
        parametersNames[1] = "startLongitude";

        parameters[2] = boundaries[2].ToString().Replace(',', '.');
        parametersNames[2] = "endLatitude";

        parameters[3] = boundaries[3].ToString().Replace(',', '.');
        parametersNames[3] = "endLongitude";

        return urlTemplate.Interpolate(parameters, parametersNames);
    }
  }
}
