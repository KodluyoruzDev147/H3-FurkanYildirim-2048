using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    #region Singleton

    private static BoardController instance;

    private void Awake()
    {
        if (instance != null)
            return;

        instance = this;
    }

    public static BoardController GetInstance()
    {
        return instance;
    }

    #endregion

    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private float distance;
    [SerializeField] private float startXPos;
    [SerializeField] private float startYPos;

    [SerializeField] private int dimensionXCount;
    [SerializeField] private int dimensionYCount;

    [SerializeField] private List<CellController> cellList = new List<CellController>();

    private Dictionary<Vector2, CellController> cellDic = new Dictionary<Vector2, CellController>();

    private void Start()
    {
        CreateBoard();
        SpawnRandomValues();
    }

    private void CreateBoard()
    {
        float currentXPos = startXPos;
        float currentYPos = startYPos;

        for (int i = 0; i < dimensionXCount; i++)
        {
            for (int j = 0; j < dimensionYCount; j++)
            {
                CellController cell = Instantiate(cellPrefab, new Vector2(currentXPos, currentYPos),Quaternion.identity).GetComponent<CellController>();
                cellList.Add(cell);
                cellDic.Add(new Vector2(i, j), cell);

                cell.SetDimention(i, j);
                currentXPos += distance;
            }
            currentXPos = startXPos;
            currentYPos -= distance;
        }
    }

    public CellController GetCellController(Vector2 dimension)
    {
        return cellDic[dimension];
    }

    private void SpawnRandomValues()
    {
        List<CellController> fakeCellList = new List<CellController>(cellList);

        CellController randomCell1 = fakeCellList[Random.Range(0, fakeCellList.Count)];
        fakeCellList.Remove(randomCell1);
        CellController randomCell2 = fakeCellList[Random.Range(0, fakeCellList.Count)];

        randomCell1.SetValue(2);
        randomCell1.SetEmpty(false);
        
        randomCell2.SetValue(2);
        randomCell2.SetEmpty(false);
    }

    public void MoveToDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:

                for (int i = 1; i < dimensionXCount; i++)
                {
                    for (int j = 0; j < dimensionYCount; j++)
                    {
                        CellController cell = cellDic[new Vector2(i, j)];

                        if (cell.IsEmpty())
                            continue;

                        for (int k = 1; k < dimensionXCount; k++)
                        {
                            if (i-k < 0)
                                break;
                            else
                            {
                                CellController cell2 = cellDic[new Vector2(i - k, j)];

                                if (cell2.IsEmpty())
                                {
                                    cell2.SetValue(cell.GetValue());
                                    cell2.SetEmpty(false);
                                    cell.SetEmpty(true);
                                    cell = cell2;
                                }
                                else
                                {
                                    if (cell.GetValue() == cell2.GetValue())
                                    {
                                        cell2.LevelUp();
                                        cell.SetEmpty(true);
                                    }
                                    else
                                        break;

                                }
                            }
                        
                        }
                    }
                }

                break;
            case Direction.DOWN:

                break;
            case Direction.RIGHT:

                break;
            case Direction.LEFT:

                break;
            default:
                break;
        }

    }

}

public enum Direction
{
    UP = 0,
    DOWN = 1,
    RIGHT = 2,
    LEFT = 3
}

