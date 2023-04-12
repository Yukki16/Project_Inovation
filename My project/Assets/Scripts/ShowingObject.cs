using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShowingObject : NetworkBehaviour
{
    public bool showOnServer = false;
    public bool showOnClient = false;

    private void Start()
    {
        if (!this.GetComponent<NetworkObject>().IsSpawned)
        {
            this.GetComponent<NetworkObject>().Spawn();
        }
        if (IsServer)
        {
            if (!showOnServer)
            {
                this.gameObject.SetActive(false);
            }else
            {
                this.gameObject.SetActive(true);
            }
        }
        if(IsClient)
        {
            if (!showOnClient)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
        /*if(!showOnServer && IsServer)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        if(!showOnClient && (IsClient || IsLocalPlayer))
        {
            this.gameObject.SetActive(false);
        }*/
    }
}
