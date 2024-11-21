using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFlipAnimator : MonoBehaviour
{

    private Animator animator;


    // Start is called before the first frame update
    void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.name == "BoatFlipTriggerRight")
        {
            animator.SetTrigger("flipRight");
        }
        
        if (other.CompareTag("Player") && gameObject.name == "BoatFlipTriggerLeft")
        {
            animator.SetTrigger("flipLeft");
        }
    }
}
