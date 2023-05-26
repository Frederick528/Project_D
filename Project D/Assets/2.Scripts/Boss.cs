using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public enum Pattern
    {
        Lazer,
        SPAWN,
        SWING,
        SHOOT,
        BURST
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BossPattern()
    {
        yield return null;

        int randomInt = Random.Range(0, 6);
        switch (randomInt)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;

        }
    }

    IEnumerator LazerPattern()
    {
        yield return null;
    }
}
