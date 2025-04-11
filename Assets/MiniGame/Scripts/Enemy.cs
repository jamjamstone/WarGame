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
        transform.LookAt(player.position);
        GameManager.Instance.resetGame += ResetGame;
        
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        player = GameManager.Instance.player.transform;
        monsterBody.AddForce((transform.position - player.position).normalized * 2, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Stone"))
        {
            monsterCollider.enabled = false;
            GameManager.Instance.MonsterDead();
            Destroy(gameObject);
        }
    }

}
