using k8s;
using k8s.Models;
using DatastoreOperator.KubeController;

namespace DatastoreOperator.Models;
public class DatastoreSpec
{
    public string Image { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public int Port { get; set; }
}

public class DatastoreStatus
{
    public string Status { get; set; } = string.Empty;
}

public class DatastoreCrd : CustomResource<DatastoreSpec, DatastoreStatus>
{
}