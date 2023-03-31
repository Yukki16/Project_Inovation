using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner: MonoBehaviour
{
    [SerializeField] private GameObject[] fallingObject;
    public void SpawnObject(Vector3 position)
    {
        Transform newObject = Instantiate(fallingObject[Random.Range(0,fallingObject.Length)].transform, position, transform.rotation);
        newObject.localPosition += new Vector3(0, 20, 0);
    }
}
