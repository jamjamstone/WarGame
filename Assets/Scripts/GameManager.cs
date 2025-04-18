using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    public InGameUIManager uiManager;
    public TurnManager turnManager;
    public UnitManager unitManager;
    public StrategeManager strategeManager;
    public static GameManager Instance
    {
        get {
              if (gameManager == null)
            { 
                return null;
            }
            else
            {
                return gameManager;
            }

        }
        
    }
    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
