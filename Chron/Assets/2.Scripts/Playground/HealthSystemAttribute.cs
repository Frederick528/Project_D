using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Playground/Attributes/Health System")]
public class HealthSystemAttribute : MonoBehaviour
{
    // public static int health = PlayerCtrl.maxHealth;

    private UIScript ui;
    public Slider hpBar;
    // Will be set to 0 or 1 depending on how the GameObject is tagged
    // it's -1 if the object is not a player
    private int playerNumber;



    private void Start()
    {
        // Find the UI in the scene and store a reference for later use
        ui = GameObject.FindObjectOfType<UIScript>();

        // Set the player number based on the GameObject tag
        switch (gameObject.tag)
        {
            case "Player":
                playerNumber = 0;
                break;
            case "Enemy":
                playerNumber = 1;
                break;
            case "Boss":
                playerNumber = 2;
                break;
            default:
                playerNumber = -1;
                break;
        }

        // Notify the UI so it will show the right initial amount
        if (ui != null
            && playerNumber != -1)
        {
            ui.SetHealth(PlayerCtrl.health, playerNumber);
        }

        //maxHealth = health; //note down the maximum health to avoid going over it when the player gets healed
    }
    void Update()
    {
        HPBar();
    }

    void HPBar()
    {
        if (playerNumber != 0)
            return;
        hpBar.value = (float)PlayerCtrl.health / PlayerCtrl.maxHealth;
    }

    // changes the energy from the player
    // also notifies the UI (if present)
    public void ModifyHealth(int amount)
    {
        //avoid going over the maximum health by forcin
        if (PlayerCtrl.health + amount > PlayerCtrl.maxHealth)
        {
            amount = PlayerCtrl.maxHealth - PlayerCtrl.health;
        }

        PlayerCtrl.health += amount;

        // Notify the UI so it will change the number in the corner
        if (ui != null
            && playerNumber != -1)
        {
            ui.ChangeHealth(amount, playerNumber);
        }

        // 주석한 이유: 다른 오브젝트 제거 스크립트도 같이 사용하기 때문에 오류가 생김.
        //DEAD
        //if (PlayerCtrl.health <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

}
