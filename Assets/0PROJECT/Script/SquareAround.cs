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

        SearchAroundLoop(ref UpSquare, xOffset: 0, yOffset: -1);
        SearchAroundLoop(ref BottomSquare, xOffset: 0, yOffset: 1);
        SearchAroundLoop(ref RightSquare, xOffset: 1, yOffset: 0);
        SearchAroundLoop(ref LeftSquare, xOffset: -1, yOffset: 0);

        ListUpdate();
    }

    void SearchAroundLoop(ref GameObject selectedSquare, int xOffset, int yOffset)
    {
        List<GameObject> AllGrids = GridManager.Instance.AllGrids;
        int AllGridsCount = AllGrids.Count;

        for (int i = 0; i < AllGridsCount; i++)
        {
            // if (i == GridManager.Instance.gridSize + 1) break; //GRIDIN KOSELERINE ULASTIYSA DONGUYU KIRIYORUM

            if (AllGrids[i].name == (xLocation + xOffset).ToString() + (yLocation + yOffset).ToString()) // ISIMLENDIRMELERDEN CEVRESINDEKI KARELERI ALGILATIYORUM
            {
                selectedSquare = AllGrids[i];
                break;
            }
        }
    }

    void ListUpdate()
    {
        if (UpSquare != null) AllAroundSquares.Add(UpSquare);
        if (BottomSquare != null) AllAroundSquares.Add(BottomSquare);
        if (RightSquare != null) AllAroundSquares.Add(RightSquare);
        if (LeftSquare != null) AllAroundSquares.Add(LeftSquare);
    }
}
