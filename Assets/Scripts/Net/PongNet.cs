using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
public class PongNet : NetworkManager
{
    [SerializeField] GameObject paddle;                   
    [SerializeField] GameObject ball;                           
    public List<PlayerNet> Players { get; } = new List<PlayerNet>();  

    public static event Action OnPConnected;                     

    private bool gameInProg = false;                            
    public bool IsGameInProg { get { return gameInProg; } }

    #region Server
    public override void AddP_Server(NetworkConnectionToClient conn)
    {
        base.AddP_Server(conn);
        PlayerNet player = conn.identity.GetComponent<PlayerNet>();
        Players.Add(player);
        player.SetPName_Server("P "+Players.Count);
    }

    public override void ChangeScene_Server(string newSceneName)
    {
        if (IsInSampleScene())
        {
            SpawnPlayers();
            SpawnBall();
        }
    }

    private bool IsInSampleScene()
    {
        return SceneManager.GetActiveScene().name == "SampleScene";
    }

    private void SpawnPlayers()
    {
        foreach (PlayerNet currentPlayer in Players)
        {
            GameObject playerPaddleInstance = InstantiatePaddle(currentPlayer);
            NetworkServer.Spawn(playerPaddleInstance, currentPlayer.connectionToClient);
        }
    }

    private GameObject InstantiatePaddle(PlayerNet player)
    {
        Transform startPosition = GetStartPosition();
        return Instantiate(paddle, startPosition.position, Quaternion.identity);
    }

    private void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ball);
        NetworkServer.Spawn(ballInstance);
    }


    public override void Connect_Server(NetworkConnectionToClient conn)
    {
        if (!gameInProg) return;
        conn.Disconnect();
    }

    public override void Disconnect_Server(NetworkConnectionToClient conn)
    {
        PlayerNet player = conn.identity.GetComponent<PlayerNet>();
        Players.Remove(player);
        base.Disconnect_Server(conn);
    }

    public void StartGame_Client()
    {
        if (Players.Count != 2) return;
        gameInProg = true;
        ServerChangeScene("SampleScene");
    }

    public override void StopGame_Client()
    {
        Players.Clear();
        gameInProg = false;
    }

    
    #endregion

    #region Client
    public override void OnConnect_Client()
    {
        base.OnConnect_Client();
        OnPConnected?.Invoke();
    }

    public override void OnStop_Client()
    {
        Players.Clear();
    }
    #endregion

}
