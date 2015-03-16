// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.ModelBinding.Metadata;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    public static class BindingHelper
    {   
        public static BindingMetadata GetBindingMetadata(IEnumerable<object> attributes)
        {
            var bindingMetadata = new BindingMetadata();

            // For Model Name  - we only use the first attribute we find. An attribute on the parameter
            // is considered an override of an attribute on the type. This is for compatibility with [Bind]
            // from MVC 5.
            //
            // BinderType and BindingSource fall back to the first attribute to provide a value.

            // BinderModelName
            var binderModelNameAttribute = attributes.OfType<IModelNameProvider>().FirstOrDefault();
            if (binderModelNameAttribute?.Name != null)
            {
                bindingMetadata.BinderModelName = binderModelNameAttribute.Name;
            }

            // BinderType
            foreach (var binderTypeAttribute in attributes.OfType<IBinderTypeProviderMetadata>())
            {
                if (binderTypeAttribute.BinderType != null)
                {
                    bindingMetadata.BinderType = binderTypeAttribute.BinderType;
                    break;
                }
            }

            // BindingSource
            foreach (var bindingSourceAttribute in attributes.OfType<IBindingSourceMetadata>())
            {
                if (bindingSourceAttribute.BindingSource != null)
                {
                    bindingMetadata.BindingSource = bindingSourceAttribute.BindingSource;
                    break;
                }
            }

            // PropertyBindingPredicateProvider
            var predicateProviders = attributes.OfType<IPropertyBindingPredicateProvider>().ToArray();
            if (predicateProviders.Length > 0)
            {
                bindingMetadata.PropertyBindingPredicateProvider = new CompositePredicateProvider(
                    predicateProviders);
            }

            return bindingMetadata;
        }

        private class CompositePredicateProvider : IPropertyBindingPredicateProvider
        {
            private readonly IEnumerable<IPropertyBindingPredicateProvider> _providers;

            public CompositePredicateProvider(IEnumerable<IPropertyBindingPredicateProvider> providers)
            {
                _providers = providers;
            }

            public Func<ModelBindingContext, string, bool> PropertyFilter
            {
                get
                {
                    return CreatePredicate();
                }
            }

            private Func<ModelBindingContext, string, bool> CreatePredicate()
            {
                var predicates = _providers
                    .Select(p => p.PropertyFilter)
                    .Where(p => p != null);

                return (context, propertyName) =>
                {
                    foreach (var predicate in predicates)
                    {
                        if (!predicate(context, propertyName))
                        {
                            return false;
                        }
                    }

                    return true;
                };
            }
        }
    }
}