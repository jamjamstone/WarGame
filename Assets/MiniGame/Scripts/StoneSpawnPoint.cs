using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject stonePrefab;
    
    
    public void StoneSpawnStart()
    {
        StartCoroutine(SpawnStones());
    }
    public void StoneSpawnEnd()
    {
        StopCoroutine(SpawnStones());
    }
    IEnumerator SpawnStones()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            var stone=Instantiate(stonePrefab, transform.position, transform.rotation);
            Destroy(stone,10f);
        }
    }
 
    
}
