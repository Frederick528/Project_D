using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SetUI : MonoBehaviour
{
    public TMP_Text goalText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (UIScript.gameOver == true)
            return;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                goalText.text = "Eliminate 1 enemies (" + Enemy.enemyCount + " / 1)\nAttack with the left click!";
                break;
            case 2:
                goalText.text = "Eliminate 2 enemies (" + Enemy.enemyCount + " / 2)\nTry using the skill by pressing the shift key! (Move to use)";
                break;
            case 3:
                goalText.text = "Upgrade!";
                break;
            case 4:
                goalText.text = "Eliminate 5 enemies (" + Enemy.enemyCount + " / 5)";
                break;
            case 5:
                goalText.text = "Watch out for thorns!";
                break;
            case 6:
                goalText.text = "Eliminate 7 enemies (" + Enemy.enemyCount + " / 7)";
                break;
            case 7:
                goalText.text = "Eliminate 10 enemies (" + Enemy.enemyCount + " / 10)";
                break;
            case 8:
            default:
                goalText.text = "";
                break;
            case 9:
                goalText.text = "Eliminate 15 enemies (" + Enemy.enemyCount + " / 15)";
                break;
            case 10:
                goalText.text = "Umm...";
                break;
        }
    }
}
