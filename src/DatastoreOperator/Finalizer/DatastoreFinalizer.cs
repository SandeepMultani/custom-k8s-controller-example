using k8s.Models;
using KubeOps.Operator.Finalizer;
using DatastoreOperator.Entities;

namespace DatastoreOperator.Finalizer;

public class DatastoreFinalizer : IResourceFinalizer<V1Datastore>
{
    private readonly ILogger<DatastoreFinalizer> _logger;

    public DatastoreFinalizer(ILogger<DatastoreFinalizer> logger)
    {
        _logger = logger;
    }

    public Task FinalizeAsync(V1Datastore entity)
    {
        _logger.LogInformation($"entity {entity.Name()} called {nameof(FinalizeAsync)}.");

        return Task.CompletedTask;
    }
}