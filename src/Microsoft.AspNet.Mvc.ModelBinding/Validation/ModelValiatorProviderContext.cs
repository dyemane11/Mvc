﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class ModelValidatorProviderContext
    {
        public ModelValidatorProviderContext(ModelMetadata modelMetadata)
        {
            ModelMetadata = modelMetadata;
        }

        public ModelMetadata ModelMetadata { get; }

        public IReadOnlyList<object> ValidatorMetadata
        {
            get
            {
                return ModelMetadata.ValidatorMetadata;
            }
        }

        public IList<IModelValidator> Validators { get; } = new List<IModelValidator>();
    }
}