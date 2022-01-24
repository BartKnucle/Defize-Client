using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.OSM.Buildings
{
  [CreateAssetMenu(menuName = "FunkySheep/OSM/Buildings")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Types.String urlTemplate;
    public FunkySheep.Types.Double startLatitude;
    public FunkySheep.Types.Double startLongitude;
    public FunkySheep.Types.Double endLatitude;
    public FunkySheep.Types.Double endLongitude;

    public override void Create(FunkySheep.Manager manager)
    {
      base.Create(manager);
      Download(manager as Manager);
    }

    public void Download(Manager manager)
    {
      manager.StartCoroutine(FunkySheep.Network.Downloader.Download(InterpolatedUrl(), (fileID, file) => {
      }));
    }

    /// <summary>
    /// Interpolate the url inserting the boundaries and the types of OSM data to download
    /// </summary>
    /// <param boundaries="boundaries">The gps boundaries to download in</param>
    /// <returns>The interpolated Url</returns>
    public string InterpolatedUrl()
    {
        string [] parameters = new string[5];
        string [] parametersNames = new string[5];

        parameters[0] = startLatitude.ToString().Replace(',', '.');
        parametersNames[0] = "startLatitude";

        parameters[1] = startLongitude.ToString().Replace(',', '.');
        parametersNames[1] = "startLongitude";

        parameters[2] = endLatitude.ToString().Replace(',', '.');
        parametersNames[2] = "endLatitude";

        parameters[3] = endLongitude.ToString().Replace(',', '.');
        parametersNames[3] = "endLongitude";

        return urlTemplate.Interpolate(parameters, parametersNames);
    }
  }
}
