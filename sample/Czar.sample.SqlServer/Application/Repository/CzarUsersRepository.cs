using Czar.sample.SqlServer.Application.IRepository;
using Czar.sample.SqlServer.Infrastructure.Config;
using Czar.sample.SqlServer.Infrastructure.Models;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Czar.sample.SqlServer.Application.Repository
{
    /// <summary>
    /// 金焰的世界
    /// 2018-12-18
    /// 用户实体基于SQLSERVER的实现
    /// </summary>
    public class CzarUsersRepository : ICzarUsersRepository
    {
        private readonly string DbConn = "";
        public CzarUsersRepository(IOptions<CzarConfig> czarConfig)
        {
            DbConn = czarConfig.Value.DbConnectionStrings;
        }
        /// <summary>
        /// 根据账号密码获取用户实体
        /// </summary>
        /// <param name="uaccount">账号</param>
        /// <param name="upassword">密码</param>
        /// <returns></returns>
        public CzarUsers FindUserByuAccount(string uaccount, string upassword)
        {
            using (var connection = new SqlConnection(DbConn))
            {
                string sql = @"SELECT * from CzarUsers where uAccount=@uaccount and uPassword=upassword and uStatus=1";
                var result = connection.QueryFirstOrDefault<CzarUsers>(sql, new { uaccount, upassword = SecretHelper.ToMD5(upassword) });
                return result;
            }
        }

        /// <summary>
        /// 根据用户主键获取用户实体
        /// </summary>
        /// <param name="sub">用户标识</param>
        /// <returns></returns>
        public CzarUsers FindUserByUid(string sub)
        {
            using (var connection = new SqlConnection(DbConn))
            {
                string sql = @"SELECT * from CzarUsers where uid=@uid";
                var result = connection.QueryFirstOrDefault<CzarUsers>(sql, new { uid=sub });
                return result;
            }
        }
    }

    /// <summary>
    /// 金焰的世界
    /// 2018-12-18
    /// 加密相关帮助类
    /// </summary>
    public class SecretHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string ToMD5(string str)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
