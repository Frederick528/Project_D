using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCtrl : MonoBehaviour
{
    public Vector2 playerPos, mousePos, movePos;
    float dis;
    float angle;
    bool action = true;
    List<Vector2> posList = new List<Vector2>();
    Vector2 beforePos;
    GameObject arrow, bullet;
    public float speed, disVal, maxArrowX, maxArrowY;

    void Start()
    {
    }

    void Update()
    {
        playerPos = transform.position;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
        dis = Vector2.Distance(playerPos, mousePos);


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(TimeReversal());
            
        }
        if (action == true)
        {
            playerMove();
            fireBullet();
        }


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
            if (dis > disVal)
                dis = disVal;
            arrow.transform.localScale = new Vector2(0.05f + dis * (maxArrowX / disVal), 0.1f+ dis * (maxArrowY / disVal));
            arrow.transform.position = new Vector2(playerPos.x + 2 * (arrow.transform.localScale.y * Mathf.Cos(angle)), playerPos.y + 2 * (arrow.transform.localScale.y * Mathf.Sin(angle)));
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
        }
        if (Input.GetMouseButtonUp(1))
        {
            movePos = new Vector2(mousePos.x, mousePos.y);
            arrow.SetActive(false);
            if (dis > disVal)
            {
                movePos.x = playerPos.x + disVal * Mathf.Cos(angle);
                movePos.y = playerPos.y + disVal * Mathf.Sin(angle);
            }
            if (posList.Count < 5)
                posList.Add(playerPos);
            else if (posList.Count >= 5)
            {
                posList.Remove(posList[0]);
                posList.Add(playerPos);
            }
            Time.timeScale = 1;
        }
        transform.position = Vector2.Lerp(playerPos, movePos, speed * Time.deltaTime);

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    speed = 100000000f;
        //}
    }
    void fireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bullet = GameManager.Instance.poolManager.Get(1);
            angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
            bullet.transform.position = playerPos;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

        }
    }

    IEnumerator TimeReversal()
    {
        action = false;
        for (int i = posList.Count - 1; i >= 0; i--)
        {
            print(posList[i]);
            print(playerPos);
            transform.position = posList[i];
            yield return new WaitForSeconds(0.5f);
        }
        movePos = playerPos;
        action = true;
        posList.Clear();
    }
}