using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCtrl : MonoBehaviour
{
    bool bulletCooldown, timeReversalCooldown;
    public static int maxHealth = 100;
    public static int health = maxHealth;
    public List<GameObject> effect;
    public Vector2 playerPos, mousePos, playerMovePos;
    float dis;
    public float bounceAngle, bounceSpeed;
    public float angle;
    bool action = true;
    List<Vector2> posList = new List<Vector2>();
    GameObject arrow, bullet;
    public float originSpeed;
    private Coroutine moveCor;
    public float speed, disVal, maxArrowX, maxArrowY;
    Rigidbody2D rb2;

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �÷��̾�� ���콺 ��ġ �޾ƿ���
        playerPos = transform.position;
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // ������ �Ÿ� ���
        angle = Mathf.Atan2(mousePos.y - playerPos.y, mousePos.x - playerPos.x);
        dis = Vector2.Distance(playerPos, mousePos);
        // �ִ� �Ÿ� ����
        if (dis > disVal)
            dis = disVal;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Ư�� �ɷ�(shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // ��Ÿ���� �� ������ ���
            if (timeReversalCooldown == false && posList.Count > 0)
            {
                // �÷��̾��� �̵��� ���߰� �ɷ� �ߵ�
                rb2.velocity = Vector2.zero;
                originSpeed = 0;
                StartCoroutine(TimeReversal());
            }

        }

        // �̵��� �����Ǵ� ���(action�� Ȱ��ȭ�� ���)
        if (action == true)
        {
            playerMove();
            // �Ѿ� �߻� ��Ÿ���� �� ������ ���
            if (bulletCooldown == false)
                fireBullet();
        }
    }

    void playerMove()
    {
        // ��Ŭ�� �� ���� �ӵ��� �������� �̵������� ����Ű�� ȭ��ǥ ����
        if (Input.GetMouseButtonDown(1))
        {
            Time.timeScale = 0.125f;
            arrow = GameManager.Instance.poolManager.Get(0);

        }

        // ��Ŭ�� ������ ���, ȭ��ǥ�� ���콺 ��ġ�� ���� ũ��� ����Ű�� ������ ����
        if (Input.GetMouseButton(1))
        {
            arrow.transform.localScale = new Vector2(0.05f + dis * (maxArrowX / disVal), 0.1f + dis * (maxArrowY / disVal));
            arrow.transform.position = new Vector2(playerPos.x + 2 * (arrow.transform.localScale.y * Mathf.Cos(angle)), playerPos.y + 2 * (arrow.transform.localScale.y * Mathf.Sin(angle)));
            arrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
        }

        // ��Ŭ���� ���� ���
        if (Input.GetMouseButtonUp(1))
        {
            // �÷��̾ �׶���� �浹�� �Ͼ ��쿡 ƨ�������� ������ ��������
            bounceAngle = angle;
            // ȭ�� ������Ʈ ��Ȱ��ȭ
            arrow.SetActive(false);

            // posList(�÷��̾��� �̵��ϱ� �� ��ġ�� ���� ����Ʈ) �ȿ� ������ 5������ ���� ������� ���, posList�� �� �÷��̾� ��ġ �߰�
            if (posList.Count < 5)
                posList.Add(playerPos);

            // posList �ȿ� ������ 5�� �̻��� ���, 0��° �ε��� ���� �����ϰ� �� �÷��̾� ��ġ �߰�
            else if (posList.Count >= 5)
            {
                posList.Remove(posList[0]);
                posList.Add(playerPos);
            }

            // ���� �ӵ��� ������� ����
            Time.timeScale = 1;

            // �÷��̾� �ӵ��� 0���� ����(�÷��̾� �̵� �߿� �ٽ� ��Ŭ���� �� ���, ���� �ӵ� ������ �̻��� �������� ������ ���� ����)
            rb2.velocity = Vector2.zero;
            // �浹�� ���� �ʾ��� ��� bounceSpeed =1, �浹���� ��� ���� ����.
            bounceSpeed = 1;
            // �÷��̾��� �̵� ����
            playerMovePos = bounceSpeed * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            // moveCor(�̵� �ڷ�ƾ)�� null�� �ƴ� ���(= �̵� �ڷ�ƾ�� �۵��ϰ� ���� ���)
            if (moveCor != null)
            {
                // moveCor�� ������ moveCor = null
                StopCoroutine(moveCor);
                moveCor = null;
            }
            // �÷��̾� �̵� ����
            moveCor = StartCoroutine(MoveCor());
        }
    }

    // (moveCor�� �÷��̾��� �̵� ���� �ڷ�ƾ)
    private IEnumerator MoveCor()
    {
        // �÷��̾� �̵��ӵ�
        originSpeed = speed * dis;
        
        // �÷��̾� �̵��ӵ��� ����� ���
        while (originSpeed > 0)
        {
            // �÷��̾� �̵� ���������� �̵��ӵ���ŭ �̵�
            transform.Translate(playerMovePos * originSpeed * Time.deltaTime);
            // ������ �ӵ� ����
            originSpeed -= originSpeed * Time.deltaTime;
            yield return null;

        }
        // �̵��ӵ��� 0 �����̸�(�̵����� �ʴ� ���), moveCor = null
        moveCor = null;

    }

    // (�Ѿ˹߻�)
    void fireBullet()
    {
        // ��Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
        {
            // poolManager���� �Ѿ��� ��ȯ
            bullet = GameManager.Instance.poolManager.Get(1);
            // �Ѿ��� ��ġ�� ���� ����
            bullet.transform.position = playerPos;
            bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg - 90);
            // �Ѿ� ��Ÿ�� ���� �� ��Ÿ�� �ڷ�ƾ ����
            bulletCooldown = true;
            StartCoroutine("BulletCooldown");

        }
    }

    // (�Ѿ� ��Ÿ�� �ڷ�ƾ)
    IEnumerator BulletCooldown()
    {
        // 0.3�ʵ��� ��Ÿ�� ��, �Ѿ� ��Ÿ�� ����
        yield return new WaitForSeconds(0.3f);
        bulletCooldown = false;
    }

    // (Ư�� �ɷ� �ڷ�ƾ)
    IEnumerator TimeReversal()
    {
        // �÷��̾��� �ݶ��̴��� ��(�ɷ� �߿��� �÷��̾ �浹���� �ʵ���)
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        // Ư�� �ɷ� ��Ÿ�� ����
        timeReversalCooldown = true;
        // �׼� = false(Ư�� �ɷ� �߿� �÷��̾��� �̵� �� ������ ���� ���ϵ��� ����)
        action = false;
        // ȭ��ǥ ��Ȱ��ȭ(��Ŭ�� ���� ���¿��� Ư�� �ɷ� ����� ���, ȭ��ǥ�� ���� ���� ������ ���⼭�� ���� ��Ȱ��ȭ ����� ��.)
        arrow.SetActive(false);
        // ���Ӽӵ��� �������(���� ������ ���� ��Ŭ�� ���� ���¿��� ����ϸ� �ӵ��� �������� ���׸� ���ֱ� ���� ������)
        Time.timeScale = 1;
        // 1�� ���� Ư�� �ɷ� ���� ����Ʈ ��ȯ
        Instantiate(effect[0], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        // posList�� �ִ� �ε�����ŭ ����
        for (int i = posList.Count - 1; i >= 0; i--)
        {
            // �÷��̾� ��ġ�� �÷��̾��� �̵��ϱ� �� ��ġ
            transform.position = posList[i];
            // 0.2�ʵ��� ����Ʈ ��ȯ
            Instantiate(effect[1], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        
        // Ư�� �ɷ� ������ ����Ʈ ����
        ParticleSystem endTimeReversal = Instantiate(effect[0], transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        // Ư�� �ɷ� ������ ����Ʈ�� ũ�� ����
        endTimeReversal.gameObject.transform.localScale = Vector3.one * 3;
        // �÷��̾��� �̵��ϱ� �� ��ġ ����Ʈ ������ ��� ����
        posList.Clear();
        // �÷��̾��� �̵� �� ���� �ٽ� Ȱ��ȭ
        action = true;
        // �÷��̾� �ݶ��̴� �ٽ� Ȱ��ȭ
        gameObject.GetComponent<CircleCollider2D>().enabled = true;

        // Ư�� �ɷ� ��Ÿ�� ����
        yield return new WaitForSeconds(4f);
        timeReversalCooldown = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            bounceSpeed = 0.3f;
            //������Ʈ�� �浹�� ��쿡 �÷��̾� �̵������� �ݴ� �������� ƨ��
            playerMovePos = -bounceSpeed * playerMovePos;

            //�÷��̾� ���� ��1��и� ��2��и��� ���
            if (bounceAngle > 0)
            {
                //�ٿ�ޱ� ���� ����: ������Ʈ�� ����Ͽ� ƨ�������� �� �׶���� �浹 �� �÷��̾ ƨ���� ���� ������ ������� �ϱ� ����
                bounceAngle = -Mathf.PI + bounceAngle;

            }

            //�÷��̾� ���� ��3��и� ��4��и��� ���
            else if (bounceAngle < 0)
            {
                bounceAngle = Mathf.PI + bounceAngle;

            }

        }

        // �׶���(��)�� �浹�� ���
        if (collision.gameObject.CompareTag("Ground"))
        {
            // �浹 ��ġ x���� �÷��̾� x ��ġ���� �ణ �� ũ�ų� ���� ���(�÷��̾��� �¿� ���⿡�� �浹�� �Ͼ ���)
            if (collision.contacts[0].point[0] > playerPos.x + 0.1 || collision.contacts[0].point[0] < playerPos.x - 0.1)
            {
                // �÷��̾� �̵� ���ʹ� ƨ���������� ������ ���ͷ� ��ȯ��(�� �������� �̵��ϰ� ��)
                playerMovePos = bounceSpeed * new Vector2(-Mathf.Cos(bounceAngle), Mathf.Sin(bounceAngle));

                // �浹 �Ŀ� bounceAngle�� ����(�浹 ��, �ٸ� �׶���� �浹�� ��쿡 ƨ���������� ������ �ٽ� ����������ϱ� ����)
                // �浹 ���� bounceAngle�� ��2��и��� ����ų ���, bounceAngle�� ��1��и����� �ٲ�
                if (bounceAngle > Mathf.PI / 2)
                    bounceAngle = Mathf.PI - bounceAngle;
                // �浹 ���� bounceAngle�� ��1��и��� ����ų ���, bounceAngle�� ��2��и����� �ٲ�
                else if (bounceAngle < Mathf.PI / 2 && bounceAngle > 0)
                    bounceAngle = Mathf.PI / 2 + bounceAngle;
                // �浹 ���� bounceAngle�� ��4��и��� ����ų ���, bounceAngle�� ��3��и����� �ٲ�
                else if (bounceAngle > -(Mathf.PI / 2) && bounceAngle < 0)
                    bounceAngle = -(Mathf.PI / 2) + bounceAngle;

                // �浹 ���� bounceAngle�� ��3��и��� ����ų ���, bounceAngle�� ��4��и����� �ٲ�
                else if (bounceAngle < -(Mathf.PI / 2))
                    bounceAngle = -Mathf.PI - bounceAngle;
            }

            // �浹 ��ġ y���� ���� �ڵ�� �� �ڵ�� �����
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
        if (arrow != null)
            arrow.SetActive(false);
        Time.timeScale = 1;
    }
}