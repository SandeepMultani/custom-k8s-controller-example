using k8s.Models;
using KubeOps.Operator.Entities;

namespace DatastoreOperator.Entities;

[KubernetesEntity(Group = "sandeepmultani.github.io", ApiVersion = "v1", Kind = "Datastore")]
public class V1Datastore : CustomKubernetesEntity<V1Datastore.V1DatastoreSpec, V1Datastore.V1DatastoreStatus>
{
    public class V1DatastoreSpec
    {
        public string Image { get; set; } = string.Empty;
        public string Database { get; set; } = string.Empty;
        public int Port { get; set; }
    }

    public class V1DatastoreStatus
    {
        public string DatastoreStatus { get; set; } = string.Empty;
    }
}