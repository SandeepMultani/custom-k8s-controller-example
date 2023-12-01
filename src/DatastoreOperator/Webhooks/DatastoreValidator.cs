using KubeOps.Operator.Webhooks;
using DatastoreOperator.Entities;

namespace DatastoreOperator.Webhooks;

public class DatastoreValidator : IValidationWebhook<V1Datastore>
{
    private readonly string[] _validImages = new string[]{
        "mysql:8.2",
        "mysql:8.0",
        "mysql:5.7",
        "mysql:5.6",
        "mysql:5"
    };

    public AdmissionOperations Operations => AdmissionOperations.Create;

    public ValidationResult Create(V1Datastore newEntity, bool dryRun)
    {
        return _validImages.Contains(newEntity.Spec.Image)
        ? ValidationResult.Fail(StatusCodes.Status400BadRequest, "Image is forbidden")
        : ValidationResult.Success();
    }
}