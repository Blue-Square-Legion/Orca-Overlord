using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeScene3 : MonoBehaviour
{
    private float timer3 = 20f;
    // Start is called before the first frame update
    void Start()
    {
        timer3 = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer3 < 11)
            SceneManager.LoadScene("Finish");

        timer3 -= 1 * Time.deltaTime;
    }
}
