using System;
using k8s;
using DatastoreOperator.Models;
using DatastoreOperator.Helpers;

namespace DatastoreOperator.Handlers;
public class DatastoreHandler
{
    public void Handle(WatchEventType eventType, DatastoreCrd resource)
    {
        Console.WriteLine("<<<<<<<<<<< {0} : {1} >>>>>>>>>>> ", eventType, resource.Metadata.Name);

        switch (eventType)
        {
            case WatchEventType.Added:
                Added(eventType, resource);
                break;

            case WatchEventType.Modified:
                Modified(eventType, resource);
                break;

            case WatchEventType.Deleted:
                Deleted(eventType, resource);
                break;

            default:
                Console.WriteLine("Event type: {0} for {1} not implemented", eventType, resource.Metadata.Name);
                break;
        }

        Console.WriteLine("<<<<<<<<<<< >>>>>>>>>>> ");
    }

    private void Added(WatchEventType eventType, DatastoreCrd resource)
    {
        Console.WriteLine("Add code here to take an action when {0} is added.", resource.Metadata.Name);
    }
    private void Modified(WatchEventType eventType, DatastoreCrd resource)
    {
        Console.WriteLine("Add code here to take an action when {0} is modified.", resource.Metadata.Name);
    }
    private void Deleted(WatchEventType eventType, DatastoreCrd resource)
    {
        Console.WriteLine("Add code here to take an action when {0} is deleted.", resource.Metadata.Name);
    }
}