using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    bool isLaser, isSwing, isShoot, isBurst, isSpawn = false;
    bool rightUp, rightDown, leftUp, leftDown, rightHandMove, leftHandMove = false;
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
        // �� ���� ��ġ �޾ƿ���
        rightHandStartPos = hands[0].transform.position;
        leftHandStartPos = hands[1].transform.position;
        // ���� �÷��̾ �ٶ󺸴� ��ũ��Ʈ
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
        nextPattern = Random.Range(3, 4);
        if (beforePattern != nextPattern)
        {
            StartCoroutine(NextPattern());
        }
        else
        {
            yield return new WaitForSeconds(2f);
            StartCoroutine(BossPattern());
        }
    }
    IEnumerator LaserPattern()
    {
        // ������ ����
        isLaser = true;
        randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        // �������� 1�ʵ��� ���� ������� �̵�
        rightUp = true;
        yield return new WaitForSeconds(1f);
        // ������ lookat ���߰� ������ �̵� ����
        rightLook.enabled = false; rightUp = false;
        // ������ ���������� Ȱ��ȭ
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5�� �� ���������� ��Ȱ��ȭ �� ������ Ȱ��ȭ
        laser[2].SetActive(false);
        laser[0].SetActive(true);

        randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        // �޼��� 1�ʵ��� ���� �ϴ����� �̵�
        leftDown = true;
        yield return new WaitForSeconds(1f);
        // �޼� lookat ���߰� �޼� �̵� ���� + ������ lookat Ȱ��ȭ
        leftLook.enabled = false; leftDown = false; rightLook.enabled = true;
        // ������ ������ ��Ȱ��ȭ
        laser[0].SetActive(false);
        // �޼� ���������� Ȱ��ȭ
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5�� �� ���������� ��Ȱ��ȭ �� ������ Ȱ��ȭ
        laser[3].SetActive(false);
        laser[1].SetActive(true);

        randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        // �������� 1�ʵ��� ���� �ϴ����� �̵�
        rightDown = true;
        yield return new WaitForSeconds(1f);
        // ������ lookat ���߰� ������ �̵� ���� + �޼� lookat Ȱ��ȭ
        rightLook.enabled = false; rightDown = false; leftLook.enabled = true;
        // �޼� ������ ��Ȱ��ȭ
        laser[1].SetActive(false);
        // ������ ���������� Ȱ��ȭ
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5�� �� ���������� ��Ȱ��ȭ �� ������ Ȱ��ȭ
        laser[2].SetActive(false);
        laser[0].SetActive(true);

        randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        // �޼��� 1�ʵ��� ���� ������� �̵�
        leftUp = true;
        yield return new WaitForSeconds(1f);
        // �޼� lookat ���߰� �޼� �̵� ���� + ������ lookat Ȱ��ȭ
        leftLook.enabled = false; leftUp = false; rightLook.enabled = true;
        // ������ ������ ��Ȱ��ȭ
        laser[0].SetActive(false);
        // �޼� ���������� Ȱ��ȭ
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        // 0.5�� �� ���������� ��Ȱ��ȭ �� ������ Ȱ��ȭ
        laser[3].SetActive(false);
        laser[1].SetActive(true);

        // 1�ʵ��� ������ ���� �ڸ��� ���ư���
        rightHandBack = true;
        yield return new WaitForSeconds(1f);
        // �޼� ������ ��Ȱ��ȭ
        laser[1].SetActive(false);
        // �޼� lookat ����
        leftLook.enabled = true;
        // 1�ʵ��� �޼� ���� �ڸ��� ���ư��� ������ �̵� ����
        leftHandBack = true; rightHandBack = false;
        yield return new WaitForSeconds(1f);
        // �޼� �̵� ����
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
        rightUp = true;
        yield return new WaitForSeconds(1.5f);
        rightUp = false;
        hands[0].transform.position = new Vector2(18, -1);

        hands[1].transform.position =
           new Vector2(
               hands[1].transform.position.x,
               target.transform.position.y
               );
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[3].SetActive(false);
        leftUp = true;
        yield return new WaitForSeconds(1.5f);
        leftUp = false;
        hands[1].transform.position = new Vector2(-18, -1);


        // �ݺ�
        hands[0].transform.position = 
            new Vector2(
                hands[0].transform.position.x, 
                target.transform.position.y
                );
        laser[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[2].SetActive(false);
        rightUp = true;
        yield return new WaitForSeconds(1.5f);
        rightUp = false;
        hands[0].transform.position = new Vector2(18, -1);

        hands[1].transform.position =
           new Vector2(
               hands[1].transform.position.x,
               target.transform.position.y
               );
        laser[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        laser[3].SetActive(false);
        leftUp = true;
        yield return new WaitForSeconds(1.5f);
        leftUp = false;
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
        isShoot = true; rightUp = true;
        yield return new WaitForSeconds(1f);
        rightUp = false; leftUp = true;
        yield return new WaitForSeconds(1f);
        leftUp = false;
        yield return new WaitForSeconds(1f);
        rightUp = true; leftUp = true;
        yield return new WaitForSeconds(1f);
        rightUp = false; leftUp = false;
        StartCoroutine(NextPattern());
    }
    IEnumerator BurstPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }
    IEnumerator SpawnPattern()
    {
        yield return null;
        StartCoroutine(NextPattern());
    }

    void IsLaser()
    {   
        //���� ���
        //float randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        //���� �ϴ�
        //float randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);
        //���� ���
        //float randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        //���� �ϴ�
        //float randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        if (isLaser && rightUp)
        {
            // �������� �÷��̾� �������� ����������� �̵��ϴ� �ڵ�
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
            // �޼��� �÷��̾� �������� ����������� �̵��ϴ� �ڵ�
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
            // �������� �÷��̾� �������� �����ϴ����� �̵��ϴ� �ڵ�
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
            // �޼��� �÷��̾� �������� �����ϴ����� �̵��ϴ� �ڵ�
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

        if (isSwing && rightUp)
        {
            hands[0].transform.position = 
                Vector2.MoveTowards(
                    hands[0].transform.position, 
                    new Vector2(
                        hands[0].transform.position.x-30, 
                        hands[0].transform.position.y),
                    Time.deltaTime * 30);
        }

        if (isSwing && leftUp)
        {
            hands[1].transform.position =
                Vector2.MoveTowards(
                    hands[1].transform.position,
                    new Vector2(
                        hands[1].transform.position.x + 30,
                        hands[1].transform.position.y),
                    Time.deltaTime * 30);
        }
    }

    void IsShoot()
    {
        if (isShoot && rightUp)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[0].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0,0,180+hands[0].transform.rotation.eulerAngles.z);

        }

        if (isShoot && leftUp)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[1].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, -180 + hands[1].transform.rotation.eulerAngles.z);

        }
    }

    void HandBack()
    {
        // ���� ���� ���� ��ġ�� �̵��ϴ� �ڵ��
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
