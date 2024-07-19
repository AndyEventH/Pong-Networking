using UnityEngine;
using Mirror;
using System;
public class Ball : NetworkBehaviour
{
    [SerializeField] int force = 42;
    public bool launched;

    public event Action OnP1ScoreUpdate;
    public event Action OnP2ScoreUpdate;
    public event Action OnGoal;

    float multiplier = 10;

    Rigidbody2D rb;
    InputManager IM;

    public override void Start_Client()
    {
        OnP1ScoreUpdate += P1UpdateScoreClient;
        OnP2ScoreUpdate += P2UpdateScoreClient;
        OnGoal += ClientGoal;
    }
    public override void Stop_Client()
    {
        OnP1ScoreUpdate -= P1UpdateScoreClient;
        OnP2ScoreUpdate -= P2UpdateScoreClient;
        OnGoal -= ClientGoal;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        IM = FindObjectOfType<InputManager>();
    }
    private void Update()
    {
        if (!isServer) return;

        if (transform.position.y > IM.boundaryHeight || transform.position.y < -IM.boundaryHeight - 2)
            OnGoal?.Invoke();

    }
    public void Launch()
    {
        if (launched) return;
        float horiz = Mathf.Sign(UnityEngine.Random.Range(-1f, 1f));
        float vert = UnityEngine.Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(horiz, vert) * force * multiplier);
        launched = true;
    }

    public void ResetBall()
    {
        Vector2 newPosition = new Vector2(0.04f, -0.83f);
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("P1"))
        {
            if (isServer)
                OnP2ScoreUpdate?.Invoke();
        }
        if (collision.transform.CompareTag("P2"))
        {
            if (isServer)
                OnP1ScoreUpdate?.Invoke();
        }
    }

    

    [ClientRpc]
    void P1UpdateScoreClient()
    {
        GameManager.Instance.AssignPoint(GameManager.Player.p1);
        ResetAll();
    }
    [ClientRpc]
    void P2UpdateScoreClient()
    {
        GameManager.Instance.AssignPoint(GameManager.Player.p2);
        ResetAll();
    }
    [ClientRpc]
    void ClientGoal()
    {
        ResetAll();
    }

    void ResetAll()
    {
        ResetBall();
        GameManager.Instance.secondsToLaunch = 3;
        GameManager.Instance.launchRemainingTime = 3;
        launched = false;
        UIManager.Instance.cooldown.SetActive(true);
    }
}