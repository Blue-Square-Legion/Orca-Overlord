using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
 
    private float timer = 20f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 11)
            SceneManager.LoadScene("Tutorial");
       
        timer -=  1*Time.deltaTime;
    }
}
