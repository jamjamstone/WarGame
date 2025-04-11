using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Animator monsterAnimator;
    [SerializeField] Rigidbody monsterBody;
    [SerializeField] Collider monsterCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player.transform;
        transform.LookAt(Vector3.zero);
        GameManager.Instance.resetGame += ResetGame;
        //monsterBody.AddForce(new Vector3(-(transform.position.x - player.position.x), 0, -(transform.position.z - player.position.z)).normalized * 2);
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        //player = GameManager.Instance.player.transform;
        monsterBody.velocity = new Vector3(-(transform.position.x-player.position.x), monsterBody.velocity.y, -(transform.position.z - player.position.z)).normalized*50;
       
        //Debug.Log(monsterBody.velocity);
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("lll");
        if (collision.collider.CompareTag("Stone"))
        {
            //Debug.Log("hit by stone");
            monsterCollider.enabled = false;
            GameManager.Instance.MonsterDead();
            Destroy(gameObject);
        }else if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.PlayerGetHit();
        }
    }

}
