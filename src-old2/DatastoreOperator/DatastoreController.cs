using System;
using System.Threading;
using System.Threading.Tasks;
using System.Dynamic;
using k8s.Models;
using k8s.Operators;
using Microsoft.Extensions.Logging;
using k8s;

namespace DatastoreOperator
{
    public class DatastoreController : Controller<DatastoreResource>
    {        
        public DatastoreController(OperatorConfiguration configuration, IKubernetes client, ILoggerFactory loggerFactory = null) : base(configuration, client, loggerFactory)
        {
        }

        protected override async Task AddOrModifyAsync(DatastoreResource resource, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Begin AddOrModify {resource}");
            
            try
            {
                // Simulate event handling
                await Task.Delay(5000, cancellationToken);

                // Update the resource
                resource.Metadata.EnsureAnnotations()["custom-key"] = DateTime.UtcNow.ToString("s");
                await UpdateResourceAsync(resource, cancellationToken);
                
                // Update the status
                if (resource.Status?.actualProperty != resource.Spec.desiredProperty)
                {
                    if (resource.Status == null)
                    {
                        resource.Status = new ExpandoObject();
                    }
                    resource.Status.actualProperty = resource.Spec.desiredProperty;
                    await UpdateStatusAsync(resource, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Interrupted! Trying to shutdown gracefully...");

                // Simulate a blocking operation
                Task.Delay(3000).Wait();
            }

            _logger.LogInformation($"End AddOrModify {resource}");
        }

        protected override async Task DeleteAsync(DatastoreResource resource, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Begin Delete {resource}");
            
            try
            {
                // Simulate event handling
                await Task.Delay(5000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Interrupted! Trying to shutdown gracefully...");

                // Simulate a blocking operation
                Task.Delay(3000).Wait();
            }

            _logger.LogInformation($"End Delete {resource}");
        }
    }
}