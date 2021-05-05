using System;
using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Variables;

public class User : GenericSingletonClass<User>
{
    public StringVariable _id;
    public Service service;
           
    bool _team;

    void Start() {
      if (Application.platform != RuntimePlatform.WebGLPlayer)
        _setUserId();
    }

    public void defineId(string Id) {
      _id.Value = Id;
      //  setId.execute();
    }

    void _setUserId(String id="") {
      if (!PlayerPrefs.HasKey("user")) {
        if (id != "") {
          _id.Value = id;
        } else {
          _id.Value = Guid.NewGuid().ToString();
        }
        PlayerPrefs.SetString("user", (string)_id.Value);
        PlayerPrefs.Save();
        service.CreateRecords();
      } else {
          _id.Value = PlayerPrefs.GetString("user");
          service.PatchRecords();
      }
    }
}
