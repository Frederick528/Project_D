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
        // Bullet �±׸� ���� ������Ʈ�� ����Ʈ�� �浹�� ��� �����(Ư�� �ɷ�(shift))
        if (collision.gameObject.CompareTag("Bullet"))
            collision.gameObject.SetActive(false);
    }
}
