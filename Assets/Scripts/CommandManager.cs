using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    /* ZTK was here
     * Command pattern ın güzel bir örneği olmuş
     * Eline sağlık
     */
    #region Singleton

    private static CommandManager instance;

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

    public static CommandManager GetInstance()
    {
        return instance;
    }

    #endregion


    public List<Move> moveList = new List<Move>();

    private Coroutine myCoroutine;

    private bool isReplay;

    public bool IsReplay { get; set; }

    public void AddMove(Move move)
    {
        moveList.Add(move);
    }

    public void Replay()
    {
        if (myCoroutine!=null)
            StopCoroutine(myCoroutine);

        IsReplay = true;
        myCoroutine = StartCoroutine(ReplayIEnumerator());

    }

    private IEnumerator ReplayIEnumerator()
    {
        for (int i = 0; i < moveList.Count; i++)
        {
            if (moveList[i].moveType == MoveType.Swipe)
            {
                BoardController.GetInstance().MoveToDirection(moveList[i].direction);
            }
            else if (moveList[i].moveType == MoveType.Spawn)
            {
                BoardController.GetInstance().SpawnRandomValues(moveList[i].cell);
            }

            yield return new WaitForSeconds(0.3f);
        }

        IsReplay = false;
    }


    public void Reset()
    {
        IsReplay = false;
        if (myCoroutine!=null)
            StopCoroutine(myCoroutine);

        moveList.Clear();

    }


}
[System.Serializable]
public class Move
{
    public MoveType moveType;
    public Direction direction;
    public CellController cell;

    public Move(MoveType moveType, Direction direction = Direction.DOWN , CellController cell = null)
    {
        this.moveType = moveType;
        this.direction = direction;
        this.cell = cell;

    }
}

public enum MoveType
{
    Swipe = 0,
    Spawn = 1
}
