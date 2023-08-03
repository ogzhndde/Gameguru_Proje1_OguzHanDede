using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : InstanceManager<UIManager>
{
    GameManager manager;
    GameData data;

    [Header("Definitions")]
    [SerializeField] private TMP_InputField TMP_GridSize;
    [SerializeField] private Button BTN_Rebuild;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        data = manager.data;
    }

    private void Start()
    {


    }

    void Update()
    {

    }

    void ClearGeneratePanel()
    {
        TMP_GridSize.text = "";
    }

    //######################################################### BUTTONS ##############################################################

    void ButtonRebuild()
    {
        if (TMP_GridSize.text == "") return;

        int GridSize = int.Parse(TMP_GridSize.text);
        EventManager.Broadcast(GameEvent.OnGenerateGrid, GridSize);
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        BTN_Rebuild.onClick.AddListener(ButtonRebuild);

        EventManager.AddHandler(GameEvent.OnStart, OnStart);
        EventManager.AddHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
        EventManager.RemoveHandler(GameEvent.OnGenerateGrid, OnGenerateGrid);
    }

    private void OnStart()
    {

    }


    private void OnGenerateGrid(object value)
    {
        ClearGeneratePanel();
    }

}
