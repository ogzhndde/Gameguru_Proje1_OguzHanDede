using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : InstanceManager<GameManager>
{
    public Texture2D CursorTexture;

    void Start()
    {
        SetCursor();
    }

    public void SetCursor()
    {
        //SET NEW CURSOR IMAGE
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
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
