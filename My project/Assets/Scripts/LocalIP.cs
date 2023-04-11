using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class LocalIP : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            GetLocalIPAddress(),  // The IP address is a string
            (ushort)7777 // The port number is an unsigned short
              );
    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                //hintText.text = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");

    }
}