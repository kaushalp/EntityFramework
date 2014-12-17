// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Framework.ConfigurationModel;
using Xunit;
using System;

namespace Microsoft.Data.Entity.Tests
{
    public class DbContextOptionsParserTest
    {
        [Fact]
        public void Connection_string_is_found_using_context_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Connection_string_is_found_using_context_full_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Connection_string_is_found_using_context_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Connection_string_is_found_using_context_full_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        private class MyContext : DbContext
        {
        }

        [Fact]
        public void Indirect_connection_string_is_found_using_context_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "Data:DefaultConnection:ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionStringKey", "Data:DefaultConnection:ConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Indirect_connection_string_is_found_using_context_full_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "Data:DefaultConnection:ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionStringKey", "Data:DefaultConnection:ConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Indirect_connection_string_is_found_using_context_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "Data:DefaultConnection:ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionStringKey", "Data:DefaultConnection:ConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Indirect_connection_string_is_found_using_context_full_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "Data:DefaultConnection:ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionStringKey", "Data:DefaultConnection:ConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Existing_options_are_updated()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var currentOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "Foo", "Goo" } };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), currentOptions);

            Assert.Equal(2, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("Goo", rawOptions["Foo"]);
        }

        [Fact]
        public void Existing_options_are_updated_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" }
                        }
                };

            var currentOptions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "Foo", "Goo" } };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, currentOptions);

            Assert.Equal(2, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("Goo", rawOptions["Foo"]);
        }

        [Fact]
        public void Key_searching_is_case_insensitive()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "entityFramework:" + typeof(MyContext).Name + ":connectionString", "MyConnectionString" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(1, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
        }

        [Fact]
        public void Nested_keys_are_read_using_context_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSize", "1" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_keys_are_read_using_context_full_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SqlServer:MaxBatchSize", "1" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":Level1:Level2:Level3", "NestedLevelValue" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_keys_are_read_using_context_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSize", "1" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_keys_are_read_using_context_full_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).FullName + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SqlServer:MaxBatchSize", "1" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).FullName + ":Level1:Level2:Level3", "NestedLevelValue" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_indirect_keys_are_read_using_context_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSizeKey", "Data:SqlServer:MaxBatchSize" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" },
                            { "Data:SqlServer:MaxBatchSize", "1" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_indirect_keys_are_read_using_context_full_name()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSizeKey", "Data:SqlServer:MaxBatchSize" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" },
                            { "Data:SqlServer:MaxBatchSize", "1" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions(config, typeof(MyContext), new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_indirect_keys_are_read_using_context_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSizeKey", "Data:SqlServer:MaxBatchSize" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" },
                            { "Data:SqlServer:MaxBatchSize", "1" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }

        [Fact]
        public void Nested_indirect_keys_are_read_using_context_full_name_generic()
        {
            var config = new Configuration
                {
                    new MemoryConfigurationSource
                        {
                            { "EntityFramework:" + typeof(MyContext).Name + ":ConnectionString", "MyConnectionString" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:MaxBatchSizeKey", "Data:SqlServer:MaxBatchSize" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SqlServer:AnotherSqlServerOption", "SqlServerOptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":SomeProvider:ProviderSpecificOption", "OptionValue" },
                            { "EntityFramework:" + typeof(MyContext).Name + ":Level1:Level2:Level3", "NestedLevelValue" },
                            { "Data:SqlServer:MaxBatchSize", "1" }
                        }
                };

            var rawOptions = new DbContextOptionsParser().ReadRawOptions<MyContext>(config, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            Assert.Equal(5, rawOptions.Count);
            Assert.Equal("MyConnectionString", rawOptions["ConnectionString"]);
            Assert.Equal("1", rawOptions["SqlServer:MaxBatchSize"]);
            Assert.Equal("SqlServerOptionValue", rawOptions["SqlServer:AnotherSqlServerOption"]);
            Assert.Equal("OptionValue", rawOptions["SomeProvider:ProviderSpecificOption"]);
            Assert.Equal("NestedLevelValue", rawOptions["Level1:Level2:Level3"]);
        }
    }
}
