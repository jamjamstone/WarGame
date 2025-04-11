using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void ResetGame();
    public event ResetGame resetGame;
    public delegate void ScoreChange(float changedScore);
    public event ScoreChange scoreChange;
    [SerializeField] SpawnManager spawnManager;
    [SerializeField] UIManager uiManager;
    
    public GameObject player;
    private int HP;


    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        resetGame.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RestartGame()
    {
        HP = 3;
        resetGame.Invoke();
        //uiManager.ResetGame();
        //spawnManager.ResetGame();
        //SceneManager.LoadScene("MiniGameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
        if (UnityEditor.EditorApplication.isPlaying==true)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    public void PlayerGetHit()
    {
        HP -= 1;
        if (HP <= 0) 
        {
            HP = 0;
            uiManager.PlayerDead();
            spawnManager.PlayerDead();
        }
    }
    public void MonsterDead()
    {
        uiManager.score += 100;
        
        if (uiManager.score >= 10000)
        {
            PlayerWin();
        }
    }
    public void PlayerWin()
    {
        uiManager.PlayerWin();
        spawnManager.PlayerDead();
    }
}
