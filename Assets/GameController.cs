using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private GameObject tileController;
    private int score = -1;
    private GameObject[,] _objectMap;
    private TileController[,] _tileControllerMap;

    [SerializeField] private TMP_Text scoreTextObject;
    [SerializeField] private Vector2Int playerPos = new Vector2Int(8,8);
    [SerializeField] private List<Vector2Int> tailPos = new List<Vector2Int>();
    [SerializeField] private Vector2Int currentMoveDir = new Vector2Int(1,0);
    [SerializeField] private float movementUpdateTimeReq = 1;
    [SerializeField] private int tobiasSpasserNeger = 5;
    
    private float _deltaTimeAddUp = 0;
    private void SetupWorld()
    {
        _objectMap = new GameObject[x,y];
        _tileControllerMap = new TileController[x,y];
        float offsetX = x / 2;
        float offsetZ = y / 2;
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _objectMap[i,j] = Instantiate(tileController, new Vector3(i-offsetX,0,j-offsetZ),Quaternion.identity,transform);
                _tileControllerMap[i, j] = _objectMap[i, j].GetComponent<TileController>();
                _tileControllerMap[i, j].UpdateState(global::TileController.TileStates.Empty);
                //Easy debug
                _objectMap[i, j].name = "LightCubeController At " + i + "," + j;
            }
        }
        
        for (var i = 0; i < tobiasSpasserNeger; i++)
        {
            tailPos.Add(playerPos-new Vector2Int(0,0));
        }

        SpawnFood();

    }

    private void SpawnFood()
    {
        score++;
        scoreTextObject.text = "Score:\n" + score;
        var usableTileControllers = _tileControllerMap.Cast<TileController>().Where(tileController => tileController.ReturnCurrentState() == TileController.TileStates.Empty).ToList();
        usableTileControllers[Random.Range(0,usableTileControllers.Count-1)].UpdateState(TileController.TileStates.Food);
    }
    
    private void UpdateTail(Vector2Int oldPlayerPos)
    {
        tailPos.Add(oldPlayerPos);
        _tileControllerMap[tailPos[0].x,tailPos[0].y].UpdateState(TileController.TileStates.Empty);
        tailPos.RemoveAt(0);
    }

    private void Start()
    {
        SetupWorld();
    }

    private void Update()
    {
        _deltaTimeAddUp += Time.deltaTime;

        #region PlayerInputs
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMoveDir = new Vector2Int(0,1);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentMoveDir = new Vector2Int(-1,0);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentMoveDir = new Vector2Int(1,0);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMoveDir = new Vector2Int(0,-1);
        }
        #endregion

        if (!(_deltaTimeAddUp >= movementUpdateTimeReq)) return;
        playerPos += currentMoveDir;

        #region DeathChecks
        if (playerPos.x > x - 1 || playerPos.x < 0)
        {
            Debug.Log("ded");
            //Todo death stuff
        }
        
        else if (playerPos.y > y - 1 || playerPos.y < 0)
        {
            Debug.Log("ded");
            //Todo death stuff
        }
        else if (_tileControllerMap[playerPos.x,playerPos.y].ReturnCurrentState() == TileController.TileStates.Snake)
        {
            Debug.Log("ded");
            //Todo death stuff
        }
        
        

        #endregion
        
        UpdateTail(playerPos-currentMoveDir);
        if (_tileControllerMap[playerPos.x, playerPos.y].ReturnCurrentState() == TileController.TileStates.Food)
        {
            tailPos.Add(tailPos[0]);
            SpawnFood();
        }
        _tileControllerMap[playerPos.x, playerPos.y].UpdateState(global::TileController.TileStates.Snake);
        _deltaTimeAddUp = 0;
    }
}
