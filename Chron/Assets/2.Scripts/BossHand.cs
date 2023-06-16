using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour
{
    //public static bool lookAtTarget = true;
    public GameObject target;
    public Enums.Directions useSide = Enums.Directions.Down;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            // StopAllCoroutines();
            return;
        }
        // ���� Ÿ��(�÷��̾�)�� �ٶ�
        Utils.SetAxisTowards(useSide, transform, target.transform.position - transform.position);
    }
}
