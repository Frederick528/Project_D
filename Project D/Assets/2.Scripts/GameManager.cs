using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PoolManager poolManager;
    public PlayerCtrl playerCtrl;
    public HpCtrl hpCtrl;
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
        
    }
}
