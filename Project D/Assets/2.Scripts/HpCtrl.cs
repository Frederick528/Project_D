using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpCtrl : MonoBehaviour
{
    public static int BulletDamage = 1;
    int health;
    public int maxHealth;
    public Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = (float)health / maxHealth;
        if (health <= 0)
            gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
            health -= BulletDamage;
        if (collision.gameObject.CompareTag("Effect"))
            health -= 1;
    }
}
