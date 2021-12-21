using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private float distance;
    [SerializeField] private float startXPos;
    [SerializeField] private float startYPos;

    [SerializeField] private List<CellController> cellList = new List<CellController>();

    private void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        float currentXPos = startXPos;
        float currentYPos = startYPos;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                CellController cell = Instantiate(cellPrefab, new Vector2(currentXPos, currentYPos),Quaternion.identity).GetComponent<CellController>();
                cellList.Add(cell);

                cell.SetDimention(i, j);
                currentXPos += distance;
            }
            currentXPos = startXPos;
            currentYPos -= distance;
        }
    }




}
