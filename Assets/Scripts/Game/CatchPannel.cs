using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CatchPannel : MonoBehaviour
{
    public static CatchPannel instance;
    public GameObject panel;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);  

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Show()
    {
        panel.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1.2f, 0.6f).OnComplete(() =>  transform.DOScale(1f,0.2f) );
        Invoke("disableImg", 3f);
    }

    private void disableImg()
    {
        panel.SetActive(false);
    }
}
