using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public bool bulletCooldown, timeReversalCooldown;
    public static int bulletLevel = 0;
    public static int maxHealth = 10;
    public static int health = maxHealth;
    public static int startMaxHealth = maxHealth;
    public List<GameObject> effect;
    public Vector2 playerPos, mousePos, playerMovePos;
    float dis;
    public float bounceAngle, bounceSpeed;
    public float angle;
    public bool action = true;
    List<Vector2> posList = new List<Vector2>();
    GameObject arrow, bullet;
    public float originSpeed;
    private Coroutine moveCor;
    public float speed, disVal, maxArrowX, maxArrowY;
    Rigidbody2D rb2;

    void Start()
    {
        Enemy.enemyCount = 0;
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 플레이어와 마우스 위치 받아오기
        playerPos = transform.position;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // 각도와 거리 계산
        angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
        dis = Vector2.Distance(playerPos, mousePos);
        // 최대 거리 제한
        if (dis > disVal)
            dis = disVal;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // 특수 능력(shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 쿨타임이 다 돌았을 경우
            if (timeReversalCooldown == false && posList.Count > 0)
            {
                // 플레이어의 이동을 멈추고 능력 발동
                rb2.velocity = Vector2.zero;
                originSpeed = 0;
                StartCoroutine(TimeReversal());
            }

        }

        // 이동이 인정되는 경우(action이 활성화인 경우)
        if (action == true)
        {
            playerMove();
            // 총알 발사 쿨타임이 다 돌았을 경우
            if (bulletCooldown == false)
                fireBullet();
        }
    }

    void playerMove()
    {
        // 우클릭 시 게임 속도가 느려지고 이동방향을 가리키는 화살표 생성
        if (Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 0.125f;
            arrow = GameManager.Instance.poolManager.Get(0);

        }

        // 우클릭 상태일 경우, 화살표가 마우스 위치에 따라 크기와 가리키는 방향이 변함
        if (Input.GetMouseButton(1))
        {
            arrow.transform.localScale = new Vector2(0.05f + dis * (maxArrowX / disVal), 0.1f + dis * (maxArrowY / disVal));
            arrow.transform.position = new Vector2(playerPos.x + 2 * (arrow.transform.localScale.y * Mathf.Cos(angle)), playerPos.y + 2 * (arrow.transform.localScale.y * Mathf.Sin(angle)));
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
        }

        // 우클릭을 땠을 경우
        if (Input.GetMouseButtonUp(1))
        {
            // 플레이어가 그라운드와 충돌이 일어날 경우에 튕겨져나갈 방향을 지정해줌
            bounceAngle = angle;
            // 화살 오브젝트 비활성화
            arrow.SetActive(false);

            // posList(플레이어의 이동하기 전 위치를 담은 리스트) 안에 변수가 5개보다 적게 들어있을 경우, posList에 현 플레이어 위치 추가
            if (posList.Count < 5)
                posList.Add(playerPos);

            // posList 안에 변수가 5개 이상일 경우, 0번째 인덱스 값을 제거하고 현 플레이어 위치 추가
            else if (posList.Count >= 5)
            {
                posList.Remove(posList[0]);
                posList.Add(playerPos);
            }

            // 게임 속도를 원래대로 돌림
            Time.timeScale = 1;

            // 플레이어 속도를 0으로 조정(플레이어 이동 중에 다시 우클릭을 할 경우, 예전 속도 때문에 이상한 방향으로 나가는 것을 방지)
            rb2.velocity = Vector2.zero;
            // 충돌을 하지 않았을 경우 bounceSpeed =1, 충돌했을 경우 값이 변함.
            bounceSpeed = 1;
            // 플레이어의 이동 벡터
            playerMovePos = bounceSpeed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            // moveCor(이동 코루틴)이 null이 아닐 경우(= 이동 코루틴이 작동하고 있을 경우)
            if (moveCor != null)
            {
                // moveCor를 끝내고 moveCor = null
                StopCoroutine(moveCor);
                moveCor = null;
            }
            // 플레이어 이동 시작
            moveCor = StartCoroutine(MoveCor());
        }
    }

    // (moveCor은 플레이어의 이동 실행 코루틴)
    private IEnumerator MoveCor()
    {
        // 플레이어 이동속도
        originSpeed = speed * dis;
        
        // 플레이어 이동속도가 양수일 경우
        while (originSpeed > 0)
        {
            // 플레이어 이동 벡터쪽으로 이동속도만큼 이동
            transform.Translate(playerMovePos * originSpeed * Time.deltaTime);
            // 서서히 속도 감소
            originSpeed -= originSpeed * Time.deltaTime;
            yield return null;

        }
        // 이동속도가 0 이하이면(이동하지 않는 경우), moveCor = null
        moveCor = null;

    }

    // (총알발사)
    void fireBullet()
    {
        // 좌클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            switch (bulletLevel)
            {
                case 0:
                    // poolManager에서 총알을 소환
                    bullet = GameManager.Instance.poolManager.Get(1);
                    // 총알의 위치와 각도 설정
                    bullet.transform.position = playerPos;
                    bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
                    // 총알 쿨타임 시작 및 쿨타임 코루틴 실행
                    bulletCooldown = true;
                    StartCoroutine(BulletCooldown(0.3f));
                    break;
                case 1:
                    for (float i = -0.15f; i <= 0.15f; i += 0.3f)
                    {
                        // poolManager에서 총알을 소환
                        bullet = GameManager.Instance.poolManager.Get(1);
                        // 총알의 위치와 각도 설정
                        bullet.transform.position = new Vector2(playerPos.x - i * Mathf.Sin(angle), playerPos.y + i * Mathf.Cos(angle));
                        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

                    }
                    // 총알 쿨타임 시작 및 쿨타임 코루틴 실행
                    bulletCooldown = true;
                    StartCoroutine(BulletCooldown(0.3f));
                    break;
                case 2:
                    for (float i = -0.15f; i <= 0.15f; i += 0.3f)
                    {
                        // poolManager에서 총알을 소환
                        bullet = GameManager.Instance.poolManager.Get(1);

                        bullet.transform.position = new Vector2(playerPos.x - i * Mathf.Sin(angle), playerPos.y + i * Mathf.Cos(angle));
                        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

                    }
                    bulletCooldown = true;
                    StartCoroutine(BulletCooldown(0.1f));
                    break;

                case 3:
                    for (float i = -0.3f; i <= 0.3f; i += 0.3f)
                    {
                        // poolManager에서 총알을 소환
                        bullet = GameManager.Instance.poolManager.Get(1);

                        bullet.transform.position = new Vector2(playerPos.x - i * Mathf.Sin(angle), playerPos.y + i * Mathf.Cos(angle));
                        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

                    }
                    bulletCooldown = true;
                    StartCoroutine(BulletCooldown(0.1f));
                    break;

                case 4: default:
                    for (float i = -0.3f; i <= 0.3f; i += 0.3f)
                    {
                        // poolManager에서 총알을 소환
                        bullet = GameManager.Instance.poolManager.Get(1);

                        bullet.transform.position = new Vector2(playerPos.x - i * Mathf.Sin(angle), playerPos.y + i * Mathf.Cos(angle));
                        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

                        GameObject bullet2 = GameManager.Instance.poolManager.Get(1);
                        bullet2.transform.position = new Vector2(playerPos.x - i * Mathf.Sin(angle) + 0.5f * (Mathf.Cos(angle)), playerPos.y + i * Mathf.Cos(angle) + 0.5f * (Mathf.Sin(angle)));
                        bullet2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);

                    }
                    bulletCooldown = true;
                    StartCoroutine(BulletCooldown(0.1f));
                    break;

            }

        }
    }

    // (총알 쿨타임 코루틴)
    IEnumerator BulletCooldown(float cooldown)
    {
        // 0.3초동안 쿨타임 후, 총알 쿨타임 종료
        yield return new WaitForSeconds(cooldown);
        bulletCooldown = false;
    }

    // (특수 능력 코루틴)
    IEnumerator TimeReversal()
    {
        // 플레이어의 콜라이더를 끔(능력 중에는 플레이어가 충돌되지 않도록)
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

        // 특수 능력 쿨타임 시작
        timeReversalCooldown = true;

        // 액션 = false(특수 능력 중에 플레이어의 이동 및 공격을 하지 못하도록 설정)
        action = false;

        // 화살표 비활성화(우클릭 누른 상태에서 특수 능력 사용할 경우, 화살표가 남는 버그 때문에 여기서도 따로 비활성화 해줘야 함.)
        arrow.SetActive(false);

        // 게임속도를 원래대로(위와 동일한 경우로 우클릭 누른 상태에서 사용하면 속도가 느려지는 버그를 없애기 위해 설정함)
        Time.timeScale = 1;

        // 1초 동안 특수 능력 시작 이펙트 소환
        Instantiate(effect[0], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        // posList에 있는 인덱스만큼 실행
        for (int i = posList.Count - 1; i >= 0; i--)
        {
            // 플레이어 위치는 플레이어의 이동하기 전 위치
            transform.position = posList[i];
            // 0.2초동안 이펙트 소환
            Instantiate(effect[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        
        // 특수 능력 마지막 이펙트 실행
        ParticleSystem endTimeReversal = Instantiate(effect[0], transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        // 특수 능력 마지막 이펙트의 크기 조절
        endTimeReversal.gameObject.transform.localScale = Vector3.one * 3;

        // 플레이어의 이동하기 전 위치 리스트 내용을 모두 제거
        posList.Clear();

        // 플레이어의 이동 및 공격 다시 활성화
        action = true;

        // 플레이어 콜라이더 다시 활성화
        gameObject.GetComponent<CircleCollider2D>().enabled = true;

        // 특수 능력 쿨타임 실행
        yield return new WaitForSeconds(8f);
        timeReversalCooldown = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            bounceSpeed = 0.3f;
            //오브젝트와 충돌할 경우에 플레이어 이동방향의 반대 방향으로 튕김
            playerMovePos = -bounceSpeed * playerMovePos;

            //플레이어 기준 제1사분면 제2사분면일 경우
            if (bounceAngle > 0)
            {
                //바운스앵글 조정 이유: 오브젝트와 충둘하여 튕겨져나간 후 그라운드와 충돌 시 플레이어가 튕겨져 나갈 각도를 정해줘야 하기 때문
                bounceAngle = -Mathf.PI + bounceAngle;

            }

            //플레이어 기준 제3사분면 제4사분면일 경우
            else if (bounceAngle < 0)
            {
                bounceAngle = Mathf.PI + bounceAngle;

            }

        }

        // 그라운드(맵)와 충돌할 경우
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 충돌 위치 x값이 플레이어 x 위치보다 약간 더 크거나 작을 경우(플레이어의 좌우 방향에서 충돌이 일어난 경우)
            if (collision.contacts[0].point[0] > playerPos.x + 0.1 || collision.contacts[0].point[0] < playerPos.x - 0.1)
            {
                // 플레이어 이동 벡터는 튕겨져나가는 방향의 벡터로 변환함(그 방향으로 이동하게 됨)
                playerMovePos = bounceSpeed * new Vector2(-Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));

                // 충돌 후에 bounceAngle값 조정(충돌 후, 다른 그라운드와 충돌할 경우에 튕겨져나가는 방향을 다시 조정해줘야하기 때문)
                // 충돌 전에 bounceAngle이 제2사분면을 가리킬 경우, bounceAngle이 제1사분면으로 바뀜
                if (bounceAngle > Mathf.PI / 2)
                    bounceAngle = Mathf.PI - bounceAngle;
                // 충돌 전에 bounceAngle이 제1사분면을 가리킬 경우, bounceAngle이 제2사분면으로 바뀜
                else if (bounceAngle < Mathf.PI / 2 && bounceAngle > 0)
                    bounceAngle = Mathf.PI / 2 + bounceAngle;
                // 충돌 전에 bounceAngle이 제4사분면을 가리킬 경우, bounceAngle이 제3사분면으로 바뀜
                else if (bounceAngle > -(Mathf.PI / 2) && bounceAngle < 0)
                    bounceAngle = -(Mathf.PI / 2) + bounceAngle;

                // 충돌 전에 bounceAngle이 제3사분면을 가리킬 경우, bounceAngle이 제4사분면으로 바뀜
                else if (bounceAngle < -(Mathf.PI / 2))
                    bounceAngle = -Mathf.PI - bounceAngle;
            }

            // 충돌 위치 y값에 대한 코드로 위 코드와 비슷함
            if (collision.contacts[0].point[1] > playerPos.y + 0.1 || collision.contacts[0].point[1] < playerPos.y - 0.1)
            {
                playerMovePos = bounceSpeed * new Vector2(Mathf.Cos(bounceAngle), -Mathf.Sin(bounceAngle));
                if (bounceAngle > Mathf.PI / 2)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle < Mathf.PI / 2 && bounceAngle > 0)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle > -(Mathf.PI / 2) && bounceAngle < 0)
                    bounceAngle = -bounceAngle;
                else if (bounceAngle < -(Mathf.PI / 2))
                    bounceAngle = -bounceAngle;
            }
        }
    }

    void OnDestroy()
    {
        Time.timeScale = 1;
        if (arrow != null)
            arrow.SetActive(false);
    }

}