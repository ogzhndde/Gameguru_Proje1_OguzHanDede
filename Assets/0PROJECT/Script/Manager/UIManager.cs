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
    [SerializeField] private TMP_InputField TMPField_GridSize;
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
        TMPField_GridSize.text = "";
    }

    //######################################################### BUTTONS ##############################################################

    void ButtonRebuild()
    {
        if (TMPField_GridSize.text == "")
        {
            TMPField_GridSize.GetComponent<Animator>().SetTrigger("_fieldEmpty");
            return;
        }


        int GridSize = int.Parse(TMPField_GridSize.text);
        EventManager.Broadcast(GameEvent.OnGenerateGrid, GridSize);

        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundClick");
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
