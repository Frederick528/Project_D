using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerCtrl : MonoBehaviour
{
    public Vector2 playerPos, mousePos, movePos;
    float dis;
    float angle;
    GameObject arrow, bullet;
    public float speed, disVal, maxArrow;

    void Start()
    {
    }

    void Update()
    {
        playerPos = transform.position;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        if (Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 0.25f;
            arrow = GameManager.Instance.poolManager.Get(0);

        }
        if (Input.GetMouseButton(1))
        {
            dis = Vector2.Distance(playerPos, mousePos);
            if (dis > disVal)
                dis = disVal;
            arrow.transform.localScale = new Vector2(0.4f, dis * (maxArrow / disVal));
            angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
            arrow.transform.position = new Vector2(playerPos.x + 2* (arrow.transform.localScale.y * Mathf.Cos(angle)), playerPos.y + 2 * (arrow.transform.localScale.y * Mathf.Sin(angle)));
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
        }
        if (Input.GetMouseButtonUp(1))
        {
            movePos = new Vector2(mousePos.x, mousePos.y);
            dis = Vector2.Distance(playerPos, movePos);
            arrow.SetActive(false);
            if (dis > disVal)
            {
                angle = Mathf.Atan2(movePos.y - playerPos.y, movePos.x - playerPos.x);
                movePos.x = playerPos.x + disVal * Mathf.Cos(angle);
                movePos.y = playerPos.y + disVal * Mathf.Sin(angle);
            }
            Time.timeScale = 1;
        }
        transform.position = Vector2.Lerp(playerPos, movePos, speed * Time.deltaTime);


        if (Input.GetMouseButtonDown(0))
        {
            bullet = GameManager.Instance.poolManager.Get(1);
            angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
            bullet.transform.position = playerPos;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

        }
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    speed = 100000000f;
        //}
    }

}
