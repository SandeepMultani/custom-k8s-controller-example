using KubeOps.Operator.Webhooks;
using DatastoreOperator.Entities;

namespace DatastoreOperator.Webhooks;

public class DatastoreMutator : IMutationWebhook<V1Datastore>
{
    public AdmissionOperations Operations => AdmissionOperations.Create;

    public MutationResult Create(V1Datastore newEntity, bool dryRun)
    {
        newEntity.Spec.Image = "not foobar";
        return MutationResult.Modified(newEntity);
    }
}