using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLineRenderer : MonoBehaviour
{
    public Transform rodTip; // Assign the tip of your fishing rod here
    public Transform hook;    // Assign your hook GameObject here
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (rodTip != null && hook != null)
        {
            lineRenderer.SetPosition(0, rodTip.position);
            lineRenderer.SetPosition(1, hook.position);
        }
    }
}
