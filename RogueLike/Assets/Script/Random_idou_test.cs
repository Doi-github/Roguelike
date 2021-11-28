using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Random_idou_test : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("x", -1);
            animator.SetInteger("y", 0);
        }
        // ‰E‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("x", 1);
            animator.SetInteger("y", 0);
        }
        // ‘O‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("y", -1);
            animator.SetInteger("x", 0);
        }
        // Œã‚ë‚ÉˆÚ“®
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetInteger("y", 1);
            animator.SetInteger("x", 0);
        }
    }
}
