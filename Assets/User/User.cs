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

    public void setUserId(string Id) {
      if (Application.platform == RuntimePlatform.WebGLPlayer) {
        _id.Value = Id;
      }
      
      service.GetRecord();
    }
}
