using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    [SerializeField] GameObject winnerScreen;
    [SerializeField] TMP_Text winnerText;
    [SerializeField] public GameObject cooldown;
    [SerializeField] TMP_Text p1Score;
    [SerializeField] TMP_Text p2Score;
    [SerializeField] TMP_Text timerLaunch;

    int p1, p2;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIM NULL");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        p1 = GameManager.Instance.scoreP1;
        p2 = GameManager.Instance.scoreP2;

        p1Score.text = p1.ToString();
        p2Score.text = p2.ToString();

        timerLaunch.text = GameManager.Instance.secondsToLaunch.ToString();

        if (GameManager.Instance.secondsToLaunch == 0)
            cooldown.SetActive(false);
    }

    public void WinnerScreen(string winner)
    {
        winnerScreen.SetActive(true);
        winnerText.text = winner + " wins!";
    }
}
