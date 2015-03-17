// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace Microsoft.AspNet.Mvc
{
    public class StreamOutputFormatterTest
    {
        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(SimplePOCO))]
        [InlineData(null)]
        public void CanWriteResult_OnlyActsOnStreams(Type type)
        {
            // Arrange
            var formatter = new StreamOutputFormatter();
            var context = new OutputFormatterContext();
            var contentType = new MediaTypeHeaderValue("text/plain");

            context.Object = type != null ? Activator.CreateInstance(type) : null;

            // Act
            var result = formatter.CanWriteResult(context, contentType);

            // Assert
            Assert.False(result);
            Assert.Null(context.SelectedContentType);
        }

        [Fact]
        public void CanWriteResult_SetsContentType()
        {
            // Arrange
            var formatter = new StreamOutputFormatter();
            var contentType = new MediaTypeHeaderValue("text/plain");
            var context = new OutputFormatterContext();
            context.Object = new MemoryStream();

            // Act
            var result = formatter.CanWriteResult(context, contentType);

            // Assert
            Assert.True(result);
            Assert.Same(contentType, context.SelectedContentType);
        }

        private class SimplePOCO
        {
            public int Id { get; set; }
        }
    }
}