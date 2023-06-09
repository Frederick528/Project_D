using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private UIScript ui;
    public GameObject btn;
    public Slider hpBar;
    bool upgrade = false;
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindObjectOfType<UIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = (float)PlayerCtrl.health / PlayerCtrl.maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && upgrade == false)
        {
            Time.timeScale = 0;
            GameManager.Instance.playerCtrl.action = false;
            GameManager.Instance.playerCtrl.timeReversalCooldown = true;
            btn.SetActive(true);
        }
    }

    public void OnClickExit()
    {
        PlayerMove();
    }

    public void OnClickBulletUpgrade()
    {
        GameManager.Instance.playerCtrl.bulletLevel += 1;
        PlayerMove();
        upgrade = true;
    }

    public void OnClickHealthUpgrade()
    {
        PlayerCtrl.maxHealth += 5;
        if (PlayerCtrl.health + 15 <= PlayerCtrl.maxHealth)
        {
            PlayerCtrl.health += 15;
        }
        else
            PlayerCtrl.health = PlayerCtrl.maxHealth;
        if (ui != null)
        {
            ui.SetHealth(PlayerCtrl.health, 0);
        }

        PlayerMove();
        upgrade = true;
    }

    public void OnClickDamageUpgrade()
    {
        PlayerMove();
        upgrade = true;
    }

    void PlayerMove()
    {
        Time.timeScale = 1;
        GameManager.Instance.playerCtrl.action = true;
        GameManager.Instance.playerCtrl.timeReversalCooldown = false;
        btn.SetActive(false);
    }
}
