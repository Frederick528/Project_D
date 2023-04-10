using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ��������� ������ ����
    public GameObject[] prefabs;
    //Ǯ ����� �ϴ� ����Ʈ
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; //�����鿡 �ִ� ������Ʈ ���� pools�� ���

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();  //pools�� �ִ� ����Ʈ���� ����� ����Ʈ�� �����
        }

    }

    public GameObject Get(int index)    //���� ������Ʈ�� ��ȯ���ִ� �Լ�
    {
        GameObject select = null;   //�Ű����� ����
                                    // ������ ���� ��Ȱ��ȭ �� ���ӿ�����Ʈ ����
                                    // �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject item in pools[index])    //foreach: �迭, ����Ʈ���� �����͸� ���������� �����ϴ� �ݺ���
        {
            if (!item.activeSelf)   //�������� ��Ȱ��ȭ�� ���
            {
                select = item;  //select�� item ����
                select.SetActive(true); //select�� ������ item�� �ٽ� Ȱ��ȭ
                break;  //�ݺ��� ����
            }
        }
        // �� ã������
        // ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);  //���ο� ���� ������Ʈ ����(prefabs[index] ������Ʈ�� �����ϰ� ���� ������Ʈ�� ���� / transform�� �׳� �� ��ġ�� �����Ѵٴ� ��(��ġ: PoolManager))
            pools[index].Add(select);   //pools[index]�� select �߰�
        }

        return select;
    }
}