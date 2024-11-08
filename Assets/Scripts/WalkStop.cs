using UnityEngine;

public class WalkStop : MonoBehaviour
{
    public Animator animator;
    bool isWalking=false;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled=false;
    }

    void Update()
    {
        if (animator != null)
        {
           if(MoveFace.movementDirection != Vector3.zero)
                isWalking = true;
           else animator.enabled = false;
        }
        if (isWalking && MoveFace.movementDirection != Vector3.zero)
            animator.enabled = true;
        if(!isWalking &&MoveFace.movementDirection != Vector3.zero)
            animator.enabled = false;    
    }
}