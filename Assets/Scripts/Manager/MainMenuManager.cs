using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] playersText = new TMP_Text[2];              
    [SerializeField] private GameObject lobby;                           

    private void OnEnable()
    {
        PongNet.OnPConnected += OnConnect_Client;
        PlayerNet.UpdatePName += OnInfoUpdate_Client;
    }
    private void OnDisable()
    {
        PongNet.OnPConnected -= OnConnect_Client;
        PlayerNet.UpdatePName -= OnInfoUpdate_Client;
    }

    void OnConnect_Client()
    {
        lobby.SetActive(true);
    }

    public void LeaveLobby()
    {
        if (NetworkServer.active)
        {
            if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
                SceneManager.LoadScene("Menu");
                return;
            }
        }

        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("Menu");
    }


    public void StartGame_Client()
    {
        NetworkClient.connection.identity.GetComponent<PlayerNet>().StartGame_Command();
    }

    private void OnInfoUpdate_Client()
    {
        List<PlayerNet> players = ((PongNet)NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++)
        {
            this.playersText[i].text = players[i].PName;
        }
    }

}
