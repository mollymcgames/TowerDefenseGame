using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{

    private Rigidbody2D rb;

    private Animator anim;
    [SerializeField] private float speed; //The speed of the knight
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Get the rigidbody component 
        anim = GetComponent<Animator>(); //Get the animator component
    }

    void Update()
    {
        // rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime; //Get the input from the keyboard and move the knight


        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * speed;   //don't need to use normalized if you don't want to

        anim.SetFloat("moveX", rb.velocity.x); //Set the moveX parameter in the animator
        anim.SetFloat("moveY", rb.velocity.y); //Set the moveY parameter in the animator

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal")); //Set the lastMoveX parameter in the animator
            anim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical")); //Set the lastMoveY parameter in the animator
        }
    }
}
