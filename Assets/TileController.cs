using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileController : MonoBehaviour
{ 
    [SerializeField]private GameObject emptyCubePrefab;
    [SerializeField]private GameObject foodCubePrefab;
    [SerializeField]private GameObject snakeCubePrefab;
    public enum TileStates{Empty,Food,Snake}

    [SerializeField]private TileStates tileState;

    private GameObject _currentSpawnedCube = null;
    // Start is called before the first frame update
    public TileStates ReturnCurrentState()
    {
        return tileState;
    }
    
    public void UpdateState(TileStates newState)
    {
        tileState = newState;
        if (_currentSpawnedCube != null)
        {
            Destroy(_currentSpawnedCube);
        }
        switch (newState)
        {
            case TileStates.Empty:
                _currentSpawnedCube = Instantiate(emptyCubePrefab, transform);
                break;
            case TileStates.Food:
                _currentSpawnedCube = Instantiate(foodCubePrefab, transform);
                break;
            case TileStates.Snake:
                _currentSpawnedCube = Instantiate(snakeCubePrefab, transform);
                break;
        }
    }
}
