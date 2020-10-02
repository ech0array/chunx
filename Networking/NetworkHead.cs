using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class NetworkHead : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    private List<GameObject> networkPrefabs;
    [SerializeField]
    private Dictionary<string, GameObject> networkPrefabsNameGameObjectMap = new Dictionary<string, GameObject>();

    #region Unity Framework Entry Functions
    private void Awake()
    {
        foreach (GameObject gameObject in networkPrefabs)
            networkPrefabsNameGameObjectMap.Add(gameObject.name, gameObject);

        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        DevelopmentHead.Track("network_status", "disconnected", Color.red);
        DevelopmentHead.Track("network_room", null);
        DevelopmentHead.Track("network_nickname", PhotonNetwork.LocalPlayer.NickName);
        DevelopmentHead.Track("network_actor_id", PhotonNetwork.LocalPlayer.UserId);
        DevelopmentHead.Track("network_server_address", null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            Connect();
        if (Input.GetKeyDown(KeyCode.F2))
            CreateRoom();
        if (Input.GetKeyDown(KeyCode.F3))
            JoinRoom();
        if (Input.GetKeyDown(KeyCode.F4))
            IntantiateEditable(networkPrefabs[0], Vector3.zero, Quaternion.identity);

    }
    #endregion


    #region Connection Functions
    internal void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.OfflineMode = false;
        DevelopmentHead.Track("network_status", "connecting", Color.yellow);
        PhotonNetwork.GameVersion = "X : 1.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    internal void Disconnect()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        PhotonNetwork.Disconnect();
    }

    internal void JoinRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinRoom("test");
    }
    internal void CreateRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.CreateRoom("test");
    }


    internal void IntantiateEditable(string prefabName, Vector3 position, Quaternion rotation, int photonId)
    {
        GameObject instance = IntantiateEditable(networkPrefabsNameGameObjectMap[prefabName], position, rotation, false);
        PhotonView photonView = instance.GetComponent<PhotonView>();
        photonView.ViewID = photonId;
    }
    internal GameObject IntantiateEditable(GameObject gameObject, Vector3 position, Quaternion rotation, bool dispatch = true)
    {
        GameObject instance = Instantiate(gameObject, position, rotation);

        if (dispatch)
        {
            PhotonView photonView = instance.GetComponent<PhotonView>();
            bool allocated = PhotonNetwork.AllocateViewID(photonView);
            if (allocated)
            {
                object[] data = new object[]
                {
                    gameObject.name,
                    instance.transform.position,
                    instance.transform.rotation,
                    photonView.ViewID
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent(32, data, raiseEventOptions, sendOptions);
                return instance;
            }
            else
            {
                Destroy(instance);
                return null;
            }
        }
        else
        {
            return instance;
        }
    }

    #endregion

    #region Photon Network Framework Functions
    public override void OnConnectedToMaster()
    {
        DevelopmentHead.Track("network_status", "connected", Color.green);
        DevelopmentHead.Track("network_is_master_client", PhotonNetwork.IsMasterClient);
        DevelopmentHead.Track("network_server_address", PhotonNetwork.ServerAddress);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DevelopmentHead.Track("network_status", "disconnected", Color.red);
    }

    public override void OnJoinedRoom()
    {
        DevelopmentHead.Log("network_head: joined room", LogType.Log);
        DevelopmentHead.Track("network_room", PhotonNetwork.CurrentRoom.Name);
        DevelopmentHead.Track("network_actor_id", photonView.OwnerActorNr);
        DevelopmentHead.Track("network_room_population", PhotonNetwork.PlayerList.Length);
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        DevelopmentHead.Log("network_head: joined room", LogType.Log);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        base.OnPlayerEnteredRoom(player);
        DevelopmentHead.Track("network_is_master_client", PhotonNetwork.IsMasterClient);
        DevelopmentHead.Log($"network_head: a player with the actor id of {player.ActorNumber} joined the room", LogType.Log);
        DevelopmentHead.Track("network_room_population", PhotonNetwork.PlayerList.Length);
    }
    public override void OnPlayerLeftRoom(Player player)
    {
        DevelopmentHead.Track("network_is_master_client", PhotonNetwork.IsMasterClient);
        DevelopmentHead.Log($"network_head: a player with the actor id of {player.ActorNumber} left the room", LogType.Log);
        DevelopmentHead.Track("network_room_population", PhotonNetwork.PlayerList.Length);
    }

    #endregion
    #region Photon Network Event Bindings

    private enum EventType : byte
    {
        InstantiateEditable = 32,
    }
    public void OnEvent(EventData eventData)
    {
        switch ((EventType)eventData.Code)
        {
            case EventType.InstantiateEditable:
                HandleInstantiateEditableEvent(eventData);
                break;
        }
    }

    private void HandleInstantiateEditableEvent(EventData eventData)
    {
        object[] data = (object[])eventData.CustomData;
        IntantiateEditable((string)data[0], (Vector3)data[1], (Quaternion)data[2], (int)data[3]);
    }
    #endregion

}