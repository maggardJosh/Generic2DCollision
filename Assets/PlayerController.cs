using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 1.0f;
    public Vector3 velocity = Vector3.zero;
    public Vector3 moveInput = Vector3.zero;
    public LayerMask CollideMask;
    public BoxCollider bCollider;
    public float JumpStrength = 10;

    // Use this for initialization
    void Start()
    {
        bCollider = GetComponent<BoxCollider>();
    }
    float lastMoveInputY = 0;
    private void FixedUpdate()
    {
        if (moveInput.x != 0)
            velocity.x = Mathf.Lerp(velocity.x, moveInput.x * speed, .5f);
        else
            velocity.x *= .6f;
        if (moveInput.y > 0 && lastMoveInputY <= 0)
            Jump();
        if (velocity.y > 0 && moveInput.y <= 0)
            velocity.y *= .8f;
        TryMove();
        velocity.y = Mathf.Max(velocity.y - GameSettings.Gravity, GameSettings.MinYVel);
        lastMoveInputY = moveInput.y;
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
            float numSections = bCollider.bounds.size.y / GameSettings.TileSize + 1;
            float offsetValue = GameSettings.CollisionOffsetValue;
            Vector3 sideCenter = bCollider.transform.position + bCollider.center + moveVect * (bCollider.bounds.size.x / 2 - offsetValue);

            for (int i = 0; i <= numSections; i++)
            {
                Vector3 tempPos = transform.position + new Vector3(moveVelocity.x, 0);
                //i==0 top side corner
                //i==1 bottom side corner
                float ySectionValue = Mathf.Min(i * GameSettings.TileSize, bCollider.bounds.size.y - offsetValue * 2f);
                Ray ray = new Ray(sideCenter + Vector3.up * (bCollider.bounds.size.y / 2 - offsetValue) + (Vector3.down * ySectionValue), moveVect);
                if (Physics.Raycast(ray, out hitResult, Mathf.Abs(moveVelocity.x) + offsetValue, CollideMask))
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.x), Color.red, 5f);
                    tempPos = new Vector3(hitResult.point.x - bCollider.center.x, tempPos.y) - moveVect * (bCollider.bounds.size.x / 2f);
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
            float numSections = bCollider.bounds.size.x / GameSettings.TileSize + 1;
            float offsetValue = GameSettings.CollisionOffsetValue;
            Vector3 verticalCenter = bCollider.transform.position + bCollider.center + moveVect * (bCollider.bounds.size.y / 2 - offsetValue);

            for (int i = 0; i <= numSections; i++)
            {
                Vector3 tempPos = transform.position + new Vector3(0, moveVelocity.y);
                //i==0 left vertical corner
                //i==1 right vertical corner
                float xSectionValue = Mathf.Min(i * GameSettings.TileSize, bCollider.bounds.size.x - offsetValue * 2f);
                Ray ray = new Ray(verticalCenter + Vector3.left * (bCollider.bounds.size.x / 2 - offsetValue) + (Vector3.right * xSectionValue), moveVect);
                if (Physics.Raycast(ray, out hitResult, Mathf.Abs(moveVelocity.y) + offsetValue, CollideMask))
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * Mathf.Abs(moveVelocity.y), Color.red, 5f);
                    if (velocity.y < 0)
                        velocity.y = 0;
                    tempPos = new Vector3(tempPos.x, hitResult.point.y - bCollider.center.y) - moveVect * (bCollider.bounds.size.y / 2f);
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
        moveInput = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            moveInput = Vector3.left;
        else if (Input.GetKey(KeyCode.D))
            moveInput = Vector3.right;

        if (Input.GetKey(KeyCode.Space))
            moveInput.y = 1;

    }
}
