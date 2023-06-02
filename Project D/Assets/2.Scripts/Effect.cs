using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Bullet 태그를 가진 오브젝트는 이펙트와 충돌할 경우 사라짐(특수 능력(shift))
        if (collision.gameObject.CompareTag("Bullet"))
            collision.gameObject.SetActive(false);
    }
}
