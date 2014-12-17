// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.ConfigurationModel;

namespace Microsoft.Data.Entity.Infrastructure
{
    public class DbContextOptionsParser
    {
        private const string EntityFrameworkKey = "EntityFramework";
        private const string KeySuffix = "Key";

        public virtual IReadOnlyDictionary<string, string> ReadRawOptions<TContext>(
            [NotNull] IConfiguration configuration,
            [NotNull] IReadOnlyDictionary<string, string> currentOptions)
            where TContext : DbContext
        {
            Check.NotNull(configuration, "configuration");
            Check.NotNull(currentOptions, "currentOptions");

            return ReadRawOptions(configuration, typeof(TContext), currentOptions);
        }

        public virtual IReadOnlyDictionary<string, string> ReadRawOptions(
            [NotNull] IConfiguration configuration,
            [NotNull] Type contextType,
            [NotNull] IReadOnlyDictionary<string, string> currentOptions)
        {
            Check.NotNull(configuration, "configuration");
            Check.NotNull(contextType, "contextType");

            var options = currentOptions.ToDictionary(i => i.Key, i => i.Value, StringComparer.OrdinalIgnoreCase);

            ReadRawOptions(options, configuration, string.Concat(
                EntityFrameworkKey, Constants.KeyDelimiter, contextType.Name), string.Empty);

            ReadRawOptions(options, configuration, string.Concat(
                EntityFrameworkKey, Constants.KeyDelimiter, contextType.FullName), string.Empty);

            return options;
        }

        private static void ReadRawOptions(
            Dictionary<string, string> options,
            IConfiguration configuration,
            string contextKey,
            string keyPrefix)
        {
            foreach (var pair in configuration.GetSubKeys(contextKey))
            {
                string value;
                var key = pair.Key;
                if (!pair.Value.TryGet(null, out value))
                {
                    ReadRawOptions(options, configuration,
                        string.Concat(contextKey, Constants.KeyDelimiter, key),
                        string.Concat(keyPrefix, key, Constants.KeyDelimiter));
                    continue;
                }

                if (key.EndsWith(KeySuffix, StringComparison.Ordinal)
                    && configuration.TryGet(value, out value))
                {
                    key = key.Substring(0, key.Length - KeySuffix.Length);
                }

                options[string.Concat(keyPrefix, key)] = value;
            }
        }
    }
}
