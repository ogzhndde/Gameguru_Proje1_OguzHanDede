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

        EventManager.AddHandler(GameEvent.OnUpdateCount, OnUpdateCount);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnUpdateCount, OnUpdateCount);
    }

    private void OnUpdateCount()
    {
        MatchCount++;
    }

}
