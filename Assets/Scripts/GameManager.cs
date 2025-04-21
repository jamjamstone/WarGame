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
    public PlayerManager playerManager;

    public List<GameObject> unitList=new List<GameObject>();//프리팹을 저장하는 방식으로
    public List<GameObject> strategeList=new List<GameObject>();//프리팹을 저장하는 방식으로

    public GameObject unitSpawnPoint;

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

    public void AddUnitToList(GameObject unit)
    {
        unitList.Add(unit);
    }
    public void AddStrategeToList(GameObject stratege)
    {
        strategeList.Add(stratege);
    }


}
