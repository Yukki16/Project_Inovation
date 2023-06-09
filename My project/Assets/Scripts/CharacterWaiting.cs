using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static GeneralGameManager;

public class CharacterWaiting : MonoBehaviour
{
    public static CharacterWaiting Instance { get; private set; }
    [SerializeField] private GameObject char1;
    [SerializeField] private GameObject char2;
    [SerializeField] private GameObject char3;
    [SerializeField] private GameObject char4;
    [SerializeField] private GameObject noPlayersText;
    [SerializeField] private TextMeshProUGUI maxAmountPlayers;
    [SerializeField] private TextMeshProUGUI currentAmountPlayers;
    [SerializeField] private TextMeshProUGUI ipAddress;

    private Dictionary<GameObject, GeneralGameManager.CharacterColors> characterAssignedClient;

    private void Awake()
    {
        Instance = this;
        characterAssignedClient = new Dictionary<GameObject, GeneralGameManager.CharacterColors>
        {
            { char1, GeneralGameManager.CharacterColors.BLACK },
            { char2, GeneralGameManager.CharacterColors.ORANGE },
            { char3, GeneralGameManager.CharacterColors.PURPLE },
            { char4, GeneralGameManager.CharacterColors.WHITE }
        };
        currentAmountPlayers.text = "0";
        maxAmountPlayers.text = "/ " + characterAssignedClient.Count.ToString();
    }

    private void Start()
    {
        foreach (var charGameObject in characterAssignedClient.Keys)
        {
            charGameObject.SetActive(false);
        }
        LoadIp();
    }

    private void LoadIp()
    {
        //get ip address
        Process process = new Process();
        process.StartInfo.FileName = "ipconfig";
        process.StartInfo.Arguments = "/all";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        // Extract the IPv4 address that is inside the wireless Lan adapter
        Regex regex = new Regex(@"Wireless LAN adapter (?:Wi(?:-)?Fi|WLAN):[\s\S]*?Connection-specific DNS Suffix\s*\. :\s*(?<dnsSuffix>[^\s]+)?[\s\S]*?IPv4 Address[.\s\S]*?:\s*(?<ipAddress>\d+\.\d+\.\d+\.\d+)[\s\S]*?Default Gateway[.\s\S]*?:\s*(?<defaultGateway>\d+\.\d+\.\d+\.\d+)");
        Match match = regex.Match(output);

        if (match.Success)
        {
            // Parse the IPv4 address
            string ipAddressString = match.Groups["ipAddress"].Value;
            ipAddress.text = ipAddressString;
        }
    }

    public void UpdateCharacterWaitingView()
    {
        Dictionary<CharacterColors, TcpClient> clientsWithTheirCharacterColor = GeneralGameManager.Instance.GetClientsWithTheirCharacterColor();
        foreach (var character in characterAssignedClient.Keys)
        {
            foreach (var color in clientsWithTheirCharacterColor.Keys)
            {
                if (characterAssignedClient[character] == color)
                {
                    if (clientsWithTheirCharacterColor[color] != null)
                    {
                        character.SetActive(true);
                    }
                    else
                    {
                        character.SetActive(false);
                    }
                }
            }       
        }  
        CheckIfAnyPlayerLeft();
    }

    //public void RemoveCharacter(TcpClient client) 
    //{
    //    GameObject charToDeactivate = null;
    //    foreach (var character in characterAssignedClient.Keys)
    //    {
    //        if (characterAssignedClient[character] == client)
    //        {
    //            charToDeactivate = character;
    //            break;
    //        }
    //    }

    //    if (charToDeactivate)
    //    {
    //        characterAssignedClient[charToDeactivate] = client;
    //        charToDeactivate.SetActive(true);
    //        CheckIfAnyPlayerLeft();
    //    }
    //}

    private void CheckIfAnyPlayerLeft()
    {
        if (NetworkManager.Instance.GetAmountOfConnectedPlayers() > 0)
        {
            noPlayersText.SetActive(false);
        }
        else
        {
            noPlayersText.SetActive(true);
        }
        UpdateAmountOfCurrentPlayers();
    }

    private void UpdateAmountOfCurrentPlayers()
    {
        currentAmountPlayers.text = $"{NetworkManager.Instance.GetAmountOfConnectedPlayers()}";
    }
}
