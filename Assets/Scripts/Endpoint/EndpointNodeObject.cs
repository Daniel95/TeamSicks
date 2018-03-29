using System.Collections.Generic;

/// <summary>
/// EndpointNodeObject gets all the endpoints in the public static list.
/// </summary>
public class EndpointNodeObject : NodeObject
{
    public static List<EndpointNodeObject> Endpoints = new List<EndpointNodeObject>();

    private void Awake()
    {
        Endpoints.Add(this);
    }
}