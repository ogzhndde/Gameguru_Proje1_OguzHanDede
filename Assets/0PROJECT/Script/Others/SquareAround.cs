using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SquareAround : MonoBehaviour
{
    public int xLocation;
    public int yLocation;

    public List<GameObject> AllAroundSquares;
    public List<GameObject> AroundSelectedSquares;
    public GameObject UpSquare;
    public GameObject BottomSquare;
    public GameObject RightSquare;
    public GameObject LeftSquare;

    public void SetAroundTiles()
    {
        //TUM KOMSU KARELERI BU KARE ICERISINE BASLANGICTA TANIMLIYOR
        SearchAroundLoop(ref UpSquare, xOffset: 0, yOffset: -1);
        SearchAroundLoop(ref BottomSquare, xOffset: 0, yOffset: 1);
        SearchAroundLoop(ref RightSquare, xOffset: 1, yOffset: 0);
        SearchAroundLoop(ref LeftSquare, xOffset: -1, yOffset: 0);

        ListUpdate();
    }

    void SearchAroundLoop(ref GameObject selectedSquare, int xOffset, int yOffset)
    {
        List<GameObject> AllGrids = GridManager.Instance.AllGrids;

        foreach (var grid in AllGrids)
        {
            int targetX = xLocation + xOffset;
            int targetY = yLocation + yOffset;
            string targetName = targetX.ToString() + targetY.ToString(); //X VE YDEN HEDEF KAREYI TESPIT EDYIORUM

            if (grid.name == targetName) //CEVRESINDE HEDEF KAREYI BULUYO
            {
                selectedSquare = grid;
                break;
            }
        }
    }

    void ListUpdate()
    {
        //CEVREMDEKI TUM GRIDLERI BIR LISTEYE ALIYORUM
        if (UpSquare != null) AllAroundSquares.Add(UpSquare);
        if (BottomSquare != null) AllAroundSquares.Add(BottomSquare);
        if (RightSquare != null) AllAroundSquares.Add(RightSquare);
        if (LeftSquare != null) AllAroundSquares.Add(LeftSquare);
    }
}
