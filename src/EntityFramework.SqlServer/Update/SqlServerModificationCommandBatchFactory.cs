// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Relational.Update;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.SqlServer.Update
{
    public class SqlServerModificationCommandBatchFactory : ModificationCommandBatchFactory
    {
        /// <summary>
        ///     This constructor is intended only for use when creating test doubles that will override members
        ///     with mocked or faked behavior. Use of this constructor for other purposes may result in unexpected
        ///     behavior including but not limited to throwing <see cref="NullReferenceException" />.
        /// </summary>
        protected SqlServerModificationCommandBatchFactory()
        {
        }

        public SqlServerModificationCommandBatchFactory(
            [NotNull] SqlServerSqlGenerator sqlGenerator)
            : base(sqlGenerator)
        {
        }

        public override ModificationCommandBatch Create([NotNull] IDbContextOptions options)
        {
            Check.NotNull(options, "options");

            int? maxBatchSize;
            try
            {
                var storeConfig = SqlServerOptionsExtension.Extract(options);
                maxBatchSize = storeConfig.MaxBatchSize;
            }
            catch (InvalidOperationException)
            {
                maxBatchSize = null;
            }

            return new SqlServerModificationCommandBatch((SqlServerSqlGenerator)SqlGenerator, maxBatchSize);
        }
    }
}
