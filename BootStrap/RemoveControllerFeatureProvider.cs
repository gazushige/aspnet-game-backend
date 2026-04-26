using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MyApi.Bootstrap
{
    public class RemoveControllerFeatureProvider
        : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Type _typeToRemove;

        public RemoveControllerFeatureProvider(Type typeToRemove)
        {
            _typeToRemove = typeToRemove;
        }

        public void PopulateFeature(
            IEnumerable<ApplicationPart> parts,
            ControllerFeature feature)
        {
            var controller = feature.Controllers
                .FirstOrDefault(t => t.AsType() == _typeToRemove);
            if (controller != null)
                feature.Controllers.Remove(controller);
        }
    }
}