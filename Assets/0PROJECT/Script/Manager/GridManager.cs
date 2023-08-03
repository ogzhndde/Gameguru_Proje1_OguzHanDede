using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridManager : InstanceManager<GridManager>
{
    GameManager manager;

    [SerializeField] private List<GameObject> AllGrids = new List<GameObject>();

    [Space]
    [Header("Gridal System Properties")]
    public GameObject squarePrefab;
    public int gridSize = 3;
    public float cellSpacing = 0.1f;


    void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        EventManager.Broadcast(GameEvent.OnGenerateGrid, 4); //DEFAULT OLARAK OYUN BASINDA 4 GRID OLUSTURUYOR
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.Broadcast(GameEvent.OnGenerateGrid, gridSize);
        }
    }

    void GenerateGrid(int gridSize)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        mainCamera.GetComponent<Camera>().orthographicSize = 5f; //KAMERANIN DEFAULT HALINDE GENERATE EDIYORUM

        Vector2 cellSize = squarePrefab.GetComponent<SpriteRenderer>().bounds.size;
        float gridWidth = (cellSize.x + cellSpacing) * gridSize - cellSpacing;
        float gridHeight = (cellSize.y + cellSpacing) * gridSize - cellSpacing;

        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        float canvasScale = Mathf.Min(screenHeight / gridHeight, screenWidth / gridWidth);

        transform.localScale = new Vector3(canvasScale, canvasScale, 1f);

        Vector2 startPos = new Vector2(-gridWidth / 2f + cellSize.x / 2f, gridHeight / 2f - cellSize.y / 2f);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2 spawnPos = startPos + new Vector2((cellSize.x + cellSpacing) * x, -(cellSize.y + cellSpacing) * y);
                GameObject gridCell = Instantiate(squarePrefab, transform);
                gridCell.transform.localPosition = spawnPos;

                AllGrids.Add(gridCell);
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
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }

    private void OnGenerateGrid(object value)
    {
        ClearAllGrids();
        GenerateGrid((int)value);
    }
}
