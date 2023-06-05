using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Boss : MonoBehaviour
{
    bool isLaser, isSwing, isShoot, isBurst, isSpawn = false;
    bool right, left, rightUp, rightDown, leftUp, leftDown, rightHandMove, leftHandMove = false;
    bool rightHandBack, leftHandBack = false;
    GameObject enemyBullet;
    public GameObject target;
    public GameObject[] hands;
    public GameObject[] laser;
    int nextPattern = 0;
    int beforePattern;

    Vector2 rightHandStartPos, leftHandStartPos;

    float randomRightAngle1, randomRightAngle2, randomLeftAngle1, randomLeftAngle2;

    BossHand rightLook;
    BossHand leftLook;
    public enum Pattern
    {
        Laser,
        SPAWN,
        SHOOT,
        BURST,
        SWING
    } 
    // Start is called before the first frame update
    void Start()
    {
        // 손 시작 위치 받아오기
        rightHandStartPos = hands[0].transform.position;
        leftHandStartPos = hands[1].transform.position;
        // 손이 플레이어를 바라보는 스크립트
        rightLook = hands[0].gameObject.GetComponent<BossHand>();
        leftLook = hands[1].gameObject.GetComponent<BossHand>();
        StartCoroutine(NextPattern());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            StopAllCoroutines();
            return;
        }
        HandBack();
        IsLaser();
        IsSwing();
        IsShoot();
        IsBurst();
    }

    IEnumerator BossPattern()
    {
        yield return null;
        switch (nextPattern)
        {
            case 1:
                StartCoroutine(LaserPattern());
                break;
            case 2:
                StartCoroutine(SwingPattern());
                break;
            case 3:
                StartCoroutine(ShootPattern());
                break;
            case 4:
                StartCoroutine(BurstPattern());
                break;
            case 5:
                StartCoroutine(SpawnPattern());
                break;

        }
    }

    IEnumerator NextPattern()
    {
        beforePattern = nextPattern;
        nextPattern = Random.Range(4, 5);
        if (beforePattern != nextPattern)
        {
            StartCoroutine(NextPattern());
        }
        else
        {
            yield return new WaitForSeconds(3f);
            StartCoroutine(BossPattern());
        }
    }
    IEnumerator LaserPattern()
    {
        // 레이저 실행
        isLaser = true;
        randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        // 오른손이 1초동안 우측 상단으로 이동
        rightUp = true;
        yield return new WaitForSeconds(1f);
        // 오른손 lookat 멈추고 오른손 이동 멈춤
        rightLook.enabled = false; rightUp = false;
        // 오른손 레이저라인 활성화
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5초 후 레이저라인 비활성화 및 레이저 활성화
        laser[2].SetActive(false);
        laser[0].SetActive(true);

        randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        // 왼손이 1초동안 좌측 하단으로 이동
        leftDown = true;
        yield return new WaitForSeconds(1f);
        // 왼손 lookat 멈추고 왼손 이동 멈춤 + 오른손 lookat 활성화
        leftLook.enabled = false; leftDown = false; rightLook.enabled = true;
        // 오른손 레이저 비활성화
        laser[0].SetActive(false);
        // 왼손 레이저라인 활성화
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5초 후 레이저라인 비활성화 및 레이저 활성화
        laser[3].SetActive(false);
        laser[1].SetActive(true);

        randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        // 오른손이 1초동안 우측 하단으로 이동
        rightDown = true;
        yield return new WaitForSeconds(1f);
        // 오른손 lookat 멈추고 오른손 이동 멈춤 + 왼손 lookat 활성화
        rightLook.enabled = false; rightDown = false; leftLook.enabled = true;
        // 왼손 레이저 비활성화
        laser[1].SetActive(false);
        // 오른손 레이저라인 활성화
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5초 후 레이저라인 비활성화 및 레이저 활성화
        laser[2].SetActive(false);
        laser[0].SetActive(true);

        randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        // 왼손이 1초동안 좌측 상단으로 이동
        leftUp = true;
        yield return new WaitForSeconds(1f);
        // 왼손 lookat 멈추고 왼손 이동 멈춤 + 오른손 lookat 활성화
        leftLook.enabled = false; leftUp = false; rightLook.enabled = true;
        // 오른손 레이저 비활성화
        laser[0].SetActive(false);
        // 왼손 레이저라인 활성화
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5초 후 레이저라인 비활성화 및 레이저 활성화
        laser[3].SetActive(false);
        laser[1].SetActive(true);

        // 1초동안 오른손 원래 자리로 돌아가기
        rightHandBack = true;
        yield return new WaitForSeconds(1f);
        // 왼손 레이저 비활성화
        laser[1].SetActive(false);
        // 왼손 lookat 멈춤
        leftLook.enabled = true;
        // 1초동안 왼손 원래 자리로 돌아가고 오른손 이동 멈춤
        leftHandBack = true; rightHandBack = false;
        yield return new WaitForSeconds(1f);
        // 왼손 이동 멈춤
        leftHandBack = false;
        isLaser = false;
        StartCoroutine(NextPattern());
    }
    IEnumerator SwingPattern()
    {
        isSwing = true; rightHandMove = true; leftHandMove = true;
        yield return new WaitForSeconds(2f);
        rightHandMove = false; leftHandMove = false;
        leftLook.enabled = false; rightLook.enabled=false;
        hands[0].transform.eulerAngles = new Vector3(0, 0, -90);
        hands[1].transform.eulerAngles = new Vector3(0, 0, 90);

        hands[0].transform.position = 
            new Vector2(
                hands[0].transform.position.x, 
                target.transform.position.y
                );
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[2].SetActive(false);
        right = true;
        yield return new WaitForSeconds(1.5f);
        right = false;
        hands[0].transform.position = new Vector2(18, -1);

        hands[1].transform.position =
           new Vector2(
               hands[1].transform.position.x,
               target.transform.position.y
               );
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[3].SetActive(false);
        left = true;
        yield return new WaitForSeconds(1.5f);
        left = false;
        hands[1].transform.position = new Vector2(-18, -1);


        // 반복
        hands[0].transform.position = 
            new Vector2(
                hands[0].transform.position.x, 
                target.transform.position.y
                );
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[2].SetActive(false);
        right = true;
        yield return new WaitForSeconds(1.5f);
        right = false;
        hands[0].transform.position = new Vector2(18, -1);

        hands[1].transform.position =
           new Vector2(
               hands[1].transform.position.x,
               target.transform.position.y
               );
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[3].SetActive(false);
        left = true;
        yield return new WaitForSeconds(1.5f);
        left = false;
        hands[1].transform.position = new Vector2(-18, -1);

        rightLook.enabled = true; leftLook.enabled = true;
        rightHandBack = true; leftHandBack = true;
        yield return new WaitForSeconds(1f);
        rightHandBack = false; leftHandBack = false;
        isSwing = false;

        StartCoroutine(NextPattern());
    }
    IEnumerator ShootPattern()
    {
        right = true;
        for (int i = 0; i < 20;  i++)
        {
            isShoot = true;
            yield return new WaitForSeconds(0.001f);
            isShoot = false;
            yield return new WaitForSeconds(0.05f);
        }
        right = false;
        
        left = true;
        for (int i = 0; i < 20; i++)
        {
            isShoot = true;
            yield return new WaitForSeconds(0.001f);
            isShoot = false;
            yield return new WaitForSeconds(0.05f);
        }
        left = false;
        
        yield return new WaitForSeconds(1f);
        
        right = true; left = true;
        for (int i = 0; i < 20; i++)
        {
            isShoot = true;
            yield return new WaitForSeconds(0.001f);
            isShoot = false;
            yield return new WaitForSeconds(0.05f);
        }
        right = false; left = false;
        // 총알이 사라진 이후에 바로 다음 패턴이 시작되는 느낌이라 그걸 제거하기 위한 1초 기다림
        yield return new WaitForSeconds(1f);
        StartCoroutine(NextPattern());
    }
    IEnumerator BurstPattern()
    {
        isBurst = true;
        right = true; left=true;
        yield return new WaitForSeconds(2f);
        isBurst = false;
        right = false; left = false;
        StartCoroutine(NextPattern());
    }
    IEnumerator SpawnPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }

    void IsLaser()
    {   
        //우측 상단
        //float randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        //우측 하단
        //float randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        //좌측 상단
        //float randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        //좌측 하단
        //float randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        if (isLaser && rightUp)
        {
            // 오른손이 플레이어 기준으로 우측상단으로 이동하는 코드
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomRightAngle1),
                        target.transform.position.y + 8 * Mathf.Sin(randomRightAngle1)),
                    Time.deltaTime
                    );
        }

        if(isLaser && leftUp)
        {
            // 왼손이 플레이어 기준으로 좌측상단으로 이동하는 코드
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomLeftAngle1),
                        target.transform.position.y + 8 * Mathf.Sin(randomLeftAngle1)),
                    Time.deltaTime
                    );
        }

        if (isLaser && rightDown)
        {
            // 오른손이 플레이어 기준으로 우측하단으로 이동하는 코드
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomRightAngle2),
                        target.transform.position.y + 8 * Mathf.Sin(randomRightAngle2)),
                    Time.deltaTime
                    );
        }

        if (isLaser && leftDown)
        {
            // 왼손이 플레이어 기준으로 좌측하단으로 이동하는 코드
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomLeftAngle2),
                        target.transform.position.y + 8 * Mathf.Sin(randomLeftAngle2)),
                    Time.deltaTime
                    );
        }
    }

    void IsSwing()
    {
        if (isSwing && rightHandMove)
        {
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(18, -1),
                    Time.deltaTime
                    );
        }

        if (isSwing && leftHandMove)
        { 
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(-18, -1),
                    Time.deltaTime
                    );
        }

        if (isSwing && right)
        {
            hands[0].transform.position = 
                Vector2.MoveTowards(
                    hands[0].transform.position, 
                    new Vector2(
                        hands[0].transform.position.x-30, 
                        hands[0].transform.position.y),
                    Time.deltaTime * 40);
        }

        if (isSwing && left)
        {
            hands[1].transform.position =
                Vector2.MoveTowards(
                    hands[1].transform.position,
                    new Vector2(
                        hands[1].transform.position.x + 30,
                        hands[1].transform.position.y),
                    Time.deltaTime * 40);
        }
    }

    void IsShoot()
    {
        if (isShoot && right)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[0].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0,0,180+hands[0].transform.rotation.eulerAngles.z);

        }

        if (isShoot && left)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[1].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, -180 + hands[1].transform.rotation.eulerAngles.z);
        }
    }

    void IsBurst()
    {
        if (isBurst && right)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[0].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, 180 + hands[0].transform.rotation.eulerAngles.z);
        }
        if (isBurst && left)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[1].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, 180 + hands[1].transform.rotation.eulerAngles.z);
        }
    }

    void HandBack()
    {
        // 보스 손이 원래 위치로 이동하는 코드들
        if (rightHandBack)
        {
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    rightHandStartPos,
                    Time.deltaTime * 3
                    );
        }
        if (leftHandBack)
        {
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    leftHandStartPos,
                    Time.deltaTime * 3
                    );
        }
    }
}
