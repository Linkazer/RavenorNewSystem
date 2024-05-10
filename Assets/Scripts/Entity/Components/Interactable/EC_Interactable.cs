using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EC_Interactable : EntityComponent<IEC_InteractableData>
{
    [SerializeField] private EC_Clicable clicableComponent;
    [SerializeField] private bool canBeInteractedWith;

    [SerializeField] private SequenceCutscene interactionCutscene;

    public bool CanBeInteractedWith => canBeInteractedWith;

    public override void SetComponentData(IEC_InteractableData componentData)
    {
        
    }

    protected override void InitializeComponent()
    {
        if(clicableComponent == null)
        {
            Debug.LogError(HoldingEntity + " is Interactable but has no EC_Clicable component");
            return;
        }
    }

    public override void Activate()
    {
        canBeInteractedWith = true;
    }

    public override void Deactivate()
    {
        canBeInteractedWith = false;
    }

    public void PlayInteraction(Action interactionCallback)
    {
        interactionCutscene.StartAction(() => EndInteraction(interactionCallback));
    }

    private void EndInteraction(Action callback)
    {
        callback?.Invoke();
    }

    public override void StartRound()
    {
        
    }

    public override void EndRound()
    {
        
    }
}
