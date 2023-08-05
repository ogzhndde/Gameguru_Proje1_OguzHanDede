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

    [Header("Definitions")]
    [SerializeField] private TMP_InputField TMPField_GridSize;
    [SerializeField] private TextMeshProUGUI TMP_MatchCount;
    [SerializeField] private Button BTN_Rebuild;

    [SerializeField] int MatchCount = 0;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();

        InvokeRepeating(nameof(TextCheck), 0.1f, 0.1f);
    }

    private void Update()
    {
        //GENERATE GRID WITH THE RETURN KEY, NOT JUST WITH THE BUTTON
        KeyboardGridGenerate();
    }

    void TextCheck()
    {
        TMP_MatchCount.text = "Match Count: " + MatchCount;
    }

    void KeyboardGridGenerate()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ButtonRebuild();
        }
    }

    //######################################################### BUTTONS ##############################################################
    public void ButtonRebuild()
    {
        //IF THE INPUT FIELD IS EMPTY, RETURN METHOD
        if (TMPField_GridSize.text == "")
        {
            //WARNING ANIMATION FOR EMPTY FIELD
            TMPField_GridSize.GetComponent<Animator>().SetTrigger("_fieldEmpty");
            return;
        }

        //TAKE VALUE IN INPUT FIELD AND GENERATE GRID BY VALUE
        int GridSize = int.Parse(TMPField_GridSize.text);
        EventManager.Broadcast(GameEvent.OnGenerateGrid, GridSize);

        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundClick");
    }

    //########################################    EVENTS    ###################################################################
    private void OnEnable()
    {
        //DEFINE BUTTON METHOD
        BTN_Rebuild.onClick.AddListener(ButtonRebuild);

        //EVENTS THAT USING IN UI
        EventManager.AddHandler(GameEvent.OnUpdateCount, OnUpdateCount);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUpdateCount, OnUpdateCount);
    }

    private void OnUpdateCount()
    {
        //UPDATE MATCH COUNT
        MatchCount++;
    }
}
