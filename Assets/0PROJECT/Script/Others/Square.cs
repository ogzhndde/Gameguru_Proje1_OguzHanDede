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
        if(_isSquareSelected) return;

        _isSquareSelected = true;
        EventManager.Broadcast(GameEvent.OnSquareSelected, gameObject);
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundSelect");
    }

    void CrossCheck()
    {
        //CARPININ AKTIF PASIFLIGINI KONTROL EDIYOR
        anim.SetBool("_isSelected", _isSquareSelected);
    }

    void SetAroundSelectedSquares()
    {
        //KOMSU OLAN HER GRIDDEN SECILI OLANLARI DIGER LISTEYE AKTARIYOR
        AroundSelectedSquares = AllAroundSquares.Where(item => item.GetComponent<Square>()._isSquareSelected && item != null).ToList();
    }

    public List<GameObject> ReportNeighborhood()
    {
        //ISTEDIGIM YERLERDE CAGIRABILMEK ICIN LISTE DONDUREN METHOD
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
