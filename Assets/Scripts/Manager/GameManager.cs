using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [HideInInspector] public int scoreP1;
    [HideInInspector] public int scoreP2;
    [HideInInspector] public float launchRemainingTime =3;
    [HideInInspector] public int secondsToLaunch = 3;

    [HideInInspector] public GameStatus gameStatus;

    public float winPoints = 12;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GM NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        gameStatus = GameStatus.gameRunning;
    }

    public enum Player
    {
        p1,
        p2
    }

    public enum GameStatus
    {
        gameRunning,
        gamePaused
    }

    private void Update()
    {
        HandleTimeScale();
        CheckForScene();
        UpdateLaunchTimer();
        CheckForGameEnd();
    }

    private void HandleTimeScale()
    {
        Time.timeScale = (gameStatus == GameStatus.gamePaused) ? 0 : 1;
    }

    private void CheckForScene()
    {
        if (SceneManager.GetActiveScene().name != "SampleScene") return;
    }

    private void UpdateLaunchTimer()
    {
        if (secondsToLaunch > 0)
        {
            launchRemainingTime -= Time.deltaTime / 1.5f;
            secondsToLaunch = Mathf.Max(0, (int)(launchRemainingTime % 60));
        }
        else
        {
            Ball ball = FindObjectOfType<Ball>();
            if (ball != null)
            {
                ball.Launch();
            }
        }
    }

    private void CheckForGameEnd()
    {
        if (scoreP1 >= winPoints || scoreP2 >= winPoints)
        {
            EndGame();
        }
    }


    public void AssignPoint(Player player)
    {
        if (player == Player.p1)
        {
            scoreP1++;
        }
        else if (player == Player.p2)
        {
            scoreP2++;
        }
    }

    public void EndGame()
    {
        gameStatus = GameStatus.gamePaused;
        UIManager.Instance.cooldown.SetActive(false);
        string winner = "";
        if (scoreP1 > scoreP2)
        {
            winner = "P1";
        }
        else if (scoreP2 > scoreP1)
        {
            winner = "P2";
        }

        UIManager.Instance.WinnerScreen(winner);
    }

    public void TPMainMenu()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
