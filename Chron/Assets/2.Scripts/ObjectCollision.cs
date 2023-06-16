using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    Rigidbody2D rb2;
    Vector2 ObjectMovePos;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        ObjectMovePos = GameManager.Instance.playerCtrl.playerMovePos;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌할 경우, 플레이어의 원래 이동 방향으로 서서히 이동
        if (collision.gameObject.CompareTag("Player"))
        {
            rb2.AddForce(ObjectMovePos * 1 / 2, ForceMode2D.Impulse);
        }
    }
}
