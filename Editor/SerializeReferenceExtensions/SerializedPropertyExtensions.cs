﻿using System.Collections.Generic;
using UnityEditor;

namespace MackySoft.SerializeReferenceExtensions.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static IEnumerable<SerializedProperty> GetChildProperties(this SerializedProperty parent, int depth = 1)
        {
            parent = parent.Copy();

            int depthOfParent = parent.depth;
            System.Collections.IEnumerator enumerator = parent.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is not SerializedProperty childProperty)
                {
                    continue;
                }
                if (childProperty.depth > (depthOfParent + depth))
                {
                    continue;
                }
                yield return childProperty.Copy();
            }
        }
    }
}