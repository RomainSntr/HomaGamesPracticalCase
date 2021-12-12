using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private UIManager UI;
    private int _gemCount = 0;

    public static StoreManager _instance;
    public static StoreManager Instance
    {
        get { return _instance; }
    }

    public int GemCount
    {
        get { return _gemCount; }
        set { _gemCount = value; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //UI.SetGemCount(GemCount);
    }
}