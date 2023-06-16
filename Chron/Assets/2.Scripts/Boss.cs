using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    // �̵� �Ÿ�
    float distance = 0.7f;
    
    // �̵� ����
    float rightDirection = 1f;
    float leftDirection = 1f;

    // swing���Ͽ��� ���� �̵� �ӵ�
    float handSpeed;

    bool isLaser, isSwing, isShoot, isBurst = false;
    bool right, left, rightUp, rightDown, leftUp, leftDown, rightHandMove, leftHandMove = false;
    bool rightHandBack, leftHandBack, handMove = false;
    bool rightBulletCooldown, leftBulletCooldown = false;

    bool berserkBoss = false;
    bool berserkRightUp, berserkRightDown, berserkLeftUp, berserkLeftDown = false;
    float berserkRightDirection = 1f;
    float berserkLeftDirection = 1f;

    GameObject enemyBullet; GameObject berserkEnemyBullet;
    public GameObject target;
    public GameObject[] hands;
    public GameObject[] laser;

    // ������ ũ�� ���� �� �Ѿ� �߻簢 ������ ��
    float addNum = 0;

    int nextPattern = 0;
    int beforePattern;

    Vector2 rightHandStartPos, leftHandStartPos;

    Vector2 berserkRightHandStartPos, berserkLeftHandStartPos;

    float randomRightAngle1, randomRightAngle2, randomLeftAngle1, randomLeftAngle2;

    BossHand rightLook, leftLook;
    BossHand berserkRightLook, berserkLeftLook;
    public enum Pattern
    {
        Laser,
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
        berserkRightHandStartPos = hands[2].transform.position;
        berserkLeftHandStartPos = hands[3].transform.position;

        // ���� �÷��̾ �ٶ󺸴� ��ũ��Ʈ
        rightLook = hands[0].gameObject.GetComponent<BossHand>();
        leftLook = hands[1].gameObject.GetComponent<BossHand>();
        berserkRightLook = hands[2].gameObject.GetComponent<BossHand>();
        berserkLeftLook = hands[3].gameObject.GetComponent<BossHand>();

        StartCoroutine(NextPattern());
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ óġ�� ��� ����
        if (target == null)
        {
            StopAllCoroutines();
            return;
        }

        // �÷��̾ ������ ��츦 ���
        if (target.transform.position.y > 3.5f)
            target.transform.position = new Vector2(target.transform.position.x, 1.8f);

        HandMove();
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
                StartCoroutine(BerserkLaserPattern());
                break;
            case 6:
                StartCoroutine(BerserkSwingPattern());
                break;
            case 7:
                StartCoroutine(BerserkShootPattern());
                break;
            case 8:
                StartCoroutine(BerserkBurstPattern());
                break;

        }
    }

    IEnumerator NextPattern()
    {
        if (GameManager.Instance.hpCtrl.health <= (0.5 * GameManager.Instance.hpCtrl.maxHealth))
        {
            berserkBoss = true;
            hands[2].SetActive(true);
            hands[3].SetActive(true);
        }

        handMove = true;
        beforePattern = nextPattern;
        if (berserkBoss)
            nextPattern = Random.Range(5, 9);
        else
            nextPattern = Random.Range(1, 5);
        if (beforePattern == nextPattern)
        {
            StartCoroutine(NextPattern());
        }
        else
        {
            if (!berserkBoss)
                yield return new WaitForSeconds(4f);
            else
                yield return new WaitForSeconds(3f);
            handMove = false;
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
        addNum = 0;

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
        addNum = 0;

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
        addNum = 0;

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
        addNum = 0;

        // �޼� lookat Ȱ��ȭ
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
        handSpeed = 40f;
        isSwing = true; rightHandMove = true; leftHandMove = true;

        yield return new WaitForSeconds(2f);

        rightHandMove = false; leftHandMove = false;
        leftLook.enabled = false; rightLook.enabled=false;

        hands[0].transform.eulerAngles = new Vector3(0, 0, -90);
        hands[1].transform.eulerAngles = new Vector3(0, 0, 90);

        for (int i = 0; i <= 1; i++)
        {
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
            hands[0].transform.position = new Vector2(30, -1);

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
        }

        rightLook.enabled = true; leftLook.enabled = true;
        rightHandBack = true; leftHandBack = true;

        yield return new WaitForSeconds(1f);

        rightHandBack = false; leftHandBack = false;
        isSwing = false;

        StartCoroutine(NextPattern());
    }
    IEnumerator ShootPattern()
    {
        isShoot = true;

        right = true;
        yield return new WaitForSeconds(2f);
        right = false;

        // �Ѿ� ��Ÿ�� �ڷ�ƾ�� �����ϴ� ���߿� �޼տ��� ���� �� ���, ��Ÿ�� �ڷ�ƾ�� ����� �۵����� �ʱ� ������ 1�� ��ٸ�
        yield return new WaitForSeconds(1f);

        left = true;
        yield return new WaitForSeconds(2f);
        left = false;
        
        yield return new WaitForSeconds(1f);
        
        right = true; left = true;
        yield return new WaitForSeconds(2f);
        right = false; left = false;

        isShoot = false;

        // �Ѿ��� ����� ���Ŀ� �ٷ� ���� ������ ���۵Ǵ� �����̶� �װ� �����ϱ� ���� 1�� ��ٸ�
        yield return new WaitForSeconds(1f);

        StartCoroutine(NextPattern());
    }
    IEnumerator BurstPattern()
    {
        isBurst = true;
        right = true; left = true;

        yield return new WaitForSeconds(2f);

        right = false; left = false;
        isBurst = false;
        addNum = 0;

        StartCoroutine(NextPattern());
    }

    IEnumerator BerserkLaserPattern()
    {
        // ������ ����
        isLaser = true;
        randomRightAngle1 = Random.Range(Mathf.PI / 2, 0.0f);
        randomRightAngle2 = Random.Range(0.0f, -Mathf.PI / 2);

        // 0.5�ʵ��� ������ �̵�
        rightUp = true;

        yield return new WaitForSeconds(0.5f);

        rightLook.enabled = false; rightUp = false;

        // 0.5�ʵ��� ����ȭ ������ �̵�
        berserkRightDown = true;
        // 0.5�ʵ��� ������ ���������� ��
        laser[2].SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        // ������ ���������� ���� 0.5�ʵ��� ������ ������ ��
        laser[2].SetActive(false);
        laser[0].SetActive(true);

        berserkRightLook.enabled = false; berserkRightDown = false;
        //0.5�ʵ��� ����ȭ ������ ���������� ��
        laser[8].SetActive(true);

        randomLeftAngle1 = Random.Range(Mathf.PI, Mathf.PI / 2);
        randomLeftAngle2 = Random.Range(-Mathf.PI, -Mathf.PI / 2);
        // 0.5�ʵ��� �޼� �̵�
        leftDown = true;

        yield return new WaitForSeconds(0.5f);

        // ������ ������ ���� addNum = 0
        laser[0].SetActive(false);
        addNum = 0;
        
        leftLook.enabled = false; leftDown = false; rightLook.enabled = true;

        // ����ȭ ������ ���������� ���� 0.5�ʵ��� ����ȭ ������ ������ ��
        laser[8].SetActive(false);
        laser[6].SetActive(true);
        // 0.5�ʵ��� �޼� ���������� ��
        laser[3].SetActive(true);
        // 0.5�ʵ��� ����ȭ �޼� �̵�
        berserkLeftUp = true;

        yield return new WaitForSeconds(0.5f);

        // ����ȭ ������ ������ ���� addNum = 0
        laser[6].SetActive(false);
        addNum = 0;

        berserkLeftLook.enabled = false; berserkLeftUp = false; berserkRightLook.enabled = true;
        // �޼� ���������� ��
        laser[3].SetActive(false);
        // 0.5�ʵ��� �޼� ������ ��
        laser[1].SetActive(true);
        // ����ȭ �޼� ���������� ��
        laser[9].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // �޼� ������ ���� addNum = 0
        laser[1].SetActive(false);
        addNum = 0;

        leftLook.enabled = true;
        // ����ȭ �޼� ���������� ��
        laser[9].SetActive(false);
        // 0.5�ʵ��� ����ȭ �޼� ������ ��
        laser[7].SetActive(true);


        yield return new WaitForSeconds(0.5f);

        // ����ȭ �޼� ������ ���� addNum = 0
        laser[7].SetActive(false);
        addNum = 0;

        // ����ȭ �޼� lookat ��
        berserkLeftLook.enabled = true;

        // 1�ʵ��� ��� ���ڸ���
        leftHandBack = true; rightHandBack = true;

        yield return new WaitForSeconds(1f);

        // �� ��� ����
        leftHandBack = false; rightHandBack = false;
        isLaser = false;
        StartCoroutine(NextPattern());
    }

    IEnumerator BerserkSwingPattern()
    {
        handSpeed = 65f;
        isSwing = true; rightHandMove = true; leftHandMove = true;

        yield return new WaitForSeconds(2f);

        rightHandMove = false; leftHandMove = false;
        leftLook.enabled = false; rightLook.enabled = false;
        berserkLeftLook.enabled = false; berserkRightLook.enabled = false;

        hands[0].transform.eulerAngles = new Vector3(0, 0, -90);
        hands[1].transform.eulerAngles = new Vector3(0, 0, 90);
        hands[2].transform.eulerAngles = new Vector3(0, 0, -90);
        hands[3].transform.eulerAngles = new Vector3(0, 0, 90);

        for (int i = 0; i <= 1; i++)
        {
            hands[0].transform.position =
                new Vector2(
                    hands[0].transform.position.x,
                    target.transform.position.y
                    );

            // Ÿ���� �߰����� �Ʒ� ���� ���(�߰� -3.75f)
            if (target.transform.position.y <= -3.75f)
            {
                hands[2].transform.position =
                    new Vector2(
                        hands[2].transform.position.x,
                        target.transform.position.y + 10f
                        );
            }

            // Ÿ���� �߰����� ���� ���� ���
            else
            {
                hands[2].transform.position =
                    new Vector2(
                        hands[2].transform.position.x,
                        target.transform.position.y - 10f
                        );
            }

            // �����ʿ��� ���������� ��
            laser[2].SetActive(true);
            laser[8].SetActive(true);

            yield return new WaitForSeconds(0.5f);

            laser[2].SetActive(false);
            laser[8].SetActive(false);
            
            // 1�ʵ��� �����ʿ��� �������� ���� �̵�
            right = true;
            yield return new WaitForSeconds(1f);
            right = false;

            hands[0].transform.position = new Vector2(30, 0);
            hands[2].transform.position = new Vector2(30, -10);

            // �޼� �̵�
            hands[1].transform.position =
               new Vector2(
                   hands[1].transform.position.x,
                   target.transform.position.y
                   );

            // Ÿ���� �߰����� �Ʒ� ���� ���(�߰� -3.75f)
            if (target.transform.position.y <= -3.75f)
            {
                hands[3].transform.position =
                    new Vector2(
                        hands[3].transform.position.x,
                        target.transform.position.y + 10f
                        );
            }

            // Ÿ���� �߰����� ���� ���� ���
            else
            {
                hands[3].transform.position =
                    new Vector2(
                        hands[3].transform.position.x,
                        target.transform.position.y - 10f
                        );
            }

            laser[3].SetActive(true);
            laser[9].SetActive(true);

            yield return new WaitForSeconds(0.5f);
            laser[3].SetActive(false);
            laser[9].SetActive(false);

            left = true;
            yield return new WaitForSeconds(1f);
            left = false;

            hands[1].transform.position = new Vector2(-18, 0);
            hands[3].transform.position = new Vector2(-18, -10);
        }

        rightLook.enabled = true; leftLook.enabled = true;
        berserkRightLook.enabled = true; berserkLeftLook.enabled = true;
        rightHandBack = true; leftHandBack = true;

        yield return new WaitForSeconds(1f);

        rightHandBack = false; leftHandBack = false;
        isSwing = false;

        StartCoroutine(NextPattern());
    }

    IEnumerator BerserkShootPattern()
    {
        isShoot = true;

        right = true; left = true;
        yield return new WaitForSeconds(4f);
        right = false; left = false;

        isShoot = false;

        // �Ѿ��� ����� ���Ŀ� �ٷ� ���� ������ ���۵Ǵ� �����̶� �װ� �����ϱ� ���� 1�� ��ٸ�
        yield return new WaitForSeconds(1f);

        StartCoroutine(NextPattern());
    }

    IEnumerator BerserkBurstPattern()
    {
        isBurst = true;
        right = true; left = true;

        yield return new WaitForSeconds(3f);

        right = false; left = false;
        isBurst = false;
        addNum = 0;

        StartCoroutine(NextPattern());
    }

    IEnumerator BulletCooldown(float cooldown)
    {
        // cooldown�ʵ��� ��Ÿ�� ��, �Ѿ� ��Ÿ�� ����
        yield return new WaitForSeconds(cooldown);
        rightBulletCooldown = false;
        leftBulletCooldown = false;
    }

    void IsLaser()
    {   
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

        if (isLaser && berserkRightUp)
        {
            // ����ȭ �������� �÷��̾� �������� ����������� �̵��ϴ� �ڵ�
            hands[2].transform.position =
                Vector2.Lerp(
                    hands[2].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomRightAngle1),
                        target.transform.position.y + 8 * Mathf.Sin(randomRightAngle1)),
                    Time.deltaTime
                    );
        }

        if (isLaser && leftUp)
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

        if (isLaser && berserkLeftUp)
        {
            // �޼��� �÷��̾� �������� ����������� �̵��ϴ� �ڵ�
            hands[3].transform.position =
                Vector2.Lerp(
                    hands[3].transform.position,
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

        if (isLaser && berserkRightDown)
        {
            // ����ȭ �������� �÷��̾� �������� �����ϴ����� �̵��ϴ� �ڵ�
            hands[2].transform.position =
                Vector2.Lerp(
                    hands[2].transform.position,
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

        if (isLaser && berserkLeftDown)
        {
            // �޼��� �÷��̾� �������� �����ϴ����� �̵��ϴ� �ڵ�
            hands[3].transform.position =
                Vector2.Lerp(
                    hands[3].transform.position,
                    new Vector2(
                        target.transform.position.x + 8 * Mathf.Cos(randomLeftAngle2),
                        target.transform.position.y + 8 * Mathf.Sin(randomLeftAngle2)),
                    Time.deltaTime
                    );
        }

        if (laser[0].activeSelf)
        {
            if (addNum < 3)
            {
                addNum += 0.05f;
                laser[4].transform.localScale = new Vector3(addNum, 30, 1);
            }
            else
                laser[4].transform.localScale = new Vector3(3, 30, 1);

        }

        if (laser[1].activeSelf)
        {
            if (addNum < 3)
            {
                addNum += 0.05f;
                laser[5].transform.localScale = new Vector3(addNum, 30, 1);
            }
            else
                laser[5].transform.localScale = new Vector3(3, 30, 1);
        }

        if (laser[6].activeSelf)
        {
            if (addNum < 3)
            {
                addNum += 0.05f;
                laser[10].transform.localScale = new Vector3(addNum, 30, 1);
            }
            else
                laser[10].transform.localScale = new Vector3(3, 30, 1);
        }

        if (laser[7].activeSelf)
        {
            if (addNum < 3)
            {
                addNum += 0.05f;
                laser[11].transform.localScale = new Vector3(addNum, 30, 1);
            }
            else
                laser[11].transform.localScale = new Vector3(3, 30, 1);
        }
    }

    void IsSwing()
    {
        if (isSwing && rightHandMove)
        {
            hands[0].transform.position =
                Vector2.Lerp(
                    hands[0].transform.position,
                    new Vector2(30, -1),
                    Time.deltaTime
                    );

            if (berserkBoss)
            {
                hands[2].transform.position =
                Vector2.Lerp(
                    hands[2].transform.position,
                    new Vector2(30, -4),
                    Time.deltaTime
                    );
            }
        }

        if (isSwing && leftHandMove)
        { 
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    new Vector2(-18, -1),
                    Time.deltaTime
                    );

            if (berserkBoss)
            {
                hands[3].transform.position =
                Vector2.Lerp(
                    hands[3].transform.position,
                    new Vector2(-18, -4),
                    Time.deltaTime
                    );
            }
        }

        if (isSwing && right)
        {
            hands[0].transform.position = 
                Vector2.MoveTowards(
                    hands[0].transform.position, 
                    new Vector2(
                        hands[0].transform.position.x-10, 
                        hands[0].transform.position.y),
                    Time.deltaTime * handSpeed);
            if (berserkBoss)
            {
                hands[2].transform.position =
                Vector2.MoveTowards(
                    hands[2].transform.position,
                    new Vector2(
                        hands[2].transform.position.x - 10,
                        hands[2].transform.position.y),
                    Time.deltaTime * handSpeed);
            }
        }

        if (isSwing && left)
        {
            hands[1].transform.position =
                Vector2.MoveTowards(
                    hands[1].transform.position,
                    new Vector2(
                        hands[1].transform.position.x + 10,
                        hands[1].transform.position.y),
                    Time.deltaTime * handSpeed);
            if (berserkBoss)
            {
                hands[3].transform.position =
                Vector2.MoveTowards(
                    hands[3].transform.position,
                    new Vector2(
                        hands[3].transform.position.x + 10,
                        hands[3].transform.position.y),
                    Time.deltaTime * handSpeed);
            }
        }
    }

    void IsShoot()
    {
        if (isShoot && right && !rightBulletCooldown)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[0].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0,0,180 + hands[0].transform.rotation.eulerAngles.z);
            
            if (berserkBoss)
            {
                berserkEnemyBullet = GameManager.Instance.poolManager.Get(2);
                berserkEnemyBullet.transform.position = hands[2].transform.position;
                berserkEnemyBullet.transform.eulerAngles = new Vector3(0, 0, 180 + hands[2].transform.rotation.eulerAngles.z);
            }

            rightBulletCooldown = true;
            StartCoroutine(BulletCooldown(0.05f));

        }

        if (isShoot && left && !leftBulletCooldown)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[1].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, -180 + hands[1].transform.rotation.eulerAngles.z);

            if (berserkBoss)
            {
                berserkEnemyBullet = GameManager.Instance.poolManager.Get(2);
                berserkEnemyBullet.transform.position = hands[3].transform.position;
                berserkEnemyBullet.transform.eulerAngles = new Vector3(0, 0, 180 + hands[3].transform.rotation.eulerAngles.z);
            }

            leftBulletCooldown = true;
            StartCoroutine(BulletCooldown(0.05f));
        }
    }

    void IsBurst()
    {
        // �Ѿ��� ��Ÿ�� ���°� �ƴ� ��� �Ѿ� �߻� ���� ����
        // 13�� ���ϴ� ������ 13�� 360���� �������� �ʱ� ������ �Ѿ��� �Ź� �ٸ� ��ġ�� �߻�Ǳ� ����.
        if (isBurst && (!rightBulletCooldown || !leftBulletCooldown))
            addNum += 13;

        if (isBurst && right && !rightBulletCooldown)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[0].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, -addNum);

            if (berserkBoss)
            {
                berserkEnemyBullet = GameManager.Instance.poolManager.Get(2);
                berserkEnemyBullet.transform.position = hands[2].transform.position;
                berserkEnemyBullet.transform.eulerAngles = new Vector3(0, 0, 0.5f * -addNum);
            }

            rightBulletCooldown = true;
            StartCoroutine(BulletCooldown(0.02f));
        }

        if (isBurst && left && !leftBulletCooldown)
        {
            enemyBullet = GameManager.Instance.poolManager.Get(2);
            enemyBullet.transform.position = hands[1].transform.position;
            enemyBullet.transform.eulerAngles = new Vector3(0, 0, addNum);

            if (berserkBoss)
            {
                berserkEnemyBullet = GameManager.Instance.poolManager.Get(2);
                berserkEnemyBullet.transform.position = hands[3].transform.position;
                berserkEnemyBullet.transform.eulerAngles = new Vector3(0, 0, 0.5f * addNum);
            }

            leftBulletCooldown = true;
            StartCoroutine(BulletCooldown(0.02f));
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
            if (berserkBoss)
            {
                hands[2].transform.position =
                Vector2.Lerp(
                    hands[2].transform.position,
                    berserkRightHandStartPos,
                    Time.deltaTime * 3
                    );
            }

        }
        if (leftHandBack)
        {
            hands[1].transform.position =
                Vector2.Lerp(
                    hands[1].transform.position,
                    leftHandStartPos,
                    Time.deltaTime * 3
                    );
            if (berserkBoss)
            {
                hands[3].transform.position =
                Vector2.Lerp(
                    hands[3].transform.position,
                    berserkLeftHandStartPos,
                    Time.deltaTime * 3
                    );
            }
        }
    }

    void HandMove()
    {
        if (handMove)
        {
            float rightHandMoveAmount = rightDirection * 0.6f * Time.deltaTime;
            hands[0].transform.Translate(0f, rightHandMoveAmount, 0f, Space.World);
            if (Mathf.Abs(hands[0].transform.position.y - rightHandStartPos.y) >= distance)
            {
                rightDirection *= -1f;
            }

            float leftHandMoveAmount = leftDirection * 0.6f * Time.deltaTime;
            hands[1].transform.Translate(0f, leftHandMoveAmount, 0f, Space.World);
            if (Mathf.Abs(hands[1].transform.position.y - leftHandStartPos.y) >= distance)
            {
                leftDirection *= -1f;
            }

            if (berserkBoss)
            {
                float berserkRightHandMoveAmount = berserkRightDirection * 0.6f * Time.deltaTime;
                hands[2].transform.Translate(0f, berserkRightHandMoveAmount, 0f, Space.World);
                if (Mathf.Abs(hands[2].transform.position.y - berserkRightHandStartPos.y) >= distance)
                {
                    berserkRightDirection *= -1f;
                }

                float berserkLeftHandMoveAmount = berserkLeftDirection * 0.6f * Time.deltaTime;
                hands[3].transform.Translate(0f, berserkLeftHandMoveAmount, 0f, Space.World);
                if (Mathf.Abs(hands[3].transform.position.y - berserkLeftHandStartPos.y) >= distance)
                {
                    berserkLeftDirection *= -1f;
                }

            }

        }
    }

}
