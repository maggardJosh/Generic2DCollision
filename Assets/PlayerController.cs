using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public Vector3 velocity = Vector3.zero;
    public LayerMask CollideMask;
    public BoxCollider bCollider;
    public float JumpStrength = 10;

    // Use this for initialization
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        TryMove();
        velocity.y = Mathf.Max(velocity.y - GameSettings.Gravity, GameSettings.MinYVel);
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
            Vector3 newPos = transform.position + new Vector3(moveVelocity.x, 0);
            float numSections = bCollider.size.y / GameSettings.TileSize + 1;
            float offsetValue = GameSettings.CollisionOffsetValue;
            Vector3 sideCenter = bCollider.transform.position + bCollider.center + moveVect * (bCollider.size.x / 2 - offsetValue);

            for (int i = 0; i <= numSections; i++)
            {
                Vector3 tempPos = transform.position + new Vector3(moveVelocity.x, 0);
                //i==0 top side corner
                //i==1 bottom side corner
                float ySectionValue = Mathf.Min(i * GameSettings.TileSize, bCollider.size.y - offsetValue * 2f);
                Ray ray = new Ray(sideCenter + Vector3.up * (bCollider.size.y / 2 - offsetValue) + (Vector3.down * ySectionValue), moveVect);
                if (Physics.Raycast(ray, out hitResult, Mathf.Abs(moveVelocity.x) + offsetValue, CollideMask))
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.x), Color.red, 5f);
                    tempPos = new Vector3(hitResult.point.x - bCollider.center.x, tempPos.y) - moveVect * (bCollider.size.x / 2f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.x), Color.green, .5f);
                }
                if ((tempPos - transform.position).sqrMagnitude < (newPos - transform.position).sqrMagnitude)
                    newPos = tempPos;   //Basically we ran into something so use this temppos
            }
            transform.position = newPos;
        }

        //Try move up or down now
        if (Mathf.Abs(moveVelocity.y) > 0)
        {
            Vector3 moveVect = Vector3.up;
            if (moveVelocity.y < 0)
                moveVect = Vector3.down;
            Vector3 newPos = transform.position + new Vector3(0, moveVelocity.y);
            float numSections = bCollider.size.x / GameSettings.TileSize + 1;
            float offsetValue = GameSettings.CollisionOffsetValue;
            Vector3 verticalCenter = bCollider.transform.position + bCollider.center + moveVect * (bCollider.size.y / 2 - offsetValue);

            for (int i = 0; i <= numSections; i++)
            {
                Vector3 tempPos = transform.position + new Vector3(0, moveVelocity.y);
                //i==0 left vertical corner
                //i==1 right vertical corner
                float xSectionValue = Mathf.Min(i * GameSettings.TileSize, bCollider.size.x - offsetValue * 2f);
                Ray ray = new Ray(verticalCenter + Vector3.left * (bCollider.size.x / 2 - offsetValue) + (Vector3.right * xSectionValue), moveVect);
                if (Physics.Raycast(ray, out hitResult, Mathf.Abs(moveVelocity.y) + offsetValue, CollideMask))
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.y), Color.red, 5f);
                    if (velocity.y < 0)
                        velocity.y = 0;
                    tempPos = new Vector3(tempPos.x, hitResult.point.y - bCollider.center.y) - moveVect * (bCollider.size.y / 2f);
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.y), Color.green, .5f);
                }
                if ((tempPos - transform.position).sqrMagnitude < (newPos - transform.position).sqrMagnitude)
                    newPos = tempPos;   //Basically we ran into something so use this temppos
            }
            transform.position = newPos;
        }
    }

    private void Jump()
    {
        velocity.y = JumpStrength;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = 0;

        if (Input.GetKey(KeyCode.A))
            velocity += Vector3.left * speed;
        else if (Input.GetKey(KeyCode.D))
            velocity += Vector3.right * speed;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        
    }
}
