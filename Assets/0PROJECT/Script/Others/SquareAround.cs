using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    THE ABSTRACT CLASS I WAS SCANNING AROUND THE SQUARE AND MADE THE NECESSARY APPOINTMENTS
*/
public abstract class SquareAround : MonoBehaviour
{
    //SQUARE X AND Y LOCATION
    public int xLocation;
    public int yLocation;

    //LISTS
    public List<GameObject> AllAroundSquares;
    public List<GameObject> AroundSelectedSquares;

    //VARIABLES OF AROUND SQUARES
    public GameObject UpSquare;
    public GameObject BottomSquare;
    public GameObject RightSquare;
    public GameObject LeftSquare;

    public void SetAroundTiles()
    {
        //ALL NEIGHBORING SQUARES DEFINE ITS INSIDE
        SearchAroundLoop(ref UpSquare, xOffset: 0, yOffset: -1);
        SearchAroundLoop(ref BottomSquare, xOffset: 0, yOffset: 1);
        SearchAroundLoop(ref RightSquare, xOffset: 1, yOffset: 0);
        SearchAroundLoop(ref LeftSquare, xOffset: -1, yOffset: 0);

        //UPDATE LIST THAT INCLUDE AROUND LIST
        ListUpdate();
    }

    void SearchAroundLoop(ref GameObject selectedSquare, int xOffset, int yOffset)
    {
        //RELIST ALL GRIDS FROM GRID MANAGER
        List<GameObject> AllGrids = GridManager.Instance.AllGrids;

        foreach (var grid in AllGrids)
        {
            //IT CONTROLS UP OR DOWN, RIGHT OR LEFT. IT DOES THIS WITH ITS OWN VALUE PLUS OFFSET VALUE
            int targetX = xLocation + xOffset;
            int targetY = yLocation + yOffset;

            //FIND THE NAME OF THE TARGET SQUARE
            string targetName = targetX.ToString() + targetY.ToString();

            //IF THE TARGET SQUARE EXISTS, IT DETECTS AND DEFINES THE SQUARE AROUND IT
            if (grid.name == targetName)
            {
                selectedSquare = grid;
                break;
            }
        }
    }

    void ListUpdate()
    {
        //UPDATE LIST THAT INCLUDE AROUND LIST
        if (UpSquare != null) AllAroundSquares.Add(UpSquare);
        if (BottomSquare != null) AllAroundSquares.Add(BottomSquare);
        if (RightSquare != null) AllAroundSquares.Add(RightSquare);
        if (LeftSquare != null) AllAroundSquares.Add(LeftSquare);
    }
}
