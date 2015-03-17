// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    /// <summary>
    /// This <see cref="IModelValidatorProvider"/> provides a required ModelValidator for members marked
    /// as <c>[DataMember(IsRequired=true)]</c>.
    /// </summary>
    public class DataMemberModelValidatorProvider : IModelValidatorProvider
    {
        public void GetValidators(ModelValidatorProviderContext context)
        {
            // Types cannot be required; only properties can
            if (context.ModelMetadata.MetadataKind != ModelMetadataKind.Property)
            {
                return;
            }

            var dataMemberAttribute = context
                .ValidatorMetadata
                .OfType<DataMemberAttribute>()
                .FirstOrDefault();
            if (dataMemberAttribute == null || !dataMemberAttribute.IsRequired)
            {
                return;
            }

            // isDataContract == true iff the container type has at least one DataContractAttribute
            var containerType = context.ModelMetadata.ContainerType.GetTypeInfo();
            var isDataContract = containerType.GetCustomAttribute<DataContractAttribute>() != null;
            if (isDataContract)
            {
                context.Validators.Add(new RequiredMemberModelValidator());
            }
        }
    }
}