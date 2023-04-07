using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class DemoManager : MonoBehaviour
{
    TcpListener listener;
    List<TcpClient> clients;

    public PlayerMove[] players;

    // Start is called before the first frame update
    void Start()
    {
        listener = new TcpListener(51000);
        listener.Start();

        clients = new List<TcpClient>();

        // TODO:
        //  clean up code (of course) - Tcp tools, streamreader
        //
        // Separate GameManager vs NetworkManager
        // GameManager:
        //   asks for the number of players when scene starts
        //   spawns player prefabs for those (including cameras?)
        //   sets the camera rendering range (splitscreen is different for 2p vs 4p)
        //   either reads input directly from clients and translates it into player movement OR subscribes to networkManager packetReceived events
        //
        // NetworkManager:
        //    has a listener
        //    does the streamreading correctly
        //    maybe: fires OnPacketReceived events?
        //    handles disconnections nicely
        //    Important: stays throughout scene changes! (DontDestroyOnLoad!)
    }

    // Update is called once per frame
    void Update()
    {
        if (listener.Pending())
        {
            clients.Add(listener.AcceptTcpClient());
        }

        for (int i=0;i<clients.Count;i++)
        {
            if (clients[i].Available>0)
            {
                int len = clients[i].Available;
                // This is ugly by the way! - check Networking course
                NetworkStream stream = clients[i].GetStream();

                byte[] header = new byte[4];
                stream.Read(header, 0, 4); // for now! - this is the length


                byte[] packet=new byte[len - 4]; // ugly!
                stream.Read(packet, 0, len - 4); // for now!

                string input = Encoding.UTF8.GetString(packet);

                Debug.Log("Got a packet: " + input);

                // more ugly code:
                if (float.TryParse(input, out float value))
                {
                    players[i].speed = value; // will be out of range error if too many clients
                }
            }
        }

    }
}
