// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.Serialization;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Validation
{
    public class DataMemberModelValidatorProviderTest
    {
        private readonly IModelMetadataProvider _metadataProvider = TestModelMetadataProvider.CreateDefaultProvider();

        [Fact]
        public void ClassWithoutAttributes_NoValidator()
        {
            // Arrange
            var provider = new DataMemberModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForProperty(typeof(ClassWithoutAttributes), "TheProperty");
            var validatorProviderContext = new ModelValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(validatorProviderContext);

            // Assert
            Assert.Empty(validatorProviderContext.Validators);
        }

        private class ClassWithoutAttributes
        {
            public int TheProperty { get; set; }
        }

        [Fact]
        public void ClassWithDataMemberIsRequiredTrue_Validator()
        {
            // Arrange
            var provider = new DataMemberModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForProperty(typeof(ClassWithDataMemberIsRequiredTrue), "TheProperty");
            var validatorProviderContext = new ModelValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(validatorProviderContext);

            // Assert
            var validator = Assert.Single(validatorProviderContext.Validators);
            Assert.True(validator.IsRequired);
        }

        [DataContract]
        private class ClassWithDataMemberIsRequiredTrue
        {
            [DataMember(IsRequired = true)]
            public int TheProperty { get; set; }
        }

        [Fact]
        public void ClassWithDataMemberIsRequiredFalse_NoValidator()
        {
            // Arrange
            var provider = new DataMemberModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForProperty(typeof(ClassWithDataMemberIsRequiredFalse), "TheProperty");
            var validatorProviderContext = new ModelValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(validatorProviderContext);

            // Assert
            Assert.Empty(validatorProviderContext.Validators);
        }

        [DataContract]
        private class ClassWithDataMemberIsRequiredFalse
        {
            [DataMember(IsRequired = false)]
            public int TheProperty { get; set; }
        }

        [Fact]
        public void ClassWithDataMemberIsRequiredTrueWithoutDataContract_NoValidator()
        {
            // Arrange
            var provider = new DataMemberModelValidatorProvider();
            var metadata = _metadataProvider.GetMetadataForProperty(typeof(ClassWithDataMemberIsRequiredTrueWithoutDataContract), "TheProperty");
            var validatorProviderContext = new ModelValidatorProviderContext(metadata);

            // Act
            provider.GetValidators(validatorProviderContext);

            // Assert
            Assert.Empty(validatorProviderContext.Validators);
        }

        private class ClassWithDataMemberIsRequiredTrueWithoutDataContract
        {
            [DataMember(IsRequired = true)]
            public int TheProperty { get; set; }
        }
    }
}
