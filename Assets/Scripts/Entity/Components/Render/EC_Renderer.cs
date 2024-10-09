using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_Renderer : EntityComponent<IEC_RendererData>
{
    [SerializeField] private AnimationHandler animationHandler;

    public AnimationHandler AnimHandler => animationHandler;

    public void SetOrientation(Vector2 direction)
    {
        //TODO 3D : Gérer les rotations des personnages
        /*if (direction.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }

        animationHandler.SetOrientation(new Vector2(Mathf.Abs(direction.x), direction.y));*/
    }

    public override void SetComponentData(IEC_RendererData componentData)
    {
        if(animationHandler != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(animationHandler.gameObject);
#else
            Destroy(animationHandler.gameObject);
#endif
        }

        animationHandler = Instantiate(componentData.AnimationHandler, transform);
        animationHandler.transform.localPosition = Vector3.zero;
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
        animationHandler.PlayAnimation("Idle", null);
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
