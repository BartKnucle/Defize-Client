using System;
using UnityEngine;
using FunkySheep.Network;
using FunkySheep.Variables;

public class User : GenericSingletonClass<User>
{
    public StringVariable _id;
    public Service service;

    void Start() {
      if (Application.platform != RuntimePlatform.WebGLPlayer)
        setUserId(_id.Value);
    }

    public void setUserId(string Id = "") {
      Debug.Log(Id);
      if (Id == null || Id == "") {
        if (!PlayerPrefs.HasKey("user")) {
          _id.Value = Guid.NewGuid().ToString();
          PlayerPrefs.SetString("user", (string)_id.Value);
          PlayerPrefs.Save();
          service.CreateRecords();
        } else {
          _id.Value = PlayerPrefs.GetString("user");
        }
      } else {
        _id.Value = Id;
      }

      service.PatchRecords();
    }
}
