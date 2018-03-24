using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 1.0f;
    public Vector3 velocity = Vector3.zero;
    public LayerMask CollideMask;

    // Use this for initialization
    void Start() {

    }

    private void FixedUpdate()
    {
        TryMove();
    }

    private void TryMove()
    {
        RaycastHit hitResult = new RaycastHit();
        //Try move left or right first

    }

    private void Jump()
    {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.A))
            velocity = Vector3.left * speed;
        else if (Input.GetKey(KeyCode.D))
            velocity = Vector3.right * speed;

        else velocity = Vector3.zero ;
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
	}
}
