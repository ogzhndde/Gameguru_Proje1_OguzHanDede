using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : InstanceManager<GameManager>
{
    public GameData data;
    
    // [Header("Definitons")]


    void Start()
    {
        
    }

    void Update()
    {

    }



    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        // EventManager.AddHandler(GameEvent.OnStart, OnStart);
    }

    private void OnDisable()
    {
        // EventManager.RemoveHandler(GameEvent.OnStart, OnStart);
    }

}
