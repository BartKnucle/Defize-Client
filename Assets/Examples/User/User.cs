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
      if (!PlayerPrefs.HasKey("user")) {
        _id.Value = Guid.NewGuid().ToString();
        PlayerPrefs.SetString("user", _id.Value);
        PlayerPrefs.Save();
      } else {
          _id.Value = PlayerPrefs.GetString("user");
      }
      setId.execute();
    }
}
