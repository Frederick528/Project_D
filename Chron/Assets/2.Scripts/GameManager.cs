using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolManager poolManager;
    public PlayerCtrl playerCtrl;
    public HpCtrl hpCtrl;
    public Boss boss;

    public GameObject btn;
    public GameObject effect;
    bool effectActivate = true;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss != null)
        {
            if (!boss.gameObject.activeSelf)
            {
                StartCoroutine(EndScene());
                
            }
        }

        if (PlayerCtrl.health <= 0)
        {
            Reset();

            if (btn != null)
            {
                btn.SetActive(true);
            }
        }
    }

    void Reset()
    {
        PlayerCtrl.bulletLevel = 0;
        PlayerCtrl.maxHealth = PlayerCtrl.startMaxHealth;
        PlayerCtrl.health = PlayerCtrl.maxHealth;
        HpCtrl.BulletDamage = 1;
        UIScript.gameOver = false;
    }

    IEnumerator EndScene()
    {
        if (effectActivate)
        {
            effectActivate = false;
            for (int i = 0; i < 100; i++)
            {
                Effect();
                yield return new WaitForSeconds(0.05f);
            }
        }

        yield return new WaitForSeconds(7f);
        Reset();
        SceneManager.LoadScene("END");
    }

    void Effect()
    {
        float randomFloatX = Random.Range(-15.0f, 15.0f);
        float randomFloatY = Random.Range(-3.0f, 3.0f);
        Vector2 instantiateEffect = new Vector2(5.8f + randomFloatX, 6f + randomFloatY);
        ParticleSystem deadEffect = Instantiate(effect, instantiateEffect, Quaternion.identity).GetComponent<ParticleSystem>();
        deadEffect.gameObject.transform.localScale = Vector3.one * 3;
    }
}
