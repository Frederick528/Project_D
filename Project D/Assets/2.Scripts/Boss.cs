using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    bool isMove = false;
    public GameObject target;
    public GameObject[] hands;
    int nextPattern = 0;
    int beforePattern;
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
        StartCoroutine(NextPattern());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove)
            return;
        hands[0].transform.position = Vector2.Lerp(hands[0].transform.position, target.transform.position, Time.deltaTime);
        hands[1].transform.position = Vector2.Lerp(hands[1].transform.position, target.transform.position, Time.deltaTime);
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
        isMove = true;
        yield return new WaitForSeconds(1f);
        BossHand.lookAtTarget = false;
        isMove = false;
        yield return new WaitForSeconds(2f);
        BossHand.lookAtTarget = true;
        yield return null;
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
}
