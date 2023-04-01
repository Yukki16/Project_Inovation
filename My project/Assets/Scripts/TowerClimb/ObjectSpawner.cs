using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner: MonoBehaviour
{
    [SerializeField] private GameObject[] fallingObject;
    public void SpawnObject(Vector3 position)
    {
        GameObject objToSpawn = new GameObject("SpawnedFallingItem");
        Transform newObject = Instantiate(fallingObject[Random.Range(0,fallingObject.Length)].transform, objToSpawn.transform);
        newObject.localPosition += position + new Vector3(0, 30, 0);

        int randomNr = Random.Range(1, 10);
        if (randomNr > 5)
        {
            Vector3 moveDir = Vector3.zero;
            if (randomNr == 6 || randomNr == 7)
            {
                moveDir = new Vector3(0, Random.Range(1000,1500), 0);
            }
            else
            {
                moveDir = new Vector3(0, -Random.Range(1000, 1500), 0);
            }
            
            float rotateSpeed = 75;
            objToSpawn.transform.Rotate(moveDir, rotateSpeed);
        }
        
    }
}
