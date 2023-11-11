using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMoonTower : MonoBehaviour
{

    private Animator myAnimator; //The animator component
    // Start is called before the first frame update
    void Start()
    {
        //GEt the animator component
        myAnimator = GetComponent<Animator>();

        //check if the Animator component exists 
        if (myAnimator != null)
        {
            //trigger the animation
            myAnimator.SetTrigger("Idle");
        }
        else
        {
            Debug.LogError("Animator component not found");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
