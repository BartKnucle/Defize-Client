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
      //public static Manager instance;
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

      new void Awake() {
        base.Awake();
      }

      void Start() {
        webSocket = WebSocketFactory.CreateInstance(wsClient.address.Value + ":" + wsClient.port.Value);
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

        int numServices = 0;
        int numRequests = 0;
        
        services.FindAll(service => service.api == msgService)
          .ForEach(service => {
            if (service._id == null || service._id.Value == null || service._id.Value == msgObject["data"]["_id"])
            {
              numServices += 1;
              service.requests.FindAll(request => request.ServerRequestName == msgRequest)
                .ForEach(request => {
                  numRequests += 1;

                  // Set the float variables
                  request.floatVariables
                    .ForEach(variable => {
                      variable.Value = msgObject["data"][variable.DatabaseFieldName];
                    });


                  // Set the int variables
                  request.intVariables
                    .ForEach(variable => {
                      variable.Value = msgObject["data"][variable.DatabaseFieldName];
                    });

                  // Set the string variables
                  request.stringVariables
                    .ForEach(variable => {
                      variable.Value = msgObject["data"][variable.DatabaseFieldName];
                    });

                  // Set the string list variables
                  request.stringListVariables
                    .ForEach(List => {
                      for (int i = 0; i < List.Value.Count; i++)
                      {
                        List.Value[i] = msgObject["data"][List.DatabaseFieldName][i][List.DatabaseSubObjectField]; 
                      }
                    });

                  // Set the bool variables
                  request.boolVariables
                    .ForEach(variable => {
                      variable.Value = msgObject["data"][variable.DatabaseFieldName];
                    });

                  jobs.Enqueue(request.onReception.Raise);
                });
            }
          });

          string countSvc = $" Mathing services count: {numServices}";
          string countRqst = $" Mathing requests count: {numRequests}";

          Debug.Log("Received message: " + strMsg + countSvc + countRqst, this);

          if (numServices == 0) {
            Debug.LogError("No matching service");
          }

          if (numRequests == 0) {
            Debug.LogError("No matching request");
          }
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