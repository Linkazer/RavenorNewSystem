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

#if UNITY_EDITOR
    public override void EDITOR_SetComponents(IEC_RendererData componentData)
    {
        if (animationHandler != null)
        {
            DestroyImmediate(animationHandler);
        }

        if (componentData != null)
        {
            animationHandler = Instantiate(componentData.AnimationHandler, transform);
            animationHandler.transform.localPosition = Vector3.zero;
        }
    }
#endif

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
