using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    private Animator animator;
    private Vector3 vel = new Vector3(3,3,0);
    GameObject Player;
    Vector3 posi;
    Transform player_trnasform;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player_trnasform = this.transform;
        
        posi = player_trnasform.position;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

   public bool can_move(Vector2 start,Vector2 end,out RaycastHit2D hit)
    {
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Move()
    {
        Vector2 vector2 = posi;
        RaycastHit2D hit;
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("x", -1);
            animator.SetInteger("y", 0);
            float dx = (float)(vel.x * Time.deltaTime + 0.5);
            vector2.x -=  dx;
            if (can_move(posi,vector2,out hit))
            {
                posi.x -= vel.x * Time.deltaTime;

                player_trnasform.position = posi;
            }
            

        }
        // ‰E‚ÉˆÚ“®
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("x", 1);
            animator.SetInteger("y", 0);
            float dx = (float)(vel.x * Time.deltaTime + 0.5);
            vector2.x += dx;
            if (can_move(posi,vector2,out hit))
            {
                posi.x += vel.x * Time.deltaTime;
                player_trnasform.position = posi;
            }
            

        }
        // ‘O‚ÉˆÚ“®
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("y", -1);
            animator.SetInteger("x", 0);
            float dy = (float)(vel.y * Time.deltaTime + 0.5);
            vector2.y += dy;
            if(can_move(posi,vector2,out hit))
            {
                posi.y += vel.y * Time.deltaTime;
                player_trnasform.position = posi;
            }
            
        }
        // Œã‚ë‚ÉˆÚ“®
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("y", 1);
            animator.SetInteger("x", 0);
            float dy = (float)(vel.y * Time.deltaTime + 0.5);
            vector2.y -= dy;
            if(can_move(posi,vector2,out hit))
            {
                posi.y -= vel.y * Time.deltaTime;
                player_trnasform.position = posi;
            }
               
        }

    }
}
