using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
        Player.OnPlayerJoin += Player_OnPlayerJoin;
    }

    private void Player_OnPlayerJoin(object sender, System.EventArgs e)
    {
        if (IsServer)
        {
            int amountOfPlayersConnected = NetworkManager.Singleton.ConnectedClientsIds.Count;
            if (amountOfPlayersConnected >= 2)
            {
                switch (amountOfPlayersConnected)
                {
                    case 2:
                        Destroy(defaultCamera.gameObject);

                        GameObject twoPlScreen = Instantiate(twoPlayerScreen,transform.position,transform.rotation);      
                        for (int i = 1; i >= 0; i--)
                        {
                            twoPlScreen.GetComponentsInChildren<CameraScript>()[0].transform.SetParent(GameObject.FindObjectsOfType<Player>()[i].transform);
                            Debug.Log("set" + i);
                        }
                        break;
                    case 3:
                        GameObject twoPlayerScreenGO = GameObject.FindGameObjectWithTag("TwoPlayerScreen");
                        if (twoPlayerScreen)
                        {
                            Destroy(twoPlayerScreenGO);
                        }

                        GameObject threePlScreen = Instantiate(threePlayerScreen, transform.position, transform.rotation);
                        for (int i = 2; i >= 0; i--)
                        {
                            threePlScreen.GetComponentsInChildren<CameraScript>()[0].transform.SetParent(GameObject.FindObjectsOfType<Player>()[i].transform);
                        }
                        break;
                    case 4:
                        GameObject threePlayerScreenGO = GameObject.FindGameObjectWithTag("ThreePlayerScreen");
                        if (twoPlayerScreen)
                        {
                            Destroy(threePlayerScreenGO);
                        }
                        

                        GameObject fourPlScreen = Instantiate(fourPlayerScreen, transform.position, transform.rotation);
                        for (int i = 3; i >= 4; i--)
                        {
                            fourPlScreen.GetComponentsInChildren<CameraScript>()[0].transform.SetParent(GameObject.FindObjectsOfType<Player>()[i].transform);
                        }
                        break;
                }
            }
        }
        else if (IsClient)
        {
            Destroy(defaultCamera.gameObject);
        }
    }
}
