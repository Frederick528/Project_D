using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCtrl : MonoBehaviour
{
    public int hp;
    public Slider hpBar;
    public List<GameObject> effect;
    public Vector2 playerPos, mousePos, movePos, playerMovePos;
    float dis;
    float bounceAngle;
    public float angle;
    bool action = true;
    List<Vector2> posList = new List<Vector2>();
    Vector2 beforePos;
    GameObject arrow, bullet;
    public float originSpeed;
    private Coroutine moveCor;
    public float speed, disVal, maxArrowX, maxArrowY;
    Rigidbody2D rb2;

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playerPos = transform.position;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
        dis = Vector2.Distance(playerPos, mousePos);
        if (dis > disVal)
            dis = disVal;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb2.velocity = Vector2.zero;
            originSpeed = 0;
            StartCoroutine(TimeReversal());

        }
        if (action == true)
        {
            playerMove();
            fireBullet();
        }
        HPBar();
    }

    void HPBar()
    {
        hpBar.value = hp / 100f;
    }

    void playerMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 0.25f;
            arrow = GameManager.Instance.poolManager.Get(0);

        }
        if (Input.GetMouseButton(1))
        {
            arrow.transform.localScale = new Vector2(0.05f + dis * (maxArrowX / disVal), 0.1f+ dis * (maxArrowY / disVal));
            arrow.transform.position = new Vector2(playerPos.x + 2 * (arrow.transform.localScale.y * Mathf.Cos(angle)), playerPos.y + 2 * (arrow.transform.localScale.y * Mathf.Sin(angle)));
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
        }
        if (Input.GetMouseButtonUp(1))
        {
            bounceAngle = angle;
            arrow.SetActive(false);
            movePos.x = playerPos.x + dis * Mathf.Cos(angle);
            movePos.y = playerPos.y + dis * Mathf.Sin(angle);
            if (posList.Count < 5)
                posList.Add(playerPos);
            else if (posList.Count >= 5)
            {
                posList.Remove(posList[0]);
                posList.Add(playerPos);
            }
            Time.timeScale = 1;
            rb2.velocity = Vector2.zero;
            playerMovePos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            if (moveCor != null)
            {
                StopCoroutine(moveCor);
                moveCor = null;
            }
            moveCor = StartCoroutine(MoveCor());
        }
        //transform.position = Vector2.Lerp(playerPos, movePos, speed * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    speed = 100000000f;
        //}
    }

    private IEnumerator MoveCor()
    {
        originSpeed = speed * dis;
        while(originSpeed > 0)
        {
            
            transform.Translate(playerMovePos * originSpeed * Time.deltaTime);
            originSpeed -= originSpeed * Time.deltaTime;
            yield return null;
            
        }
        moveCor = null;

    }

    void fireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bullet = GameManager.Instance.poolManager.Get(1);
            bullet.transform.position = playerPos;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

        }
    }

    IEnumerator TimeReversal()
    {
        //yield return new WaitForSeconds(0f);
        action = false;
        arrow.SetActive(false);
        Time.timeScale = 1;
        Instantiate(effect[0], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        for (int i = posList.Count - 1; i >= 0; i--)
        {
            print(posList[i]);
            print(playerPos);
            transform.position = posList[i];
            Instantiate(effect[1], transform.position, Quaternion.identity);

            yield return new WaitForSeconds(0.3f);
        }
        movePos = playerPos;
        ParticleSystem endTimeReversal = Instantiate(effect[0], transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        endTimeReversal.gameObject.transform.localScale = Vector3.one * 3;
        action = true;
        posList.Clear();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            print("충돌");
            playerMovePos = -playerMovePos;
            if (angle > 0)
            {
                bounceAngle = angle - Mathf.PI;

            }
            if (angle < 0)
            {
                bounceAngle = angle + Mathf.PI;

            }

        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.contacts[0].point[0] > playerPos.x + 0.1 || collision.contacts[0].point[0] < playerPos.x - 0.1)
            {
                playerMovePos = new Vector2(-Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));

                if (bounceAngle > Mathf.PI / 2)
                    bounceAngle = Mathf.PI - bounceAngle;
                else if (bounceAngle < Mathf.PI / 2 || bounceAngle > 0)
                    bounceAngle = Mathf.PI / 2 + bounceAngle;
                else if (bounceAngle > -(Mathf.PI / 2) || bounceAngle < 0)
                    bounceAngle = -(Mathf .PI / 2) + bounceAngle;
                else if (bounceAngle < -(Mathf.PI / 2))
                    bounceAngle = Mathf.PI + bounceAngle;
            }
            if (collision.contacts[0].point[1] > playerPos.y + 0.1 || collision.contacts[0].point[1] < playerPos.y - 0.1)
            {
                playerMovePos = new Vector2(Mathf.Cos(bounceAngle), -Mathf.Sin(bounceAngle));
                if (bounceAngle > Mathf.PI / 2)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle < Mathf.PI / 2 || bounceAngle > 0)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle > -(Mathf.PI / 2) || bounceAngle < 0)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle < -(Mathf.PI / 2))
                    bounceAngle = -bounceAngle;
            }
            //angle = Mathf.Atan2(collision.contacts[0].point[1] - playerPos.y, collision.contacts[0].point[0] - playerPos.x); 쓸 수 없음~
        }
        //movePos = transform.position;
        action = true;
    }
}