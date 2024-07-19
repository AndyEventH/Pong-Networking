using UnityEngine;
using Mirror;
public class InputManager : NetworkBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] public float boundaryHeight = 4f;

    Vector2 oldPos;

    private void Update()
    {
        InputMovement();
        BoundariesCheck();
    }

    void BoundariesCheck()
    {
        if (!isOwned) return;

        if (transform.position.y >= boundaryHeight - 1)
        {
            transform.position = new Vector2(oldPos.x, Mathf.RoundToInt(oldPos.y));
        }
        if (transform.position.y <= -boundaryHeight)
        {
            transform.position = new Vector2(oldPos.x, Mathf.RoundToInt(oldPos.y));
        }
        oldPos = transform.position;
    }

    void InputMovement()
    {
        if (!isOwned) return;

        float vMove = Input.GetAxis("Vertical");

        transform.Translate(Vector2.up * Time.deltaTime * speed * vMove);
            
    }
}
