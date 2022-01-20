using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Network;
using FunkySheep.Events;

namespace Game.Player
{
    public class Manager : MonoBehaviour
  {
      public Service networkService;
      //public ARSessionOrigin origin;
      public FunkySheep.Types.Vector3 position;
      public FunkySheep.Types.Double calculatedLatitude;
      public FunkySheep.Types.Double calculatedLongitude;
      public FunkySheep.Types.Vector3 calculatedMercatorPosition;
      public FunkySheep.Types.String status;
      public GameEvent onPlayerStarted;
      public GameEvent onPlayerMove;
      
      Vector3 _lastPosition;

      public void Create() {
          CalculatePositions();
          position.Value = _lastPosition = transform.position;
          status.Value = "Player Started";
          onPlayerStarted.Raise();
      }

      void Update()
      {
          CalculatePositions();
          float distance = Vector3.Distance(transform.position, _lastPosition);

          if (distance >= 10) {
              onPlayerMove.Raise();
              networkService.CreateRecords();
              _lastPosition = transform.position;
          }
      }

      public void CalculatePositions() {
          position.Value = transform.position;
          var calculatedGPS = FunkySheep.GPS.Utils.toGeoCoord(position.Value + FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value);
          calculatedLatitude.Value = calculatedGPS.latitude;
          calculatedLongitude.Value = calculatedGPS.longitude;
          calculatedMercatorPosition.Value = FunkySheep.GPS.Utils.toCartesianVector(calculatedLatitude.Value, calculatedLongitude.Value);

          /*Vector3 gpsPosition = FunkySheep.GPS.Manager.Instance.currentMercatorPosition.Value - FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value;
          if (Vector3.Distance(gpsPosition, position.Value) < 1 ) {
              this.transform.position = gpsPosition;
          }*/
      }
  }
    
}
