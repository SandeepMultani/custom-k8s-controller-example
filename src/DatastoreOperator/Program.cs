using System;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DatastoreOperator.Models;
using DatastoreOperator.KubeController;
using DatastoreOperator.Handlers;

namespace DatastoreOperator;

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Starting {0}", nameof(DatastoreOperator));

        var crd = new CustomResourceDefinition()
        {
            ApiVersion = "sandeepmultani.github.io/v1",
            PluralName = "datastores",
            Kind = "Datastore",
            Namespace = "default"
        };

        var datastoreHandler = new DatastoreHandler();
        var controller = new Controller<DatastoreCrd>(
            new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile()),
            //new Kubernetes(KubernetesClientConfiguration.InClusterConfig()),
            crd,
            (WatchEventType eventType, DatastoreCrd resource) =>
                datastoreHandler.Handle(eventType, resource));

        var cts = new CancellationTokenSource();
        await controller.StartAsync(cts.Token).ConfigureAwait(false);

        Console.WriteLine("Stopped {0}", nameof(DatastoreOperator));
    }
}