using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Species
{
    CLOWNNFISH,
    MAGIKARP,
    ABYSSAL,
    TROUT,
    PABLO
}

public abstract class Fish : MonoBehaviour
{
    public float movementSpeed;
    public Species species;
    protected int numberOfActions;
    private Camera frustrum;
    public LayerMask mask;

    // UI 
    public GameObject exclamation_mark;
    public GameObject particles;

    public FishManager fishManager;

    public Fish(Species species)
    {
        this.species = species;
    }

    public virtual void Action()
    {
        //Perform action
    }

    public Species GetSpecies()
    {
        return species;
    }

    protected void Awake()
    {
        frustrum = GetComponentInChildren<Camera>();
        frustrum.enabled = false;  // Doesn't override viewport
    }

    protected void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;
        //bool detect = false;
        //Transform detectTrasnform = null;


        //Collider[] colliders = Physics.OverlapSphere(transform.position, frustrum.farClipPlane, mask);
        //Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustrum);
        //foreach (Collider col in colliders)
        //{
        //    if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
        //    {
        //        RaycastHit hit;
        //        Ray ray = new Ray();
        //        ray.origin = transform.position;
        //        ray.direction = (col.transform.position - transform.position).normalized;
        //        ray.origin = ray.GetPoint(frustrum.nearClipPlane);


        //        if (Physics.Raycast(ray, out hit, frustrum.farClipPlane, mask))
        //        {
        //            //targetTransform = hit.collider.transform;

        //            detectTrasnform = hit.collider.transform;
        //            print("Fish has a target");
        //            break;

        //        }
        //    }

        //}
    }

    // Called when the fish attempts to steer the rod
    public void ShowExclamationMark(bool show)
    {
        // TODO , replace with tweening
        if (gameObject)
        {
            exclamation_mark?.SetActive(show);
        }
    }

    public void ShowParticles()
    {
        particles.SetActive(true);
        Invoke("DeactivateParticles", 0.85f);

    }
    private void DeactivateParticles()
    {
        particles.SetActive(false);
    }

    private void OnDestroy()
    {
        fishManager.fish_list.Remove(this);
    }

}
