using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectSpawner: NetworkBehaviour
{
    [SerializeField] private GameObject[] fallingObject;
    [SerializeField] private GameObject[] powerUpObject;

    public static ObjectSpawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void SpawnObject(Vector3 position)
    {
        GameObject objToSpawn = new GameObject("SpawnedFallingItem");
        Transform newObject = Instantiate(fallingObject[Random.Range(0,fallingObject.Length)].transform, objToSpawn.transform);
        newObject.localPosition += position + new Vector3(0, GameManager.Instance.GetHighestHeightOfAllPlayers() + 30, 0);

        int randomNr = Random.Range(1, 10);
        if (randomNr > 5)
        {
            Vector3 moveDir = Vector3.zero;
            if (randomNr == 6 || randomNr == 7)
            {
                moveDir = new Vector3(0, Random.Range(500,1500), 0);
            }
            else
            {
                moveDir = new Vector3(0, -Random.Range(500, 1500), 0);
            }

            float rotateSpeed = Random.Range(60, 75);
            objToSpawn.transform.Rotate(moveDir, rotateSpeed);
        }

        NetworkObject newObjectNetworkObject = newObject.GetComponent<NetworkObject>();
        newObjectNetworkObject.Spawn(true);
        Destroy(objToSpawn);
    }

    
    public void SpawnPowerUpItem(Vector3 position)
    {
        GameObject objToSpawn = new GameObject("SpawnedPowerUp");
        Transform newObject = Instantiate(powerUpObject[Random.Range(0, powerUpObject.Length)].transform, objToSpawn.transform);
        newObject.localPosition += position + new Vector3(0, 20, 0);

        int randomNr = Random.Range(1, 10);
        if (randomNr > 5)
        {
            Vector3 moveDir = Vector3.zero;
            if (randomNr == 6 || randomNr == 7)
            {
                moveDir = new Vector3(0, Random.Range(1000, 1500), 0);
            }
            else
            {
                moveDir = new Vector3(0, -Random.Range(1000, 1500), 0);
            }

            float rotateSpeed = 75;
            objToSpawn.transform.Rotate(moveDir, rotateSpeed);
        }

        NetworkObject newObjectNetworkObject = newObject.GetComponent<NetworkObject>();
        newObjectNetworkObject.Spawn(true);
        Destroy(objToSpawn);
    }
}