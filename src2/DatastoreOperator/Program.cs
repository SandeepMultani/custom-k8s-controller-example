using System;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DatastoreOperator.Models;
using DatastoreOperator.KubeController;

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

        var controller = new Controller<DatastoreCrd>(
            new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile()),
            crd,
            (WatchEventType eventType, DatastoreCrd example) =>
                Console.WriteLine("Event type: {0} for {1}", eventType, example.Metadata.Name));

        var cts = new CancellationTokenSource();
        await controller.StartAsync(cts.Token).ConfigureAwait(false);

        Console.WriteLine("Stopped {0}", nameof(DatastoreOperator));
    }
}