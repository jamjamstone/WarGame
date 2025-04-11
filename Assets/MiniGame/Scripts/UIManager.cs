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
    [SerializeField] Button resetButton;
    [SerializeField] Button quitButton;

    public float score;
    

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
    }
    public void PlayerDead()
    {
        winnerImage.enabled = false;
        looseImage.enabled = true;
        resetButton.enabled = true;
        quitButton.enabled = true;
    }
    public void ResetGame()
    {
        score = 0;
        winnerImage.enabled = false;
        looseImage.enabled = false;
        resetButton.enabled = false;
        quitButton.enabled = false; 
        scoreText.text = score.ToString();
    }
    public void PlayerWin()
    {
        winnerImage.enabled = true;
        looseImage.enabled = false;
        resetButton.enabled = true;
        quitButton.enabled = true;
    }
}
