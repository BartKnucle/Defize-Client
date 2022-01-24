using UnityEngine;

namespace Game.World
{
  [CreateAssetMenu(menuName = "FunkySheep/Game/World")]
  public class SO : FunkySheep.SO
  {
    public FunkySheep.Types.Int zoom;
    public FunkySheep.Types.Double latitude;
    public FunkySheep.Types.Double longitude;

    public FunkySheep.Types.Double startLatitude;
    public FunkySheep.Types.Double startLongitude;
    public FunkySheep.Types.Double endLatitude;
    public FunkySheep.Types.Double endLongitude;

    public override void Create (FunkySheep.Manager manager)
    {
      base.Create(manager);
      CalculateGPSBoundaries();
    }

    public void CalculateGPSBoundaries()
    {
      double[] boundaries = FunkySheep.Map.Utils.CaclulateGpsBoundaries(zoom.Value, latitude.Value, longitude.Value);
      startLatitude.Value = boundaries[0];
      startLongitude.Value = boundaries[1];
      endLatitude.Value = boundaries[2];
      endLongitude.Value = boundaries[3];
    }
  }
}
