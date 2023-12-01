using System.Collections.Generic;
using k8s;
using k8s.Models;

namespace DatastoreOperator.KubeController;
public abstract class CustomResource : KubernetesObject
{
    public V1ObjectMeta Metadata { get; set; }
}

public abstract class CustomResource<TSpec, TStatus> : CustomResource
{
    public TSpec Spec { get; set; } = default(TSpec);
    public TStatus Status { get; set; } = default(TStatus);
}

public abstract class CustomResourceList<T> : KubernetesObject where T : CustomResource
{
    public V1ListMeta Metadata { get; set; }
    public List<CustomResource> Items { get; set; }
}

public class CustomResourceDefinition
{
    public string ApiVersion { get; set; } = string.Empty;

    public string PluralName { get; set; } = string.Empty;

    public string Kind { get; set; } = string.Empty;

    public string Namespace { get; set; } = string.Empty;
}