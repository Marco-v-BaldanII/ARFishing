using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CatchPannel : MonoBehaviour
{
    public static CatchPannel instance;
    public GameObject panel;
    public GameObject FishName;
    public GameObject FishImage;

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

    public void Show(Fish fish)
    {
        switch (fish.GetSpecies())
        {
            case Species.PABLO:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Fish/RichardDiscord");
                FishName.GetComponent<Text>().text = "Pablo";
                break;
            case Species.MAGIKARP:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Fish/GordonMcFish");
                FishName.GetComponent<Text>().text = "Magikarp";
                break;
            case Species.CLOWNNFISH:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Fish/Pablo");
                FishName.GetComponent<Text>().text = "Clownfish";
                break;
            case Species.TROUT:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Fish/RegularGeorge");
                FishName.GetComponent<Text>().text = "Trout";
                break;
            default:
                Debug.Log("You re the fish!");
                break;
        }
        
        panel.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1.0f, 0.6f).SetEase(Ease.OutBounce);
        Invoke("disableImg", 3f);
    }

    private void disableImg()
    {
        panel.SetActive(false);
    }
}
