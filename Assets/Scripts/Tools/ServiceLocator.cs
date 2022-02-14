using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ServiceLocator
{
    private static Dictionary<System.Type, object> _services = new Dictionary<System.Type, object>();

    public static void Register<T>(object service)
    {
        if (_services.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Trying to add service which already exist: {typeof(T)}");
            return;
        }

        Debug.Log($"Was added service: {typeof(T)}");
        _services[typeof(T)] = service;
    }

    public static void Register(object service)
    {
        if (_services.ContainsKey(service.GetType()))
        {
            Debug.LogError($"Trying to add service which already exist: {service.GetType()}");
            return;
        }

        _services[service.GetType()] = service;
    }

    public static void Ungerister<T>()
    {
        if (!_services.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Trying to remove service which did not exist: {typeof(T)}");
            return;
        }

        _services.Remove(typeof(T));
    }

    public static void Ungerister(object service)
    {
        if (!_services.ContainsKey(service.GetType()))
        {
            Debug.LogError($"Trying to remove service which did not exist: {service.GetType()}");
            return;
        }

        _services.Remove(service.GetType());
    }

    public static T Resolve<T>()
    {
        if (!_services.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Trying to resolve service which did not exist: {typeof(T)}");
            return default;
        }

        return (T)_services[typeof(T)];
    }
}