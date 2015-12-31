using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    const string VERSION = "1.0";
    public string roomName = "Room 69";

    GameObject globalScriptsObject;

    GameObject gameMapCamera;

    Transform[] spawnPoints;
    SpawnSystem spawnPointsFromSpawnSystem;

    public Transform spawnPosition;//This is for now, spawn system will come...

	// Use this for initialization
	void Start () {
        Debug.Log("Sakums!");
        PhotonNetwork.ConnectUsingSettings(VERSION);

        gameMapCamera = GameObject.Find("Waiting_Camera");
        globalScriptsObject = GameObject.Find("_GlobalScripts");

        spawnPointsFromSpawnSystem = globalScriptsObject.GetComponent<SpawnSystem>();

        spawnPoints = spawnPointsFromSpawnSystem.possibleSpawnPoints;
        
	}

    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby()");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed()");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()");

        spawnPlayer();
    }

    void spawnPlayer()
    {

        if (spawnPoints == null)
        {
            PhotonNetwork.Instantiate("PlayerController", spawnPosition.position, Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerController", spawnPoints[Random.Range(0, spawnPoints.Length)].position, spawnPoints[Random.Range(0, spawnPoints.Length)].rotation, 0);
        }
        

        gameMapCamera.active = false;//When Player spawns no need for Map Camera.
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
