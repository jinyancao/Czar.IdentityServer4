
using System.Collections.Generic;

namespace Czar.IdentityServer4.Options
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-03
    /// 配置存储信息
    /// </summary>
    public class DapperStoreOptions
    {
        /// <summary>
        /// 是否启用自定清理Token
        /// </summary>
        public bool EnableTokenCleanup { get; set; } = false;

        /// <summary>
        /// 清理token周期（单位秒），默认1小时
        /// </summary>
        public int TokenCleanupInterval { get; set; } = 3600;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string DbConnectionStrings { get; set; }

        /// <summary>
        /// 是否启用强制过期策略,默认不开启
        /// </summary>
        public bool EnableForceExpire { get; set; } = false;

        /// <summary>
        /// Redis缓存连接
        /// </summary>
        public List<string> RedisConnectionStrings { get; set; }
    }
}
