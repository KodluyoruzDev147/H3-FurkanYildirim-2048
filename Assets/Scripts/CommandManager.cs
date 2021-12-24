using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{

    public List<Direction> moveList = new List<Direction>();
    public List<CellController> spawnedCells = new List<CellController>();

    #region Singleton

    private static CommandManager instance;

    private void Awake()
    {
        if (instance != null)
            return;

        instance = this;
    }

    public static CommandManager GetInstance()
    {
        return instance;
    }

    #endregion


    public void AddMove(Direction move)
    {
        moveList.Add(move);
    }

    public void AddSpawnedCell(CellController cell)
    {
        spawnedCells.Add(cell);
    }

    public void Replay()
    {
        StartCoroutine(ReplayIEnumerator());
    }

    private IEnumerator ReplayIEnumerator()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            if (i == 0)
                BoardController.GetInstance().SpawnRandomValues(2);
            else
                BoardController.GetInstance().SpawnRandomValues(1);

            BoardController.GetInstance().MoveToDirection(moveList[i]);
            yield return new WaitForSeconds(0.8f);
        }
    }

    
}
