using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{

    [SerializeField] private int dimentionX; 
    [SerializeField] private int dimentionY;
    

    public void SetDimention(int x,int y)
    {
        dimentionX = x;
        dimentionY = y;
    }
    



}
