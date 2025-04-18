using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using MBT;

public class FishManager : MonoBehaviour
{
    public int numFish;
    public int spawn_zone_width;
    public GameObject[] fish_prefabs; // change to array of fiish
    public List<Fish> fish_list;
    public float Ypos = 1;

    // Start is called before the first frame update
    void Start()
    {
        fish_list = new List<Fish>();
        for (int i = 0; i < numFish; ++i)
        {
            fish_list.Add (Instantiate(fish_prefabs[Random.Range(0, fish_prefabs.Length)]).GetComponent<Fish>() );
            Vector3 pos = Random.insideUnitSphere * spawn_zone_width;

            int id = Random.Range(0, 3);

            if(id== 0) { Ypos = 0.1f; }
            else if(id == 1) { Ypos = -0.9f; }
            else { Ypos = -0.5f; }
            pos.y = Ypos;

            fish_list[fish_list.Count - 1].transform.position = pos;
            fish_list[fish_list.Count - 1].fishManager = this;
        }
    }

    public void ResetAllTargets(Fish exception)
    {
        foreach (var fish in fish_list)
        {
            if (exception == null || fish != exception)
            {
                MBT.Blackboard board = fish.GetComponent<MBT.Blackboard>();
                TransformVariable trs = board.GetVariable<TransformVariable>("target");
                trs.Value = null;
                fish.transform.parent = null;
            }
        }
    }

}
