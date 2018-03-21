using System.Collections.Generic;

public class EndpointNodeObject : NodeObject
{

    public static List<EndpointNodeObject> Endpoints = new List<EndpointNodeObject>();

    private void Awake()
    {
        Endpoints.Add(this);
    }

}
