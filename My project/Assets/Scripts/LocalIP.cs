/*using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class LocalIP : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI TEXT;
    private void Start()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            GetLocalIPAddress(),  // The IP address is a string
            (ushort)7777 // The port number is an unsigned short
              );
        TEXT.text = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address;
    }

    public string GetLocalIPAddress()
    {
        UnicastIPAddressInformation mostSuitableIp = null;

        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var network in networkInterfaces)
        {
            if (network.OperationalStatus != OperationalStatus.Up)
                continue;

            var properties = network.GetIPProperties();

            if (properties.GatewayAddresses.Count == 0)
                continue;

            foreach (var address in properties.UnicastAddresses)
            {
                if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                    continue;

                if (IPAddress.IsLoopback(address.Address))
                    continue;

                if (!address.IsDnsEligible)
                {
                    if (mostSuitableIp == null)
                        mostSuitableIp = address;
                    continue;
                }

                // The best IP is the IP got from DHCP server
                if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                {
                    if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                        mostSuitableIp = address;
                    continue;
                }

                return address.Address.ToString();
            }
        }

        return mostSuitableIp != null
            ? mostSuitableIp.Address.ToString()
            : "";
    }
    *//*public string GetLocalIPAddress()
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

    }*//*
}*/