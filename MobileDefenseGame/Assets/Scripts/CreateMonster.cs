using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour {

    public List<GameObject> respawnSpotList;

    public GameObject monster1Prefab;
    public GameObject monster2Prefab;

    private GameObject monsterPrefab;

    private int spawnCount = 0;
    private IEnumerator coroutine; // coroutine 변수 선언

    void Start () {
        
        monsterPrefab = monster1Prefab;
        coroutine = process(); // coroutine란 변수를 process 코루틴으로 지정
        StartCoroutine(coroutine);
	}

    void Create()
    {
        int index = Random.Range(0, 4);
        GameObject respawnSpot = respawnSpotList[index];
        Instantiate(monsterPrefab, respawnSpot.transform.position, Quaternion.identity);
        GameManager.instance.monsterAddCount++;
        spawnCount += 1;
    }

    IEnumerator process()
    {
        while (true)
        {
            if (GameManager.instance.round > GameManager.instance.totalRound) StopCoroutine(coroutine);
            if(spawnCount < GameManager.instance.spawnNumber) // 몬스터의 출몰 한계
            {
                Create();
            }
            if(spawnCount == GameManager.instance.spawnNumber &&
                GameObject.FindGameObjectWithTag("Monster") == null) 
                // 소환된 몬스터 수가 클리어 목표 몬스터 수와 같고, 현재 몬스터가 존재하지 않는다면
            {
                if(GameManager.instance.totalRound == GameManager.instance.round)
                //현재 스테이지가 전체 중 마지막 스테이지라면
                {
                    GameManager.instance.gameClear(); // 게임 클리어
                    GameManager.instance.round += 1; // 코루틴 실행을 위한 라운드 증가
                }
                else
                {
                    GameManager.instance.clearRound();
                    spawnCount = 0;

                    if (GameManager.instance.round == 4)
                    {
                        monsterPrefab = monster2Prefab;
                        GameManager.instance.spawnTime = 2.0f;
                        GameManager.instance.spawnNumber = 10;
                    }
                }
            }
            if(spawnCount == 0) yield return new WaitForSeconds(GameManager.instance.roundReadyTime);
            // 새 라운드가 시작됐다면 라운드 준비 시간 만큼 잠시 대기
            else yield return new WaitForSeconds(GameManager.instance.spawnTime);
            // 그 이후부터는 몬스터 소환 쿨타임만큼 대기
        }
    }
}
