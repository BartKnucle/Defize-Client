using System;
using System.Text;
using System.Collections.Generic;
using FunkySheep.Variables;
using FunkySheep.Events;
using NativeWebSocket;
using SimpleJSON;
using UnityEngine;


namespace FunkySheep.Network {
  [AddComponentMenu("FunkySheep/Network/Manager")]
  public class Manager : GenericSingletonClass<Manager>
  {
      public WsClient wsClient;
      public List<Service> services;
      public WebSocket webSocket;
      public GameEvent onStatusChanged;
      public StringVariable status;
      public Double startedAt;
      Queue<Action> jobs = new Queue<Action>();
      public Queue<String> messages = new Queue<String>();

      internal void AddJob(Action newJob) {
        jobs.Enqueue(newJob);
      }

      void Start() {
        webSocket = WebSocketFactory.CreateInstance(wsClient.address + ":" + wsClient.port);
        //  Binding the events
        webSocket.OnOpen += onConnectionOpen;
        webSocket.OnClose += onConnectionClose;
        webSocket.OnError += onConnectionError;
        webSocket.OnMessage += onMessage;

        status.Value = "";
        webSocket.Connect();
      }

      void Update() {
        #if !UNITY_WEBGL || UNITY_EDITOR
          this.webSocket.DispatchMessageQueue();
        #endif

        while (jobs.Count > 0) 
            jobs.Dequeue().Invoke();

        if (status.Value == "Connected") {
          while (messages.Count > 0) 
          {
            Send(messages.Dequeue());
          }
        }
      }

      private void Send(string data) {
          webSocket.Send(Encoding.UTF8.GetBytes(data));
      }

      void changeStatus(string status) {
        this.status.Value = status;
        jobs.Enqueue(onStatusChanged.Raise);
      }

      private void onConnectionOpen() {
        startedAt = Time.Now();
        changeStatus("Connected");
      }

      private void onConnectionClose(WebSocketCloseCode code) {
        changeStatus("Disconnected");
      }

        private void onMessage(byte[] msg) {
        string strMsg = Encoding.UTF8.GetString(msg);
        JSONNode msgObject = JSON.Parse(strMsg);
        string msgService = msgObject["data"]["service"];
        string msgRequest = msgObject["data"]["request"];
        
        services.FindAll(service => service.api == msgService)
          .ForEach(service => {
            service.fields.ForEach(field => {
              field.fromJSONNode(msgObject["data"][field.name]);
            });

            //  Raise the event
            if (service.onReception) {
              service.onReception.Raise();
            }
          });

          Debug.Log("Received message: " + strMsg + this);
      }

      private void onConnectionError(string errMsg) {
        changeStatus(errMsg);
      }
      
      void OnApplicationQuit()
      {
        if (status.Value == "Connected") {
          webSocket.Close();
        }
      }
  }
}