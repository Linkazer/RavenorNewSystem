using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_Renderer : EntityComponent<IEC_RendererData>
{
    [SerializeField] private AnimationHandler animationHandler;

    public AnimationHandler AnimHandler => animationHandler;

    public override void SetComponentData(IEC_RendererData componentData)
    {
        if(animationHandler == null)
        {
            animationHandler = Instantiate(componentData.AnimationHandler, transform);
            animationHandler.transform.localPosition = Vector3.zero;
        }
    }

    protected override void InitializeComponent()
    {
       
    }

    public override void Activate()
    {
        animationHandler.PlayAnimation("Idle");
    }

    public override void Deactivate()
    {
        animationHandler.EndAnimation();
    }

    public override void StartRound()
    {
        
    }

    public override void EndRound()
    {
        
    }
}
