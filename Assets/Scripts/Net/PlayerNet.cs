using UnityEngine.SceneManagement;
using System;
using Mirror;
public class PlayerNet : NetworkBehaviour
{
    [SyncVar(hook = nameof(PNameUpdated_Client))]
    private string pName;                                  

    public string PName { get { return pName; } }

    public static event Action UpdatePName;             

    MainMenuManager mainMenu;

    private void Start()
    {
        mainMenu = FindObjectOfType<MainMenuManager>();

    }

    #region Server
    public override void Start_Server()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Command] 
    public void StartGame_Command()
    {
        ((PongNet)NetworkManager.singleton).StartGame_Client();
    }
    [Server] 
    public void SetPName_Server(string newName)
    {
        pName = newName;
    }
    #endregion


    #region Client
    public override void Start_Client()
    {
        if (NetworkServer.active) return;
        ((PongNet)NetworkManager.singleton).Players.Add(this);

        DontDestroyOnLoad(gameObject);
    }

    public override void Stop_Client()
    {
        UpdatePName?.Invoke();
        if (!isClientOnly) return;
        ((PongNet)NetworkManager.singleton).Players.Remove(this);

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            NetworkManager.singleton.StopHost();
            SceneManager.LoadScene("Menu");
        }
    }

    private void PNameUpdated_Client(string oldName, string newName)
    {
        UpdatePName?.Invoke();
    }
    #endregion
}
