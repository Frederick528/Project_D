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
                goalText.text = "Eliminate 5 enemies (" + Enemy.enemyCount + " / 5)";
                break;
            case 2: case 3: default:
                goalText.text = "";
                break;
        }
    }
}
