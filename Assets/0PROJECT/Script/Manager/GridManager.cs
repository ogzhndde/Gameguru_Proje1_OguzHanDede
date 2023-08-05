using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable] //DEFINE ALL GRIDAL SYSTEM VARIABLES IN THIS STRUCT
public struct GridalSystem
{
    public GameObject squarePrefab;
    public int gridSize;
    public float cellSpacing;
}

public class GridManager : InstanceManager<GridManager>
{
    public List<GameObject> AllGrids = new List<GameObject>();

    public GridalSystem gridalSystem;

    void Start()
    {
        //GENERATE DEFAULT GRIDAL SYSTEM IN THE BEGINNING
        EventManager.Broadcast(GameEvent.OnGenerateGrid, gridalSystem.gridSize);
    }

    void GenerateGrid(int gridSize)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        //DEFAULT GENERATION SIZE VALUE OF CAMERA 
        mainCamera.GetComponent<Camera>().orthographicSize = 5f; //KAMERANIN DEFAULT HALINDE GENERATE EDIYORUM

        Vector2 cellSize = gridalSystem.squarePrefab.GetComponent<SpriteRenderer>().bounds.size;
        float gridWidth = (cellSize.x + gridalSystem.cellSpacing) * gridSize - gridalSystem.cellSpacing;
        float gridHeight = (cellSize.y + gridalSystem.cellSpacing) * gridSize - gridalSystem.cellSpacing;

        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        float canvasScale = Mathf.Min(screenHeight / gridHeight, screenWidth / gridWidth);

        transform.localScale = new Vector3(canvasScale, canvasScale, 1f);

        Vector2 startPos = new Vector2(-gridWidth / 2f + cellSize.x / 2f, gridHeight / 2f - cellSize.y / 2f);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //CREATE ALL SQUARES IN GRIDAL SYSTEM
                Vector2 spawnPos = startPos + new Vector2((cellSize.x + gridalSystem.cellSpacing) * x, -(cellSize.y + gridalSystem.cellSpacing) * y);
                GameObject square = Instantiate(gridalSystem.squarePrefab, transform);
                square.transform.localPosition = spawnPos;

                //ASSIGN THE X AND Y VALUES TO THE CREATED SQUARES
                Square spawnedSquare = square.GetComponent<Square>();
                spawnedSquare.xLocation = x;
                spawnedSquare.yLocation = y;

                //RENAME SQUARES ACCORDING TO THEIR X AND Y VALUES
                square.name = x.ToString() + y.ToString();

                //ADD ALL SQUARES IN GRID LIST
                AllGrids.Add(square);
            }
        }

        //RETURN THE CAMERA TO ITS OLD POSITION TO SPACINGS
        mainCamera.GetComponent<Camera>().orthographicSize = 5.15f;
    }

    void ClearAllGrids()
    {
        //CLEAR GRIDAL SYSTEM
        for (int i = 0; i < AllGrids.Count; i++)
        {
            Destroy(AllGrids[i]);
        }
        AllGrids.Clear();
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        //ALL EVENTS USING FOR GRIDAL SYSTEM
        EventManager.AddHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
        EventManager.AddHandler(GameEvent.OnSquareSelected, OnSquareSelected);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
        EventManager.RemoveHandler(GameEvent.OnSquareSelected, OnSquareSelected);
    }

    private void OnGenerateGrid(object value)
    {
        ClearAllGrids();

        //GENERATE GRIDAL SYSTEM ACCORDING TO THE VALUE
        GenerateGrid((int)value);
    }

    private void OnSquareSelected(object value)
    {
        //DEFINE SELECTED SQUARE
        var selectedSquare = (GameObject)value;
        var square = selectedSquare.GetComponent<Square>();

        //PATH LIST WITH ALL SELECTED SQUARES ADDED
        List<GameObject> ClearPath = new List<GameObject>();

        //ADD SELECTED SQUARE TO LIST
        ClearPath.Add(selectedSquare);

        //CHECK AROUND AND ADD ALREADY SELECTED SQUARES TO LIST
        ClearPath = ClearPath.Union(square.ReportNeighborhood()).ToList(); //CEVREDEKI SECILILERI DIREKT LISTEME EKLETIYORUM

        for (int i = 0; i < square.ReportNeighborhood().Count; i++)
        {
            //CHECK OBJECTS WITH SECOND WAVE SELECTED AND ADD TO LIST
            ClearPath = ClearPath.Union(square.ReportNeighborhood()[i].GetComponent<Square>().ReportNeighborhood()).ToList();
        }

        //IF THERE IS A PATH OF AT LEAST 3 SELECTED SQUARE
        if (ClearPath.Count >= 3)
        {
            //CLEAR PROCESS
            foreach (var item in ClearPath)
            {
                EventManager.Broadcast(GameEvent.OnSquareClear, item);
                EventManager.Broadcast(GameEvent.OnPlaySound, "SoundPop");
            }

            //EVENT THAT COUNTS CLEARED PATHS
            EventManager.Broadcast(GameEvent.OnUpdateCount);
        }
    }
}
