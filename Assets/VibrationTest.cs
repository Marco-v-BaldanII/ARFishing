using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VibrationTest : MonoBehaviour
{
    float timer = 1.5f;
    public GameObject[] activeObjects;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SceneTransition", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            AndroidVibration.Vibrate(400);
            timer = 1.5f;
        }
    }

    void SceneTransition()
    {
        for (int i = 0; i < activeObjects.Length; i++)
        {
            activeObjects[i].gameObject.SetActive(true);
        }
        //SceneManager.LoadScene(1);
    }
}
