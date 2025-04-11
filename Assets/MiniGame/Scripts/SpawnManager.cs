using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject spawnPointGroup;
    [SerializeField] StoneSpawnPoint stoneSpawnPoint;
    private List<GameObject> spawnList = new List<GameObject>();
    [SerializeField] GameObject monsterPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.resetGame += ResetGame;
        for (int i=0;i<spawnPointGroup.transform.childCount;i++)
        {
            spawnList.Add(spawnPointGroup.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    
    public void StartSpawn()
    {
        StartCoroutine(SpawnMonster());
    }
    IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            int i=Random.Range(0,spawnList.Count);
            Instantiate(monsterPrefab, spawnList[i].transform.position, spawnList[i].transform.rotation);
        }
    }
    public void PlayerDead()
    {
        stoneSpawnPoint.StoneSpawnEnd();
        StopCoroutine(SpawnMonster());
    }
    public void ResetGame()
    {
        stoneSpawnPoint.StoneSpawnStart();
        StartSpawn();
    }
}
