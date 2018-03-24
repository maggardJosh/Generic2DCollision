using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public Vector3 velocity = Vector3.zero;
    public LayerMask CollideMask;
    public BoxCollider bCollider;

    // Use this for initialization
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        TryMove();
    }

    private void TryMove()
    {
        RaycastHit hitResult = new RaycastHit();
        Vector3 moveVelocity = velocity * Time.fixedDeltaTime;
        //Try move left or right first
        if (Mathf.Abs(moveVelocity.x) > 0)
        {
            Vector3 moveVect = Vector3.right;
            if (moveVelocity.x < 0)
                moveVect = Vector3.left;
            Vector3 rightCenter = bCollider.transform.position + bCollider.center + (moveVect * bCollider.size.x / 2.1f);
            Vector3 newPos = transform.position + new Vector3(moveVelocity.x, 0);
            for (int i = 0; i < 2; i++)
            {
                Vector3 tempPos = transform.position + new Vector3(moveVelocity.x, 0);
                //i==0 top right corner
                //i==1 bottom right corner
                Ray ray = new Ray(rightCenter + (i == 0 ? Vector3.up : Vector3.down) * bCollider.size.y / 2.1f, moveVect);
                if (Physics.Raycast(ray, out hitResult, Mathf.Abs(moveVelocity.x), CollideMask))
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.x), Color.red, 5f);
                    tempPos = new Vector3(hitResult.point.x, tempPos.y) - moveVect * (bCollider.size.x / 2f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.x), Color.green, .5f);
                }
                Debug.Log((tempPos - transform.position).sqrMagnitude.ToString() + (newPos - transform.position).sqrMagnitude);
                if ((tempPos - transform.position).sqrMagnitude < (newPos - transform.position).sqrMagnitude)
                    newPos = tempPos;   //Basically we ran into something so use this temppos
            }
            transform.position = newPos;
        }
    }

    private void Jump()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            velocity = Vector3.left * speed;
        else if (Input.GetKey(KeyCode.D))
            velocity = Vector3.right * speed;

        else velocity = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
}
