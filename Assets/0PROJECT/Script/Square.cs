using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Square : SquareAround
{
    [Header("Definitions")]
    [SerializeField] private GameObject OBJ_CrossImage;
    private Animator anim;

    public bool _isSquareSelected = false;


    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        
        yield return new WaitForSeconds(0.1f);

        SetAroundTiles();

        InvokeRepeating(nameof(CrossCheck), 0f, 0.1f);
        InvokeRepeating(nameof(SetAroundSelectedSquares), 0.1f, 0.1f);
    }

    private void OnMouseDown()
    {
        _isSquareSelected = true;
        EventManager.Broadcast(GameEvent.OnSquareSelected, gameObject);
    }

    void CrossCheck()
    {
        anim.SetBool("_isSelected", _isSquareSelected);
    }

    void SetAroundSelectedSquares()
    {
        AroundSelectedSquares = AllAroundSquares.Where(item => item.GetComponent<Square>()._isSquareSelected && item != null).ToList();
    }

    public List<GameObject> ReportNeighborhood()
    {
        return AroundSelectedSquares;
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSquareClear, OnSquareClear);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSquareClear, OnSquareClear);
    }


    private void OnSquareClear(object value)
    {
        if (gameObject == (GameObject)value)
        {
            _isSquareSelected = false;
        }
    }


}
