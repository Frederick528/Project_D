using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;
    //풀 담당을 하는 리스트
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; //프리펩에 있는 오브젝트 개수 pools에 담기

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();  //pools에 있는 리스트값을 지우고 리스트만 남기기
        }

    }

    public GameObject Get(int index)    //게임 오브젝트를 반환해주는 함수
    {
        GameObject select = null;   //매개변수 지정
                                    // 선택한 툴의 비활성화 된 게임오브젝트 접근
                                    // 발견하면 select 변수에 할당
        foreach (GameObject item in pools[index])    //foreach: 배열, 리스트들의 데이터를 순차적으로 접근하는 반복문
        {
            if (!item.activeSelf)   //아이템이 비활성화인 경우
            {
                select = item;  //select에 item 지정
                select.SetActive(true); //select에 지정된 item을 다시 활성화
                break;  //반복문 종료
            }
        }
        // 못 찾았으면
        // 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);  //새로운 게임 오브젝트 생성(prefabs[index] 오브젝트를 복제하고 게임 오브젝트로 지정 / transform은 그냥 이 위치로 복제한다는 뜻(위치: PoolManager))
            pools[index].Add(select);   //pools[index]에 select 추가
        }

        return select;
    }
}