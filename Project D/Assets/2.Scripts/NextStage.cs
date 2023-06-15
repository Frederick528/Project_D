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
                if (Enemy.enemyCount >= 5)
                {
                    open.SetActive(true);
                }
                break;
            case 2:
                open.SetActive(true);
                break;

        }
    }
}
