using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    bool isLazer, isSpawn, isSwing, isShoot, isBurst = false;
    bool rightUp, rightDown, leftUp, leftDown = false;
    public GameObject target;
    public GameObject[] hands;
    int nextPattern = 0;
    int beforePattern;

    float randomRightAngle1, randomRightAngle2, randomLeftAngle1, randomLeftAngle2;

    // 아직 작동 안됨
    bool rightLook = true;
    bool leftLook = true;
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
        rightLook = hands[0].gameObject.GetComponent<BossHand>().enabled;
        leftLook = hands[1].gameObject.GetComponent<BossHand>().enabled;

        StartCoroutine(NextPattern());
    }

    // Update is called once per frame
    void Update()
    {
        IsLazer();
    }

    IEnumerator BossPattern()
    {
        yield return null;
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(LazerPattern());
                break;
            case 2:
                StartCoroutine(SpawnPattern());
                break;
            case 3:
                StartCoroutine(SwingPattern());
                break;
            case 4:
                StartCoroutine(ShootPattern());
                break;
            case 5:
                StartCoroutine(BurstPattern());
                break;

        }
    }

    IEnumerator NextPattern()
    {
        yield return new WaitForSeconds(2f);
        beforePattern = nextPattern;
        nextPattern = Random.Range(1, 2);
        if (beforePattern != nextPattern)
        {
            StartCoroutine(NextPattern());
        }
        else
            StartCoroutine(BossPattern());
    }
    IEnumerator LazerPattern()
    {
        isLazer = true;
        randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        rightUp = true;
        yield return new WaitForSeconds(1f);
        rightLook = false; rightUp = false;

        randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        leftDown = true;
        yield return new WaitForSeconds(1f);
        leftLook = false; leftDown = false; rightLook = true;

        randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        rightDown = true;
        yield return new WaitForSeconds(1f);
        rightLook = false; rightDown = false; leftLook = true;

        randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        leftUp = true;
        yield return new WaitForSeconds(1f);
        leftLook = false; leftUp = false; rightLook = true;
        StartCoroutine(NextPattern());
    }
    IEnumerator SpawnPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }
    IEnumerator SwingPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }
    IEnumerator ShootPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }
    IEnumerator BurstPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }

    void IsLazer()
    {   //우측 상단
        //float randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        //우측 하단
        //float randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        //좌측 상단
        //float randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        //좌측 하단
        //float randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        if (isLazer && rightUp)
        {
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(
                        target.transform.position.x + 3 * Mathf.Cos(randomRightAngle1),
                        target.transform.position.y + 3 * Mathf.Sin(randomRightAngle1)),
                    Time.deltaTime
                    );
        }

        if(isLazer && leftUp)
        {
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(
                        target.transform.position.x + 3 * Mathf.Cos(randomLeftAngle1),
                        target.transform.position.y + 3 * Mathf.Sin(randomLeftAngle1)),
                    Time.deltaTime
                    );
        }

        if (isLazer && rightDown)
        {
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(
                        target.transform.position.x + 3 * Mathf.Cos(randomRightAngle2),
                        target.transform.position.y + 3 * Mathf.Sin(randomRightAngle2)),
                    Time.deltaTime
                    );
        }

        if (isLazer && leftDown)
        {
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(
                        target.transform.position.x + 3 * Mathf.Cos(randomLeftAngle2),
                        target.transform.position.y + 3 * Mathf.Sin(randomLeftAngle2)),
                    Time.deltaTime
                    );
        }
    }
}
