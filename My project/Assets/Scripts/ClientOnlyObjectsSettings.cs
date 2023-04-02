using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientOnlyObjectsSettings : NetworkBehaviour
{
    private void Start()
    {
        if(isServer)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
    
}
