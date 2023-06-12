using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private UIScript ui;
    public GameObject btn;
    public TextMeshProUGUI[] upgradeLabels;
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
        SetText();
        if (Input.GetKeyDown(KeyCode.Space))
            upgrade = false;
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

    void SetText()
    {
        switch (PlayerCtrl.bulletLevel)
        {
            case 0:
                upgradeLabels[0].text = "Bullet + 1.";
                break;
            case 1:
                upgradeLabels[0].text = "The reload speed of the bullet is increased.";
                break;
            case 2:
                upgradeLabels[0].text = "Bullet + 1.";
                break;
            case 3:
                upgradeLabels[0].text = "Fire the bullet twice.";
                break;
            case 4: default:
                upgradeLabels[0].text = "It's Max level.";
                break;

        }

        upgradeLabels[1].text = "Recover Health 15\n Maximum Health + 5";
        upgradeLabels[2].text = "Bullet Damage + 1";
    }

    public void OnClickExit()
    {
        PlayerMove();
    }

    public void OnClickBulletUpgrade()
    {
        if (PlayerCtrl.bulletLevel >= 4)
            return;
        PlayerCtrl.bulletLevel += 1;
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
        HpCtrl.BulletDamage += 1;
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
