using UnityEngine;
using UnityEngine.XR.ARFoundation;
using FunkySheep.Network;
using FunkySheep.Events;

namespace Game.Player
{
    public class Manager : MonoBehaviour
  {
      //public Service networkService;
      //public ARSessionOrigin origin;
      public FunkySheep.Types.Vector3 position;
      public FunkySheep.Types.Float currentHeight;
      public FunkySheep.Types.Double calculatedLatitude;
      public FunkySheep.Types.Double calculatedLongitude;
      public FunkySheep.Types.Vector3 calculatedMercatorPosition;
      public FunkySheep.Types.String status;
      public GameEventGO onPlayerStarted;
      public GameEventGO onPlayerMove;

      //public FunkySheep.World.WorldSO worldSO;
      
      Vector3 _lastPosition;

      private void Awake() {
        //GetComponent<CharacterController>().enabled = false;
      }

      public void Create() {
          CalculatePositions();
          position.Value = _lastPosition = transform.position;
          status.Value = "Player Started";
          onPlayerStarted.Raise(this.gameObject);
      }

      void Update()
      {
          CalculatePositions();
          float distance = Vector3.Distance(transform.position, _lastPosition);
          if (this.transform.position == Vector3.zero)
          {
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z) + Vector3.up * currentHeight.Value;
            if (this.transform.position != Vector3.zero)
            {
              GetComponent<CharacterController>().enabled = true;
            }
            
          }

          if (distance >= 10) {
              onPlayerMove.Raise(this.gameObject);
              //networkService.CreateRecords();
              _lastPosition = transform.position;
          }
      }

      public void CalculatePositions() {
          position.Value = transform.position;
          var calculatedGPS = FunkySheep.GPS.Utils.toGeoCoord(position.Value + FunkySheep.GPS.Manager.Instance.initialMercatorPosition.Value);
          calculatedLatitude.Value = calculatedGPS.latitude;
          calculatedLongitude.Value = calculatedGPS.longitude;
          calculatedMercatorPosition.Value = FunkySheep.GPS.Utils.toCartesianVector(calculatedLatitude.Value, calculatedLongitude.Value);
      }

      public void onBuildEnter(GameObject buildingGo)
      {
        GetComponent<Builder>().buildingGo = buildingGo;
        GetComponent<Builder>().enabled = true;
      }

      public void onBuildExit(GameObject buildingGo)
      {
        GetComponent<Builder>().enabled = false;
        GetComponent<Builder>().buildingGo = null;
      }
  }
    
}
