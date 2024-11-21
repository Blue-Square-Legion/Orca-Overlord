using UnityEngine;

public class WalkStop : MonoBehaviour
{
    public Animator animator;
    bool isWalking=false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
      
                isWalking = true;
         
            animator.enabled = true;
       
    }
}