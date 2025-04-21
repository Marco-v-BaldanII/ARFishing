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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            
            panel.SetActive(true);
            Clownfish fish = new Clownfish();
            fish.species = Species.MAGIKARP;
            Show(fish);
        }
    }

    public void Show(Fish fish)
    {
        switch (fish.GetSpecies())
        {
            case Species.PABLO:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/trout");
                FishName.GetComponent<TextMeshProUGUI>().text = "Pablo";
                break;
            case Species.MAGIKARP:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/magikgord");
                FishName.GetComponent<TextMeshProUGUI>().text = "Magikarp";
                break;
            case Species.CLOWNNFISH:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/clownfish");
                FishName.GetComponent<TextMeshProUGUI>().text = "Clownfish";
                break;
            case Species.ABYSSAL:
                FishImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/lampfish2");
                FishName.GetComponent<TextMeshProUGUI>().text = "Abyssal";
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
