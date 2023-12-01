using k8s.Operators;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DatastoreOperator
{
    [CustomResourceDefinition("sandeepmultani.github.io", "v1", "datastores")]
    public class DatastoreResource : DynamicCustomResource
    {
        public override string ToString()
        {
            return $"{Metadata.NamespaceProperty}/{Metadata.Name} (gen: {Metadata.Generation}), Spec: {JsonConvert.SerializeObject(Spec)} Status: {JsonConvert.SerializeObject(Status ?? new object())}";
        }
    }
}