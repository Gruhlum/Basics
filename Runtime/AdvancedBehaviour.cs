using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public abstract class AdvancedBehaviour : MonoBehaviour
    {
        protected virtual void Reset()
        {
            if (gameObject.name.Contains("GameObject"))
            {
                gameObject.name = GetType().Name;
            }

            foreach (var field in GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.IsPrivate)
                {
                    SerializeField attribute = field.GetCustomAttribute<SerializeField>();
                    if (attribute == null)
                    {
                        continue;
                    }
                }

                if (!typeof(Component).IsAssignableFrom(field.FieldType))
                {
                    continue;
                }
                var result = GetComponent(field.FieldType);
                if (result != null)
                {
                    field.SetValue(this, result);
                }
                if (field.Name.Contains("Controller"))
                {
                    field.SetValue(this, FindObjectOfType(field.FieldType));
                }
            }
        }
    }
}