using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
    SCRIPT WORKING ON ALL CREATED SQUARES
*/
public class Square : SquareAround
{
    [Header("Definitions")]
    [SerializeField] private GameObject OBJ_CrossImage;
    [SerializeField] private Animator anim;

    public bool _isSquareSelected = false;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        //DETECT NEIGHBORING SQUARES AROUND IT
        SetAroundTiles();

        InvokeRepeating(nameof(CrossCheck), 0f, 0.1f);
        InvokeRepeating(nameof(SetAroundSelectedSquares), 0.1f, 0.1f);
    }

    private void OnMouseDown()
    {
        //IF SQUARE IS ALREADY SELECTED, RETURN THE METHOD
        if (_isSquareSelected) return;

        _isSquareSelected = true;
        EventManager.Broadcast(GameEvent.OnSquareSelected, gameObject);
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundSelect");
    }

    void CrossCheck()
    {
        //CHECKING IF THE SQUARE IS SELECTED OR NOT, AND PLAY ANIMATION
        anim.SetBool("_isSelected", _isSquareSelected);
    }

    void SetAroundSelectedSquares()
    {
        //SCANNING NEIGHBORS GRIDS AND TAKING SELECTED THINGS TO LIST
        AroundSelectedSquares = AllAroundSquares.Where(item => item.GetComponent<Square>()._isSquareSelected && item != null).ToList();
    }

    public List<GameObject> ReportNeighborhood()
    {
        //DETECTS SELECTED NEIGHBORS AND CALLS WHERE NECESSARY
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
