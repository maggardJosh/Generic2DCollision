using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{

    public GameObject followObj;
    public float tweenValue = .8f;
    // Use this for initialization
    void Start()
    {

    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followObj.transform.position + Vector3.back * 10, tweenValue);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
