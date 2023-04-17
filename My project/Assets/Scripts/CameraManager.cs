using Assets.Scripts;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CameraScript defaultCamera;
    [SerializeField] private GameObject twoPlayerScreen;
    [SerializeField] private GameObject threePlayerScreen;
    [SerializeField] private GameObject fourPlayerScreen;

    private void Awake()
    {
        Instance = this;
        Debug.Log("Subscribe");
        NetworkManager.Instance.OnAllPlayersJoined += Instance_OnAllPlayersJoined;
        
    }

    private void Instance_OnAllPlayersJoined(object sender, System.EventArgs e)
    {
        UpdateScreenView(NetworkManager.Instance.GetAmountOfConnectedPlayers());
    }

    public void UpdateScreenView(int amountOfPlayersConnected)
    {
            DeleteExistingScreens(amountOfPlayersConnected);
            if (amountOfPlayersConnected >= 2)
            {
                switch (amountOfPlayersConnected)
                {
                    case 2:
                        GameObject twoPlScreen = Instantiate(twoPlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Instance.GetAllPlayers())
                        {
                            if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
                            {
                                TCPLayer connectedTCPlayer = connectedPlayer as TCPLayer;
                                CameraScript currentCamera = twoPlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedTCPlayer.transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedTCPlayer.transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedTCPlayer.transform);
                            }
                            else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
                            {
                                PlayerCharacter connectedGlidingPlayer = connectedPlayer as PlayerCharacter;
                                CameraScript currentCamera = twoPlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, -currentCamera.transform.eulerAngles.y, 0);
                                currentCamera.transform.SetParent(connectedGlidingPlayer.transform);
                                currentCamera.transform.SetLocalPositionAndRotation(Vector3.zero, currentCamera.transform.rotation);
                        }
                            
                        }
                        break;
                    case 3:
                        GameObject threePlScreen = Instantiate(threePlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Instance.GetAllPlayers())
                        {
                            
                            if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
                            {
                                TCPLayer connectedTCPlayer = connectedPlayer as TCPLayer;
                                CameraScript currentCamera = threePlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedTCPlayer.transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedTCPlayer.transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedTCPlayer.transform);
                            }
                            else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
                            {
                                PlayerCharacter connectedGlidingPlayer = connectedPlayer as PlayerCharacter;
                                CameraScript currentCamera = threePlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedGlidingPlayer.transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedGlidingPlayer.transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedGlidingPlayer.transform);
                            }

                    }
                        break;
                    case 4:
                        GameObject fourPlScreen = Instantiate(fourPlayerScreen, transform.position, transform.rotation);
                        foreach (var connectedPlayer in NetworkManager.Instance.GetAllPlayers())
                        {
                            if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
                            {
                                TCPLayer connectedTCPlayer = connectedPlayer as TCPLayer;
                                CameraScript currentCamera = fourPlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedTCPlayer.transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedTCPlayer.transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedTCPlayer.transform);
                            }
                            else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
                            {
                                PlayerCharacter connectedGlidingPlayer = connectedPlayer as PlayerCharacter;
                                CameraScript currentCamera = fourPlScreen.GetComponentsInChildren<CameraScript>()[0];
                                currentCamera.transform.Rotate(0, connectedGlidingPlayer.transform.eulerAngles.y - currentCamera.transform.eulerAngles.y, 0);
                                Vector3 newPosition = new Vector3(currentCamera.transform.position.x, connectedGlidingPlayer.transform.position.y, currentCamera.transform.position.z);
                                currentCamera.transform.SetPositionAndRotation(newPosition, currentCamera.transform.rotation);
                                currentCamera.transform.SetParent(connectedGlidingPlayer.transform);
                            }
                        }
                        break;
                }
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

        foreach (TCPLayer player in GameObject.FindObjectsOfType<TCPLayer>())
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
