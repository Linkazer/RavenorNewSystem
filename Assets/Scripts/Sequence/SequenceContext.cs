using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceContext
{
    public Entity interactionEntity; // L'entit� ayant lanc� la S�quence. Peut �tre vide si la s�quence est lanc� automatiquement.
    public Sequence currentSequence;

    public bool IsCutscene => currentSequence is SequenceCutscene;

    public SequenceContext(Sequence sequence)
    {
        currentSequence = sequence;
    }

    public SequenceContext(Sequence sequence, Entity entity)
    {
        currentSequence = sequence;
        interactionEntity = entity;
    }
}
