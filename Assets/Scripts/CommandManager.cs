using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public delegate void Moves();
    public Moves moves;

    public List<Moves> moveList = new List<Moves>();

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


    public void AddMove(Moves move)
    {
        moveList.Add(move);
    }


}
