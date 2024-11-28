using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFlipAnimator : MonoBehaviour
{
    public int maxPlays;

    private Animator animator;
    private int playCount = 0;
    public bool flipReady = false;
    private bool shakeReload = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = transform.GetComponent<Animator>();

        //this should get the children triggers of the boats that the script is attached to
        Transform ShakeTriggerTransform = this.transform.Find("ShakeTrigger");
        Transform BoatFlipRightTriggerTransform = this.transform.Find("BoatFlipTriggerRight");
        Transform BoatFlipLeftTriggerTransform = this.transform.Find("BoatFlipTriggerLeft");

        GameObject ShakeTrigger = ShakeTriggerTransform.gameObject;
        GameObject BoatFlipTriggerRight = BoatFlipRightTriggerTransform.gameObject;
        GameObject BoatFlipTriggerLeft = BoatFlipLeftTriggerTransform.gameObject;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        //shake the boat if the player runs into the shake trigger
        //trying to make this for loop interchangeable with each boat and then set flipReady to true 
        //so the next time the player enters a trigger it will flip the baot
        if (other.CompareTag("Player") && gameObject.name == "ShakeTrigger" && flipReady == false)
        {
            Debug.Log("hit the trigger");
            for (int i = playCount; i < maxPlays; i++)
            {
                animator.SetTrigger("Shake");
                float animDuration = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(animDuration);

            }
            flipReady = true;
        }

        //flip boat to either side after shaking the boat
        if (other.CompareTag("Player") && gameObject.name == "BoatFlipTriggerRight" && flipReady == true)
        {
            animator.SetTrigger("flipRight");
        }

        if (other.CompareTag("Player") && gameObject.name == "BoatFlipTriggerLeft" && flipReady == true)
        {
            animator.SetTrigger("flipLeft");
        }
    }

    IEnumerator PlayAnimationLoop()
    {
        // Play the animation for a fixed number of times (e.g., 5 times)
        for (int i = playCount; i < maxPlays; i++)
        {
            if (shakeReload)  // Ensure we're still inside the trigger zone
            {
                animator.SetTrigger("Shake");  // Trigger the animation
                playCount++;  // Increment the play count

                // Wait for the animation to finish (this is optional based on your timing)
                float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(animationLength);  // Wait for the animation length

                Debug.Log("Animation played " + playCount + " times.");
            }
            else
            {
                // If player leaves trigger before 5 times, stop playing
                Debug.Log("Player left the trigger zone.");
                break;
            }
        }
    }
}
