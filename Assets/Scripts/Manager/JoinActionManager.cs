using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class JoinActionManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField address;   
    public void Join()
    {
        string address = this.address.text;
        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();
    }

    public void HostLobby()
    {
        NetworkManager.singleton.StartHost();
    }
}
