// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    public sealed class ComplexModelDtoModelBinder : IModelBinder
    {
        public async Task<ModelBindingResult> BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(ComplexModelDto))
            {
                return null;
            }

            ModelBindingHelper.ValidateBindingContext(bindingContext, typeof(ComplexModelDto), allowNullModel: false);

            var dto = (ComplexModelDto)bindingContext.Model;
            foreach (var propertyMetadata in dto.PropertyMetadata)
            {
                var propertyModelName = ModelBindingHelper.CreatePropertyModelName(
                    bindingContext.ModelName,
                    propertyMetadata.BinderModelName ?? propertyMetadata.PropertyName);
                var propertyBindingContext =
                    new ModelBindingContext(bindingContext, propertyModelName, propertyMetadata);

                // Bind and propagate the values.
                var modelBindingResult =
                    await bindingContext.OperationBindingContext.ModelBinder.BindModelAsync(propertyBindingContext);
                if (modelBindingResult == null)
                {
                    // Could not bind. Add default result to let caller know to pick up default value (if any).
                    dto.Results[propertyMetadata] =
                        new ModelBindingResult(model: null, key: propertyModelName, isModelSet: false);
                }
                else
                {
                    dto.Results[propertyMetadata] = modelBindingResult;
                }
            }

            return new ModelBindingResult(dto, bindingContext.ModelName, isModelSet: true);
        }
    }
}
