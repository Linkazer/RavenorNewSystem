using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceContext
{
    public Entity interactionEntity; // L'entité ayant lancé la Séquence. Peut être vide si la séquence est lancé automatiquement.
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
