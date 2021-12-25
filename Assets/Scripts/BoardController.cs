using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    #region Singleton

    private static BoardController instance;

    /* ZTK was here
     * Singleton sadece static instance demek değildir.
     * Singleton olabilmesi için başka bir obje oluşumunu tamamen engellemelidir.
     */

    /* Furkan was here
     * Teşekkür ederim kopyayı destroy etmeyi unutmuşum.
     */

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(this);

            return;
        }
  
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

    private Dictionary<Vector2, CellController> cellDic = new Dictionary<Vector2, CellController>();

    [SerializeField] private List<CellController> emptyCellList = new List<CellController>();

    [SerializeField] private List<CellController> cellList = new List<CellController>();

    private void Start()
    {
        CreateBoard();
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

                cellDic.Add(new Vector2(i, j), cell);
                AddEmptyList(cell,true);
                cellList.Add(cell);

                cell.SetDimention(i, j);
                currentXPos += distance;
            }
            currentXPos = startXPos;
            currentYPos -= distance;
        }

        SpawnRandomValues(emptyCellList[Random.Range(0, emptyCellList.Count)]);
    }

    
    public void Replay()
    {
        emptyCellList.Clear();

        for (int i = 0; i < cellList.Count; i++)
        {
            cellList[i].SetValue(0);
            cellList[i].SetEmpty(true);
        }

        CommandManager.GetInstance().Replay();
    }

    public void Restart()
    {
        CommandManager.GetInstance().Reset();

        for (int i = 0; i < cellList.Count; i++)
        {
            Destroy(cellList[i].gameObject);
        }

        cellDic.Clear();
        cellList.Clear();
        emptyCellList.Clear();
        CreateBoard();
    }


    public CellController GetCellController(Vector2 dimension)
    {
        return cellDic[dimension];
    }


    public void SpawnRandomValues(CellController cell)
    {
        if (cell != null)
        {
            if (!CommandManager.GetInstance().IsReplay)
                CommandManager.GetInstance().AddMove(new Move(MoveType.Spawn, cell: cell));

            cell.SetValue(2);
            cell.SetEmpty(false);
            emptyCellList.Remove(cell);

        }
            

        if (emptyCellList.Count == 0)
            GameOverControl();
    }

    public void SpawnCommandValues(List<CellController> cells)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].SetValue(2);
            cells[i].SetEmpty(false);
            emptyCellList.Remove(cells[i]);
        }
    }

    

    private void GameOverControl()
    {
        bool gameEnd = false;

        foreach (var item in cellDic)
        {

            CellController cell = item.Value;
            CellController cell2;

            if (cell.DimensionY + 1 < dimensionYCount)
            {
                cell2 = cellDic[new Vector2(cell.DimensionX, cell.DimensionY + 1)];
                gameEnd = IsEqualCellValues(cell, cell2);

                if (gameEnd)
                    break;
            }

            if (cell.DimensionY - 1 >= 0)
            {
                cell2 = cellDic[new Vector2(cell.DimensionX, cell.DimensionY - 1)];
                gameEnd = IsEqualCellValues(cell, cell2);

                if (gameEnd)
                    break;
            }

            if (cell.DimensionX + 1< dimensionXCount)
            {
                cell2 = cellDic[new Vector2(cell.DimensionX + 1, cell.DimensionY)];
                gameEnd = IsEqualCellValues(cell, cell2);

                if (gameEnd)
                    break;
            }

            if (cell.DimensionX - 1 >= 0)
            {
                cell2 = cellDic[new Vector2(cell.DimensionX - 1, cell.DimensionY)];
                gameEnd = IsEqualCellValues(cell, cell2);

                if (gameEnd)
                    break;
            }

        }

        if (!gameEnd)
            Debug.Log("Game Over");

    }
    private bool IsEqualCellValues(CellController cell1,CellController cell2)
    {
        return Equals(cell1.GetValue(), cell2.GetValue()) ? true : false;
    }

    public void AddEmptyList(CellController cell,bool isEmpty)
    {
        if (isEmpty)
            emptyCellList.Add(cell);
        else
            emptyCellList.Remove(cell);

    }

    public void MoveToDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:

                UpSideControl();

                break;
            case Direction.DOWN:

                DownSideControl();

                break;
            case Direction.RIGHT:

                RightSideControl();

                break;
            case Direction.LEFT:

                LeftSideControl();

                break;
            default:
                break;
        }

        if (emptyCellList.Count > 0 && !CommandManager.GetInstance().IsReplay)
            SpawnRandomValues(emptyCellList[Random.Range(0, emptyCellList.Count)]);


    }

    private void LeftSideControl()
    {
        for (int i = 1; i < dimensionYCount; i++)
        {
            for (int j = 0; j < dimensionYCount; j++)
            {
                CellController cell = cellDic[new Vector2(j, i)];

                if (cell.IsEmpty())
                    continue;

                for (int k = 1; k < dimensionXCount; k++)
                {
                    if (i - k < 0)
                        break;
                    else
                    {
                        CellController cell2 = cellDic[new Vector2(j, i - k)];

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
    }

    private void RightSideControl()
    {
        for (int i = dimensionYCount - 2; i >= 0; i--)
        {
            for (int j = 0; j < dimensionYCount; j++)
            {
                CellController cell = cellDic[new Vector2(j, i)];

                if (cell.IsEmpty())
                    continue;

                for (int k = 1; k < dimensionXCount; k++)
                {
                    if (i + k > dimensionYCount - 1)
                        break;
                    else
                    {
                        CellController cell2 = cellDic[new Vector2(j, i + k)];

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
    }

    private void DownSideControl()
    {
        for (int i = dimensionXCount - 2; i >= 0; i--)
        {
            for (int j = 0; j < dimensionYCount; j++)
            {
                CellController cell = cellDic[new Vector2(i, j)];

                if (cell.IsEmpty())
                    continue;

                for (int k = 1; k < dimensionXCount; k++)
                {
                    if (i + k > dimensionXCount - 1)
                        break;
                    else
                    {
                        CellController cell2 = cellDic[new Vector2(i + k, j)];

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
    }

    private void UpSideControl()
    {
        for (int i = 1; i < dimensionXCount; i++)
        {
            for (int j = 0; j < dimensionYCount; j++)
            {
                CellController cell = cellDic[new Vector2(i, j)];

                if (cell.IsEmpty())
                    continue;

                for (int k = 1; k < dimensionXCount; k++)
                {

                    if (i - k < 0)
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
    }


}

public enum Direction
{
    UP = 0,
    DOWN = 1,
    RIGHT = 2,
    LEFT = 3
}

