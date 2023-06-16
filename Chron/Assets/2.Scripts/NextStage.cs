using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    [SerializeField] GameObject open;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                if (Enemy.enemyCount >= 1)
                    open.SetActive(true);
                break;

            case 2:
                if (Enemy.enemyCount >= 2)
                    open.SetActive(true);
                break;

            case 3:
            case 5:
            case 8:
            case 10:
                open.SetActive(true);
                break;

            case 4:
                if (Enemy.enemyCount >= 5)
                    open.SetActive(true);
                break;

            case 6:
                if (Enemy.enemyCount >= 7)
                    open.SetActive(true);
                break;

            case 7:
                if (Enemy.enemyCount >= 10)
                    open.SetActive(true);
                break;

            case 9:
                if (Enemy.enemyCount >= 15)
                    open.SetActive(true);
                break;

        }
    }
}
