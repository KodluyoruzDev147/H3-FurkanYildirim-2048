using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellController : MonoBehaviour
{

    [SerializeField] private int dimentionX; 
    [SerializeField] private int dimentionY;

    [SerializeField] private int currentValue;
    [SerializeField] TextMeshPro textValue;

    [SerializeField] private bool isEmpty = true;

    public int DimensionX { get { return dimentionX; } }
    public int DimensionY { get { return dimentionY; } }

    public void SetDimention(int x,int y)
    {
        dimentionX = x;
        dimentionY = y;
    }
    

    public void SetValue(int value)
    {
        currentValue = value;
        textValue.text = value.ToString();
    }

    public int GetValue()
    {
        return currentValue;
    }

    public bool IsEmpty()
    {
        return isEmpty;
    }

    public void SetEmpty(bool value)
    {
        isEmpty = value;

        BoardController.GetInstance().AddEmptyList(this, value);

        if (value)
        {
            currentValue = 0;
            textValue.text = string.Empty;
        }
    }

    public void LevelUp()
    {
        currentValue *= 2;
        textValue.text = currentValue.ToString();
    }


}
