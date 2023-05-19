using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //float cooldown;
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
            //cooldown = 1f;
            //StartCoroutine("BulletCooldown");
        }
        //if (cooldown <= 0)
        //    gameObject.SetActive(false);
    }
    //IEnumerator BulletCooldown()
    //{

    //    while (cooldown > 0)
    //    {
    //        cooldown -= Time.deltaTime;
    //        yield return null;
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
        Instantiate(effect, collision.contacts[0].point, Quaternion.identity);
    }

}
