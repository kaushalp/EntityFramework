// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.Data.Entity.SqlServer
{
    public class SqlServerOptionsExtension : RelationalOptionsExtension
    {
        private const string ProviderPrefix = "SqlServer";
        private const string MaxBatchSizeKey = "MaxBatchSize";

        private int? _maxBatchSize;

        public virtual int? MaxBatchSize
        {
            get { return _maxBatchSize; }

            set
            {
                _maxBatchSize = value;
            }

        }

        protected override void Configure(IReadOnlyDictionary<string, string> rawOptions)
        {
            base.Configure(rawOptions);
            string maxBatchSizeString;
            if (!_maxBatchSize.HasValue)
            {
                var MaxBatchSizeConfigurationKey = string.Concat(ProviderPrefix, ":", MaxBatchSizeKey);
                if (rawOptions.TryGetValue(MaxBatchSizeConfigurationKey, out maxBatchSizeString))
                {
                    int maxBatchSizeInt;
                    if (!Int32.TryParse(maxBatchSizeString, out maxBatchSizeInt))
                    {
                        throw new InvalidOperationException(Strings.IntegerConfigurationValueFormatError(MaxBatchSizeConfigurationKey, maxBatchSizeString));
                    }
                    _maxBatchSize = maxBatchSizeInt;
                }
            }
        }

        protected override void ApplyServices(EntityServicesBuilder builder)
        {
            Check.NotNull(builder, "builder");

            builder.AddSqlServer();
        }

        public static new SqlServerOptionsExtension Extract([NotNull] IDbContextOptions options)
        {
            Check.NotNull(options, "options");

            var storeConfigs = options.Extensions
                .OfType<SqlServerOptionsExtension>()
                .ToArray();

            if (storeConfigs.Length == 0)
            {
                throw new InvalidOperationException(/* */);
            }

            if (storeConfigs.Length > 1)
            {
                throw new InvalidOperationException(/* */);
            }

            return storeConfigs[0];
        }
    }
}
