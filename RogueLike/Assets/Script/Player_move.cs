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
    public GameObject maincamera;
    Transform camera_transform;
    Vector3 came_posi;

    private BoxCollider2D boxCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        player_trnasform = this.transform;
        posi = player_trnasform.position;
        boxCollider = GetComponent<BoxCollider2D>();

        maincamera = GameObject.Find("Main Camera");
        camera_transform = maincamera.transform;
        came_posi = camera_transform.position;

        came_posi = posi;
        came_posi.z = -11;
        camera_transform.position = came_posi;


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

                came_posi = camera_move(came_posi, -1 * vel.x * Time.deltaTime, 0);

                camera_transform.position = came_posi;

            }
            

        }
        // ‰E‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("x", 1);
            animator.SetInteger("y", 0);
            float dx = (float)(vel.x * Time.deltaTime + 0.5);
            vector2.x += dx;
            if (can_move(posi,vector2,out hit))
            {
                posi.x += vel.x * Time.deltaTime;
                player_trnasform.position = posi;

                came_posi = camera_move(came_posi, vel.x * Time.deltaTime, 0);

                camera_transform.position = came_posi;

            }
            

        }
        // ‘O‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("y", -1);
            animator.SetInteger("x", 0);
            float dy = (float)(vel.y * Time.deltaTime + 0.5);
            vector2.y += dy;
            if(can_move(posi,vector2,out hit))
            {
                posi.y += vel.y * Time.deltaTime;
                player_trnasform.position = posi;
                
                came_posi = camera_move(came_posi, 0, vel.y * Time.deltaTime);
                camera_transform.position = came_posi;
            }
            
        }
        // Œã‚ë‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("y", 1);
            animator.SetInteger("x", 0);
            float dy = (float)(vel.y * Time.deltaTime + 0.5);
            vector2.y -= dy;
            if(can_move(posi,vector2,out hit))
            {
                posi.y -= vel.y * Time.deltaTime;
                player_trnasform.position = posi;

                came_posi = camera_move(came_posi, 0, -1 * vel.y * Time.deltaTime);
                camera_transform.position = came_posi;
            }
               
        }

    }


    Vector3 camera_move(Vector3 camera_posi, float dx,float dy)
    {
        camera_posi.y += dy;
        camera_posi.x += dx;

        return camera_posi;
    }

}

