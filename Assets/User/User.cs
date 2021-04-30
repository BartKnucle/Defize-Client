using System;
using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Variables;

public class User : GenericSingletonClass<User>
{
    public FunkySheep.Network.Variables.NetStringVariable _id;

    public Request setId;
           
    bool _team;

    new void Awake() {
        base.Awake();
    }

    void Start() {
      if (Application.platform != RuntimePlatform.WebGLPlayer)
        _setUserId();
    }

    public void defineId(string Id) {
      _id.Value = Id;
      setId.execute();
    }

    void _setUserId(String id="") {
      if (!PlayerPrefs.HasKey("user")) {
        if (id != "") {
          _id.Value = id;
        } else {
          _id.Value = Guid.NewGuid().ToString();
        }
        PlayerPrefs.SetString("user", _id.Value);
        PlayerPrefs.Save();
      } else {
          _id.Value = PlayerPrefs.GetString("user");
      }
      setId.execute();
    }
}
