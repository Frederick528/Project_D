using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 오브젝트와 충돌이 충돌할 경우, 총알 제거와 함께 타격 이펙트 소환
        gameObject.SetActive(false);
        Instantiate(effect, collision.contacts[0].point, Quaternion.identity);
    }


}
