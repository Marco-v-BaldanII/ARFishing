using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;

[System.Serializable]
public class FishSpawnInfo
{
    public GameObject fishPrefab;
    [Range(0f, 1f)]
    public float spawnProbability = 0.5f;
    public string fishName;
}

public class FishManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int minFishCount = 5;
    public int maxFishCount = 15;
    public int spawn_zone_width = 10;

    [Header("Boundary Settings")]
    public float despawnBoundary = 15f; 
    public float spawnOffset = 5f;

    [Header("Fish Configuration")]
    public FishSpawnInfo[] fishTypes;
    public List<Fish> fish_list;

    [Header("Depth Layers")]
    public float topLayerY = 0.1f;
    public float middleLayerY = -0.5f;
    public float bottomLayerY = -0.9f;

    [Range(0f, 1f)]
    public float topLayerProbability = 0.33f;
    [Range(0f, 1f)]
    public float middleLayerProbability = 0.33f;

    private Transform playerCamera;

    void Start()
    {
        fish_list = new List<Fish>();
        playerCamera = Camera.main.transform;
        SpawnFish();
    }

    void Update()
    {
        CheckAndReplaceFish();
    }

    private void CheckAndReplaceFish()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;

        List<Fish> fishesToRemove = new List<Fish>();

        foreach (Fish fish in fish_list)
        {
            if (fish != null)
            {
                float distance = Vector3.Distance(fish.transform.position, playerCamera.position);

                if (distance > despawnBoundary)
                {
                    fishesToRemove.Add(fish);
                }
            }
        }

        foreach (Fish fish in fishesToRemove)
        {
            fish_list.Remove(fish);
            Destroy(fish.gameObject);
            SpawnReplacementFish(1);
        }

        if (fish_list.Count < minFishCount)
        {
            int fishToSpawn = minFishCount - fish_list.Count;
            SpawnReplacementFish(fishToSpawn);
        }
    }

    private void SpawnReplacementFish(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject selectedFishPrefab = SelectFishPrefabBasedOnProbability();

            if (selectedFishPrefab != null)
            {
                GameObject fishObject = Instantiate(selectedFishPrefab);
                Fish fishComponent = fishObject.GetComponent<Fish>();
                fish_list.Add(fishComponent);

                Vector3 spawnPosition = CalculateSpawnPosition();

                fishComponent.transform.position = spawnPosition;
                fishComponent.fishManager = this;
            }
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(despawnBoundary - spawnOffset, despawnBoundary);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        Vector3 position = playerCamera.position + offset;
        position.y = SelectDepthLayer();

        return position;
    }

    private void SpawnFish()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;

        NormalizeProbabilities();

        int fishToSpawn = Random.Range(minFishCount, maxFishCount + 1);

        for (int i = 0; i < fishToSpawn; ++i)
        {
            GameObject selectedFishPrefab = SelectFishPrefabBasedOnProbability();

            if (selectedFishPrefab != null)
            {
                GameObject fishObject = Instantiate(selectedFishPrefab);
                Fish fishComponent = fishObject.GetComponent<Fish>();
                fish_list.Add(fishComponent);

                Vector3 pos = Random.insideUnitSphere * spawn_zone_width;
                pos.y = SelectDepthLayer();

                fishComponent.transform.position = pos;
                fishComponent.fishManager = this;
            }
        }
    }

    private GameObject SelectFishPrefabBasedOnProbability()
    {
        if (fishTypes == null || fishTypes.Length == 0)
        {
            Debug.LogError("No fish types configured in FishManager!");
            return null;
        }

        float randomValue = Random.value;
        float cumulativeProbability = 0f;

        foreach (FishSpawnInfo fishInfo in fishTypes)
        {
            cumulativeProbability += fishInfo.spawnProbability;

            if (randomValue <= cumulativeProbability)
            {
                return fishInfo.fishPrefab;
            }
        }

        //fallback to the last fish
        return fishTypes[fishTypes.Length - 1].fishPrefab;
    }

    private float SelectDepthLayer()
    {
        float randomValue = Random.value;

        if (randomValue < topLayerProbability)
        {
            return topLayerY;
        }
        else if (randomValue < topLayerProbability + middleLayerProbability)
        {
            return middleLayerY;
        }
        else
        {
            return bottomLayerY;
        }
    }

    private void NormalizeProbabilities()
    {
        if (fishTypes == null || fishTypes.Length == 0) return;

        float sum = 0f;
        foreach (FishSpawnInfo fishInfo in fishTypes)
        {
            sum += fishInfo.spawnProbability;
        }

        if (Mathf.Abs(sum - 1f) > 0.001f)
        {
            Debug.Log($"Normalizing fish spawn probabilities (sum was {sum})");

            foreach (FishSpawnInfo fishInfo in fishTypes)
            {
                fishInfo.spawnProbability /= sum;
            }
        }

        float depthLayerSum = topLayerProbability + middleLayerProbability;
        if (depthLayerSum > 1f)
        {
            float scale = 1f / depthLayerSum;
            topLayerProbability *= scale;
            middleLayerProbability *= scale;
        }
    }

    public void ResetAllTargets(Fish exception)
    {
        foreach (var fish in fish_list)
        {
            if (fish != null && (exception == null || fish != exception))
            {
                MBT.Blackboard board = fish.GetComponent<MBT.Blackboard>();
                TransformVariable trs = board.GetVariable<TransformVariable>("target");
                trs.Value = null;
                fish.transform.parent = null;
            }
        }
    }

    public void AddFish(int count)
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;

        SpawnReplacementFish(count);
    }
}