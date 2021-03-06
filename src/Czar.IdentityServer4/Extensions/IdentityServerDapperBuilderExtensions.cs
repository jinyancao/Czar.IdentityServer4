﻿using Czar.IdentityServer4.Options;
using System;
using IdentityServer4.Stores;
using Czar.IdentityServer4.Stores.SqlServer;
using Czar.IdentityServer4.Interfaces;
using Czar.IdentityServer4.HostedServices;
using Microsoft.Extensions.Hosting;
using Czar.IdentityServer4.Stores.MySql;
using IdentityServer4.ResponseHandling;
using Czar.IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using Czar.IdentityServer4.Caches;
using IdentityServer4.Validation;
using Czar.IdentityServer4.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 使用Dapper扩展
    /// </summary>
    public static class IdentityServerDapperBuilderExtensions
    {
        /// <summary>
        /// 配置Dapper接口和实现(默认使用SqlServer)
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">存储配置信息</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddDapperStore(
            this IIdentityServerBuilder builder,
            Action<DapperStoreOptions> storeOptionsAction = null)
        {
            var options = new DapperStoreOptions();
            builder.Services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);
            builder.Services.AddTransient<IClientStore, SqlServerClientStore>();
            builder.Services.AddTransient<IResourceStore, SqlServerResourceStore>();
            builder.Services.AddTransient<IPersistedGrantStore, SqlServerPersistedGrantStore>();
            builder.Services.AddTransient<IPersistedGrants, SqlServerPersistedGrants>();
            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();
            builder.Services.AddSingleton<ITokenResponseGenerator, CzarTokenResponseGenerator>();
            builder.Services.AddTransient(typeof(ICache<>), typeof(CzarRedisCache<>));
            builder.Services.AddTransient<IIntrospectionRequestValidator, CzarIntrospectionRequestValidator>();
            return builder;
        }

        /// <summary>
        /// 使用Mysql存储
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder UseMySql(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, MySqlClientStore>();
            builder.Services.AddTransient<IResourceStore, MySqlResourceStore>();
            builder.Services.AddTransient<IPersistedGrantStore, MySqlPersistedGrantStore>();
            builder.Services.AddTransient<IPersistedGrants, MySqlPersistedGrants>();
            return builder;
        }
    }
}
