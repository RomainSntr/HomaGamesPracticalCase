using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int _height;
    public int Height
    {
        get { return _height; }
    }
}
