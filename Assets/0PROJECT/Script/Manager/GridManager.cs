using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
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
        EventManager.Broadcast(GameEvent.OnGenerateGrid, gridalSystem.gridSize); //DEFAULT OLARAK OYUN BASINDA GRID OLUSTURUYOR
    }

    void GenerateGrid(int gridSize)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

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
                //KARELERIMI INSTANTIATE EDIYORUM
                Vector2 spawnPos = startPos + new Vector2((cellSize.x + gridalSystem.cellSpacing) * x, -(cellSize.y + gridalSystem.cellSpacing) * y);
                GameObject square = Instantiate(gridalSystem.squarePrefab, transform);
                square.transform.localPosition = spawnPos;

                //OLUSAN KARELERIN ICERISINE X VE Y DEGERI ATAYIP ISIMLERINI DEGISTIRIYORUM
                Square spawnedSquare = square.GetComponent<Square>();
                spawnedSquare.xLocation = x;
                spawnedSquare.yLocation = y;

                square.name = x.ToString() + y.ToString(); //KAREYI X VE Y'SINE GORE ISIMLENDIRIYORUM

                AllGrids.Add(square);
            }
        }

        mainCamera.GetComponent<Camera>().orthographicSize = 5.15f; // SPACING ICIN KAMERAYI BIR TIK GERI CEKIYORUM
    }

    void ClearAllGrids()
    {
        for (int i = 0; i < AllGrids.Count; i++)
        {
            Destroy(AllGrids[i]);
        }
        AllGrids.Clear();
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
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
        GenerateGrid((int)value);
    }

    private void OnSquareSelected(object value)
    {
        var selectedSquare = (GameObject)value;
        var square = selectedSquare.GetComponent<Square>();

        List<GameObject> ClearPath = new List<GameObject>(); //TEMIZLENMESI GEREKEN YOL

        ClearPath.Add(selectedSquare);

        ClearPath = ClearPath.Union(square.ReportNeighborhood()).ToList(); //CEVREDEKI SECILILERI DIREKT LISTEME EKLETIYORUM

        for (int i = 0; i < square.ReportNeighborhood().Count; i++)
        {
            //IKINCI DALGADAKI YERLERI DE KONTROL ETTIRIP LISTEME EKLIYORUM
            ClearPath = ClearPath.Union(square.ReportNeighborhood()[i].GetComponent<Square>().ReportNeighborhood()).ToList();
        }

        if (ClearPath.Count >= 3) //KOMSU HALINDEKI EN AZ 3 SECILI KARE VARSA 
        {
            foreach (var item in ClearPath)
            {
                EventManager.Broadcast(GameEvent.OnSquareClear, item);
                EventManager.Broadcast(GameEvent.OnPlaySound, "SoundPop");
            }

            EventManager.Broadcast(GameEvent.OnUpdateCount);
        }
    }
}
