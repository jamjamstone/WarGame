using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] RawImage winnerImage;
    [SerializeField] RawImage looseImage;
    [SerializeField] GameObject resetButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] TMP_Text playerHP;
    public float score;
    public int playerHPNumber;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.resetGame += ResetGame;
        
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        scoreText.text=score.ToString();
        playerHP.text = playerHPNumber.ToString();
    }
    public void PlayerDead()
    {
        playerHP.text = playerHPNumber.ToString();
        winnerImage.enabled = false;
        looseImage.enabled = true;
        resetButton.SetActive(true);
        quitButton.SetActive(true);
    }
    public void ResetGame()
    {
        score = 0;
        winnerImage.enabled = false;
        looseImage.enabled = false;
        resetButton.SetActive(false);
        quitButton.SetActive(false);
        playerHPNumber = GameManager.Instance.Hp;
        playerHP.text= playerHPNumber.ToString();
        scoreText.text = score.ToString();
    }
    public void PlayerWin()
    {
        winnerImage.enabled = true;
        looseImage.enabled = false;
        resetButton.SetActive(true);
        quitButton.SetActive(true);
    }
}
