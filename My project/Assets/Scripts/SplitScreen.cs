using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SplitScreen : MonoBehaviour
{
    NetworkManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        switch(manager.numPlayers)
        {
            default: Camera cam1 = new Camera();
                
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
