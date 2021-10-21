using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using FunkySheep.Variables;
using FunkySheep.Network;

public class Quests : MonoBehaviour
{
    public Service service;

    public StringVariable selectedQuestId;
    public StringVariable quest_name;
    public UIDocument UI;
    public void Awake()
    {
      Button Btn = UI.rootVisualElement.Q<Button>("create-hunt");
      Btn.RegisterCallback<ClickEvent>(this._create);
    }

    public void Start() {
      service.FindRecords();
    }

    /// <summary>
    /// Create a new quest
    /// </summary>
    /// <param name="evt">Button ClickEvent</param>
    private void _create(ClickEvent evt)
    {
      quest_name.Value = UI.rootVisualElement.Q<TextField>("hunt-name").text;
      service.CreateRecords();
    }

    /// <summary>
    /// Update the list if the available quests
    /// </summary>
    public void updateList() {
      switch (service.lastRawMsg["method"].Value)
      {
        case "find":
          UI.rootVisualElement.Q<VisualElement>("quests_list").Clear();
          SimpleJSON.JSONArray quests = service.lastRawMsg["data"]["data"].AsArray;
          for (int i = 0; i < quests.Count; i++)
          {
            Label label = new Label(quests[i]["name"]);
            label.name = "quest-" + quests[i]["_id"];
            label.RegisterCallback<ClickEvent, System.String>(this._select, quests[i]["_id"]);

            UI.rootVisualElement.Q<VisualElement>("quests_list").Add(label);
          }
          break;
        case "create":
          Label labelAdd = new Label(service.lastRawMsg["data"]["name"]);
          labelAdd.name = "quest-" + service.lastRawMsg["data"]["_id"];
          labelAdd.RegisterCallback<ClickEvent, System.String>(this._select, service.lastRawMsg["data"]["_id"]);

          UI.rootVisualElement.Q<VisualElement>("quests_list").Add(labelAdd);
          break;
        case "patch":
          UI.rootVisualElement.Q<Label>("quest-" + service.lastRawMsg["data"]["_id"]).text = service.lastRawMsg["data"]["name"];
          break;
        case "update":
          UI.rootVisualElement.Q<Label>("quest-" + service.lastRawMsg["data"]["_id"]).text = service.lastRawMsg["data"]["name"];
          break;
        case "remove":
          UI.rootVisualElement.Q<VisualElement>("quests_list").Remove(UI.rootVisualElement.Q<Label>("quest-" + service.lastRawMsg["data"]["_id"]));
          break;
      }
    }

    /// <summary>
    /// Select a quest from the UI list
    /// </summary>
    /// <param name="evt">Client Event</param>
    /// <param name="_id">Slected quest ID</param>
    private void _select(ClickEvent evt, string _id)
    {
      selectedQuestId.Value = _id;
    }
}
