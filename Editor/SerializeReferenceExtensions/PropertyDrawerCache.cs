﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace MackySoft.SerializeReferenceExtensions.Editor
{
    public static class PropertyDrawerCache
    {

        private static readonly Dictionary<Type, PropertyDrawer> s_Caches = new Dictionary<Type, PropertyDrawer>();

        public static bool TryGetPropertyDrawer(Type type, out PropertyDrawer drawer)
        {
            if (!s_Caches.TryGetValue(type, out drawer))
            {
                Type drawerType = GetCustomPropertyDrawerType(type);
                drawer = (drawerType != null) ? (PropertyDrawer)Activator.CreateInstance(drawerType) : null;

                s_Caches.Add(type, drawer);
            }
            return drawer != null;
        }

        private static Type GetCustomPropertyDrawerType(Type type)
        {
            Type[] interfaceTypes = type.GetInterfaces();

            TypeCache.TypeCollection types = TypeCache.GetTypesWithAttribute<CustomPropertyDrawer>();
            foreach (Type drawerType in types)
            {
                object[] customPropertyDrawerAttributes = drawerType.GetCustomAttributes(typeof(CustomPropertyDrawer), true);
                foreach (CustomPropertyDrawer customPropertyDrawer in customPropertyDrawerAttributes)
                {
                    FieldInfo field = customPropertyDrawer.GetType().GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                    {
                        Type fieldType = field.GetValue(customPropertyDrawer) as Type;
                        if (fieldType != null)
                        {
                            if (fieldType == type)
                            {
                                return drawerType;
                            }

                            // If the property drawer also allows for being applied to child classes, check if they match
                            FieldInfo useForChildrenField = customPropertyDrawer.GetType().GetField("m_UseForChildren", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (useForChildrenField != null)
                            {
                                object useForChildrenValue = useForChildrenField.GetValue(customPropertyDrawer);
                                if (useForChildrenValue is bool && (bool)useForChildrenValue)
                                {
                                    // Check interfaces
                                    if (Array.Exists(interfaceTypes, interfaceType => interfaceType == fieldType))
                                    {
                                        return drawerType;
                                    }

                                    // Check derived types
                                    Type baseType = type.BaseType;
                                    while (baseType != null)
                                    {
                                        if (baseType == fieldType)
                                        {
                                            return drawerType;
                                        }

                                        baseType = baseType.BaseType;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}