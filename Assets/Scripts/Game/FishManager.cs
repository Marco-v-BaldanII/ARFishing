using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public int numFish;
    public int spawn_zone_width;
    public GameObject[] fish_prefabs; // change to array of fiish
    public List<GameObject> fish_list;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFish; ++i)
        {
            fish_list.Add (Instantiate(fish_prefabs[Random.Range(0, fish_prefabs.Length)]) );
            Vector3 pos = Random.insideUnitSphere * spawn_zone_width;
            pos.y = Random.Range(-0.4f, -1f);
            fish_list[fish_list.Count - 1].transform.position = pos;
        }
    }



}
