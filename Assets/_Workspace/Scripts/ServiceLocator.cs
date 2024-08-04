using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        if (services.ContainsKey(type))
        {
            Debug.LogWarning($"Service of type {type} is already registered.");
            return;
        }
        services[type] = service;
        //Debug.Log($"Service of type {type} is registered."); 
    }

    public static T GetService<T>()
    {
        var type = typeof(T);
        if (services.ContainsKey(type))
        {
            return (T)services[type];
        }
        Debug.LogError($"Service of type {type} is not registered.");
        return default;
    }

    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        if (services.ContainsKey(type))
        {
            services.Remove(type);
        }
        else
        {
            Debug.LogWarning($"Service of type {type} is not registered.");
        }
    }

    public static void ClearAllServices()
    {
        services.Clear();
    }
}
