
using UnityEngine;
using UnityEngine.VFX;

public class InWaterState : IState
{
    Hook hook;
    Rigidbody rigid;
    Collider collider;
    VisualEffect waterSplashEffect;
    public InWaterState(Hook pablo)
    {
        hook = pablo;
        rigid = hook.GetComponent<Rigidbody>();
        collider = hook.GetComponent <Collider>();
        waterSplashEffect = hook.GetComponentInChildren<VisualEffect>();
    }

    public override void Process()
    {
        rigid.velocity = Vector3.Lerp(rigid.velocity, Vector3.zero, Time.deltaTime);
    }

    public override void OnAreaEnter(Collider other)
    {
        //if (other.CompareTag("Fish"))
        //{
        //    other.transform.parent = hook.transform;
        //   // bitten = true;
        //    rigid.useGravity = false;
        //    collider.enabled = false;
        //   // StartCoroutine(AttemptBreakFree());
        //    Fish fish = other.gameObject.GetComponent<Fish>();
        //    if (fish) { 
        //        hook.currentFish = fish; 
        //    }

        //    // Bit by fish -> transition to bitten state
        //    CallTransition(State.BITTEN, this);
        //    bitten = true;
        //}
    }

    bool bitten = false;

    public override void Enter()
    {
        hook.baitCollider.enabled = true;
        bitten = false;
        waterSplashEffect.gameObject.SetActive(true);
        waterSplashEffect.Play();
        
    }

    void DeactivateSplash()
    {
        waterSplashEffect.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        hook.baitCollider.enabled = false;
        DeactivateSplash();


        if ( hook.currentFish != null) hook.fishManager.ResetAllTargets(hook.currentFish);
        
    }
}

