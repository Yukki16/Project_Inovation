using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{
    [SerializeField] private CameraScript defaultCamera;
    [SerializeField] private GameObject twoPlayerScreen;
    [SerializeField] private GameObject threePlayerScreen;
    [SerializeField] private GameObject fourPlayerScreen;

    private void Start()
    {
        TCMiniGameStateManager.Instance.OnAllPlayersJoined += Player_OnPlayerJoin;
        Player.OnPlayerLeave += Player_OnPlayerLeave;
    }

    private void Player_OnPlayerLeave(object sender, Player.OnPlayerLeaveArgs e)
    {
        UpdateScreenView(NetworkManager.Singleton.ConnectedClientsIds.Count - 1, e.clientId);

    }

    private void Player_OnPlayerJoin(object sender, System.EventArgs e)
    {
        UpdateScreenView(NetworkManager.Singleton.ConnectedClientsIds.Count);
    }

    public void UpdateScreenView(int amountOfPlayersConnected, ulong disconnectedClient = default)
    {
        if (IsServer)
        {
            DeleteExistingScreens(amountOfPlayersConnected);
            if (amountOfPlayersConnected >= 2)
            {
                switch (amountOfPlayersConnected)
                {
                    case 2:
                        GameObject twoPlScreen = Instantiate(twoPlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Singleton.ConnectedClientsList)
                        {
                            if (connectedPlayer.ClientId != disconnectedClient || disconnectedClient == default)
                            {
                                CameraScript currentCamera = twoPlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedPlayer.PlayerObject.GetComponent<Player>().transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedPlayer.PlayerObject.GetComponent<Player>().transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition,currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedPlayer.PlayerObject.GetComponent<Player>().transform);
                            }
                        }
                        break;
                    case 3:
                        GameObject threePlScreen = Instantiate(threePlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Singleton.ConnectedClientsList)
                        {
                            Debug.Log(connectedPlayer.ClientId);
                            if (connectedPlayer.ClientId != disconnectedClient || disconnectedClient == default)
                            {
                                CameraScript currentCamera = threePlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedPlayer.PlayerObject.GetComponent<Player>().transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedPlayer.PlayerObject.GetComponent<Player>().transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedPlayer.PlayerObject.GetComponent<Player>().transform);
                            }
                        }
                        break;
                    case 4:
                        GameObject fourPlScreen = Instantiate(fourPlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Singleton.ConnectedClientsList)
                        {
                            if (connectedPlayer.ClientId != disconnectedClient || disconnectedClient == default)
                            {
                                CameraScript currentCamera = fourPlayerScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedPlayer.PlayerObject.GetComponent<Player>().transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedPlayer.PlayerObject.GetComponent<Player>().transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedPlayer.PlayerObject.GetComponent<Player>().transform);
                            }
                        }
                        break;
                }
            }
        }
        else if (IsClient)
        {
            defaultCamera.gameObject.SetActive(false);
        }
    }

    private void DeleteExistingScreens(int amountOfPlayersConnected)
    {
        if (amountOfPlayersConnected >= 2)
        {
            defaultCamera.gameObject.SetActive(false);
        }
        else
        {
            defaultCamera.gameObject.SetActive(true);
        }

        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            CameraScript[] playerCameras = player.GetComponentsInChildren<CameraScript>();
            if (playerCameras.Length >= 1)
            {
                foreach (var playerCamera in playerCameras)
                {
                    Destroy(playerCamera.gameObject);
                }
            }       
        }

        GameObject[] twoPlayerScreenGO = GameObject.FindGameObjectsWithTag("TwoPlayerScreen");
        
        if (twoPlayerScreenGO != default)
        {
            foreach (var twoPlScreen in twoPlayerScreenGO)
            {
                Destroy(twoPlScreen);
            }
            
        }

        GameObject[] threePlayerScreenGO = GameObject.FindGameObjectsWithTag("ThreePlayerScreen");
        if (threePlayerScreenGO != default)
        {
            foreach (var threePLS in threePlayerScreenGO)
            {
                Destroy(threePLS);
            }
        }

        GameObject[] fourPlayerScreenGO = GameObject.FindGameObjectsWithTag("FourPlayerScreen");
        if (fourPlayerScreenGO != default)
        {
            foreach (var fourPLS in fourPlayerScreenGO)
            {
                Destroy(fourPLS);
            }
        }
    }
}
