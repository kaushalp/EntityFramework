﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational.Metadata;
using Microsoft.Data.Entity.Relational.Update;
using Moq;
using Xunit;

namespace Microsoft.Data.Entity.Relational.Tests.Update
{
    public class ModificationCommandBatchFactoryTest
    {
        [Fact]
        public void Create_returns_new_instances()
        {
            var factory = new TestModificationCommandBatchFactory(
                new Mock<SqlGenerator>().Object);
            var options = new Mock<IDbContextOptions>().Object;

            var firstBatch = factory.Create(options);
            var secondBatch = factory.Create(options);

            Assert.NotNull(firstBatch);
            Assert.NotNull(secondBatch);
            Assert.NotSame(firstBatch, secondBatch);
        }

        [Fact]
        public void AddCommand_delegates()
        {
            var sqlGenerator = new Mock<SqlGenerator>().Object;
            var factory = new TestModificationCommandBatchFactory(sqlGenerator);

            var modificationCommandBatchMock = new Mock<ModificationCommandBatch>();
            var mockModificationCommand = new Mock<ModificationCommand>().Object;

            factory.AddCommand(modificationCommandBatchMock.Object, mockModificationCommand);

            modificationCommandBatchMock.Verify(mcb => mcb.AddCommand(mockModificationCommand));
        }

        private class TestModificationCommandBatchFactory : ModificationCommandBatchFactory
        {
            public TestModificationCommandBatchFactory(SqlGenerator sqlGenerator)
                : base(sqlGenerator)
            {
            }

            public override ModificationCommandBatch Create([NotNull] IDbContextOptions options)
            {
                return new TestModificationCommandBatch(SqlGenerator);
            }
        }

        private class TestModificationCommandBatch : SingularModificationCommandBatch
        {
            public TestModificationCommandBatch(SqlGenerator sqlGenerator)
                : base(sqlGenerator)
            {
            }

            public override IRelationalPropertyExtensions GetPropertyExtensions(IProperty property)
            {
                return property.Relational();
            }
        }
    }
}
