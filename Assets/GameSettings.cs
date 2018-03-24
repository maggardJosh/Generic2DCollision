using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    private static GameSettings _instance;
    public static GameSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameSettings>();
            return _instance;
        }
    }
    public float tileHeight = 1;
    public static float TileSize { get { return Instance.tileHeight; } }

    public float collisionOffsetValue = .01f;
    public static float CollisionOffsetValue { get { return Instance.collisionOffsetValue; } }

    public float gravity = .1f;
    public static float Gravity { get { return Instance.gravity; } }

    public float minYVel = -3f;
    public static float MinYVel { get { return Instance.minYVel; } }

    // Use this for initialization
    void Start()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
