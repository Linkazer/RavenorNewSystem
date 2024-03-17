using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReferencePicker
{
    public class ReferenceEditor : PropertyAttribute
    {
        public readonly Type type;

        public ReferenceEditor(Type nType)
        {
            type = nType;
        }
    }
}