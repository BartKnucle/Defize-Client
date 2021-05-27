using UnityEngine;
using UnityEngine.UI;
using FunkySheep.Network;
using FunkySheep.Variables;

public class UpdateText : MonoBehaviour
{
  public StringVariable text;
  //  public Request request;

  public void SetText() {
    transform.GetComponent<InputField>().text = (string)text.Value;
  }

  public void SendText() {
    this.text.Value = transform.GetComponent<InputField>().text;
    //  request.execute();
  }
}
