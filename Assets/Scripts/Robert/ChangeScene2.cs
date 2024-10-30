using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene2 : MonoBehaviour
{
    private float timer2 = 20f;
    // Start is called before the first frame update
    void Start()
    {
        timer2 = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer2 < 11)
            SceneManager.LoadScene("ThirdPersonMovement");

        timer2 -= 1 * Time.deltaTime;
    }
}
