using k8s.Models;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Controller.Results;
using KubeOps.Operator.Finalizer;
using KubeOps.Operator.Rbac;
using DatastoreOperator.Entities;
using DatastoreOperator.Finalizer;
using DatastoreOperator.Helpers;
using KubeOps.KubernetesClient;
using System.Text;

namespace DatastoreOperator.Controller;

[EntityRbac(typeof(V1Datastore), Verbs = RbacVerb.All)]
public class DatastoreController : IResourceController<V1Datastore>
{
    private readonly IKubernetesClient _client;
    private readonly ILogger<DatastoreController> _logger;
    private readonly IFinalizerManager<V1Datastore> _finalizerManager;

    public DatastoreController(IKubernetesClient client, ILogger<DatastoreController> logger, IFinalizerManager<V1Datastore> finalizerManager)
    {
        _client = client;
        _logger = logger;
        _finalizerManager = finalizerManager;
    }

    public async Task<ResourceControllerResult?> ReconcileAsync(V1Datastore entity)
    {
        _logger.LogInformation($"entity {entity.Name()} called {nameof(ReconcileAsync)}.");
        
        var ns = entity.Namespace();
        var secretName = entity.Name() + "-root-secret";
        byte[] port = Encoding.ASCII.GetBytes(entity.Spec.Port.ToString());
        byte[] database = Encoding.ASCII.GetBytes(entity.Spec.Database.ToString());
        byte[] randomPwd = Encoding.ASCII.GetBytes(RandomPasswordGenerator.Generate());

        var secret = await _client.Get<V1Secret>(secretName, ns);
        if (secret == null) {
            secret = new V1Secret {
               Metadata = new V1ObjectMeta {
                Name = secretName,
                NamespaceProperty = ns,
                OwnerReferences = new List<V1OwnerReference> {
                    new V1OwnerReference {
                        ApiVersion = entity.ApiVersion,
                        Kind = entity.Kind,
                        Name = entity.Name(),
                        Uid = entity.Uid()
                    }
                }
               },

               Data = new Dictionary<string, byte[]> {
                { Constants.DatabaseRootPasswordKey, randomPwd },
                { Constants.DatabasePortKey, port },
                { Constants.DatabaseDatabaseKey, database }
               }
            };

            await _client.Create(secret);
            _logger.LogInformation($"created secret {secretName} for entity {entity.Name()}.");
        }
        else {
            if (secret.Data[Constants.DatabasePortKey] != port || secret.Data[Constants.DatabaseDatabaseKey] != database) {
                _logger.LogInformation($"updating secret {secretName} and entity status for entity {entity.Name()}.");               

                secret.Data[Constants.DatabasePortKey] = port;
                secret.Data[Constants.DatabaseDatabaseKey] = database;
                await _client.Update(secret);

                entity.Status.DatastoreStatus = "updated";
                await _client.UpdateStatus(entity);
            }
        }

        await _finalizerManager.RegisterFinalizerAsync<DatastoreFinalizer>(entity);
        return ResourceControllerResult.RequeueEvent(TimeSpan.FromSeconds(10));
    }

    public Task StatusModifiedAsync(V1Datastore entity)
    {
        _logger.LogInformation($"entity {entity.Name()} called {nameof(StatusModifiedAsync)}.");

        return Task.CompletedTask;
    }

    public Task DeletedAsync(V1Datastore entity)
    {
        _logger.LogInformation($"entity {entity.Name()} called {nameof(DeletedAsync)}.");

        return Task.CompletedTask;
    }
}