using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using YU.Core.DataEntity;
using YU.Core.Utils;

namespace YU.Core
{
    public static class AppService
    {
        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="dbPassWord"></param>
        public static void InitConn(string dbName, string dbPassWord)
        {
            DBUtils.SetConnectionString(dbName, dbPassWord);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="dbName"></param>
        public static void InitDB(string dbName)
        {
            DBUtils.CreateDB(dbName);
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configKey"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string configKey, T defaultValue = default(T))
        {
            string selectSql = " SELECT VALUE FROM CONFIG WHERE FID = @FID ";
            SQLiteParameter parm = new SQLiteParameter("@FID", DbType.String);
            parm.Value = configKey;
            string dbValue = DBUtils.ExecuteScalar(selectSql, "", parm);
            if (dbValue.IsNullOrEmptyOrWhiteSpace())
                return defaultValue;
            else
                return JsonConvert.DeserializeObject<T>(dbValue);
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="configValue"></param>
        public static void SetConfig(string configKey, object configValue)
        {
            string selectSql = " SELECT 1 FROM CONFIG WHERE FID = @FID ";
            SQLiteParameter parm = new SQLiteParameter("@FID", DbType.String);
            parm.Value = configKey;
            string sql = string.Empty;

            if (DBUtils.ExecuteScalar<int>(selectSql, -1, parm) == -1)
                sql = @" INSERT INTO CONFIG(FID,VALUE) VALUES(@FID,@VALUE) ";
            else
                sql = @" UPDATE CONFIG SET VALUE = @VALUE WHERE FID = @FID ";

            SQLiteParameter[] parms = new SQLiteParameter[]
                              {
                        new SQLiteParameter("@FID", DbType.String),
                        new SQLiteParameter("@VALUE", DbType.String)
                              };
            parms[0].Value = configKey;
            parms[1].Value = JsonConvert.SerializeObject(configValue);

            DBUtils.ExecuteNonQuery(sql, parms);
        }

        /// <summary>
        /// 初始化表结构
        /// </summary>
        public static void InitTable()
        {
            List<KeyValuePair<string, SQLiteParameter[]>> sqlList = new List<KeyValuePair<string, SQLiteParameter[]>>();

            if (!DBUtils.ExistTable("CONFIG"))
            {
                //Config表
                string sql = "CREATE TABLE CONFIG (FID TEXT,VALUE TEXT)";
                sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, null));
            }

            if (!DBUtils.ExistTable("USER"))
            {
                //User表
                string sql = "CREATE TABLE USER (FID TEXT,PTSITEID INTEGER,USERID INTEGER,USERNAME TEXT,MAIL TEXT,PASSWORD TEXT,SECURITYQUESTIONORDER INTEGER,SECUITYANSWER TEXT,ISENABLETWO_STEPVERIFICATION INTEGER)";
                sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, null));
            }

            if (!DBUtils.ExistTable("PERSONINFO"))
            {
                //PersonInfo表
                string sql = @"CREATE TABLE PERSONINFO (FID TEXT,PTSITEID INTEGER,SITENAME TEXT,USERID INTEGER,URL TEXT,NAME TEXT,
DOWNSIZE TEXT,UPSIZE TEXT,SHARERATE TEXT,SEEDNUMBER TEXT,SEEDTIMES TEXT,DOWNTIMES TEXT,SEEDRATE TEXT,BONUS REAL,REGISTERDATE TEXT,RANK TEXT,LASTSYNCDATE TEXT)";
                sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, null));

                sql = @"CREATE INDEX PERSONINFO_SITEID ON PERSONINFO (PTSITEID)";
                sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, null));
            }

            DBUtils.ExecuteNonQueryBatch(sqlList);
        }

        /// <summary>
        /// 插入个人信息
        /// </summary>
        /// <param name="infos"></param>
        public static void InsertPersonInfo(IEnumerable<PTInfo> infos)
        {
            List<KeyValuePair<string, SQLiteParameter[]>> sqlList = new List<KeyValuePair<string, SQLiteParameter[]>>();
            if (infos != null && infos.Count() > 0)
            {
                foreach (var info in infos)
                {
                    string sql = @" INSERT INTO PERSONINFO(PTSITEID,SITENAME,USERID,URL,NAME,DOWNSIZE,UPSIZE,SHARERATE,SEEDNUMBER,SEEDTIMES,DOWNTIMES,SEEDRATE,BONUS,REGISTERDATE,RANK,LASTSYNCDATE,FID)
VALUES(@PTSITEID,@SITENAME,@USERID,@URL,@NAME,@DOWNSIZE,@UPSIZE,@SHARERATE,@SEEDNUMBER,@SEEDTIMES,@DOWNTIMES,@SEEDRATE,@BONUS,@REGISTERDATE,@RANK,@LASTSYNCDATE,@FID) ";
                    SQLiteParameter[] parms = new SQLiteParameter[]
                      {
                                    new SQLiteParameter("@PTSITEID", DbType.Int32),
                                    new SQLiteParameter("@SITENAME", DbType.String),
                                    new SQLiteParameter("@USERID", DbType.Int32),
                                    new SQLiteParameter("@URL", DbType.String),
                                    new SQLiteParameter("@NAME", DbType.String),
                                    new SQLiteParameter("@DOWNSIZE", DbType.String),
                                    new SQLiteParameter("@UPSIZE", DbType.String),
                                    new SQLiteParameter("@SHARERATE", DbType.String),
                                    new SQLiteParameter("@SEEDNUMBER", DbType.String),
                                    new SQLiteParameter("@SEEDTIMES", DbType.String),
                                    new SQLiteParameter("@DOWNTIMES", DbType.String),
                                    new SQLiteParameter("@SEEDRATE", DbType.String),
                                    new SQLiteParameter("@BONUS", DbType.Double),
                                    new SQLiteParameter("@REGISTERDATE", DbType.DateTime),
                                    new SQLiteParameter("@RANK", DbType.String),
                                    new SQLiteParameter("@LASTSYNCDATE", DbType.DateTime),
                                    new SQLiteParameter("@FID", DbType.String),
                      };
                    parms[0].Value = info.SiteId;
                    parms[1].Value = info.SiteName;
                    parms[2].Value = info.UserId;
                    parms[3].Value = info.Url;
                    parms[4].Value = info.Name;
                    parms[5].Value = info.DownSize;
                    parms[6].Value = info.UpSize;
                    parms[7].Value = info.ShareRate;
                    parms[8].Value = info.SeedNumber;
                    parms[9].Value = info.SeedTimes;
                    parms[10].Value = info.DownTimes;
                    parms[11].Value = info.SeedRate;
                    parms[12].Value = info.Bonus;
                    parms[13].Value = info.RegisterDate;
                    parms[14].Value = info.Rank;
                    parms[15].Value = info.LastSyncDate;
                    parms[16].Value = Convert.ToString(Guid.NewGuid());
                    sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, parms));
                }
            }
            if (sqlList.Count > 0)
                DBUtils.ExecuteNonQueryBatch(sqlList);
        }

        /// <summary>
        /// 获取最新的同步信息
        /// </summary>
        /// <param name="siteIds"></param>
        /// <returns></returns>
        public static List<PTInfo> GetLastPersonInfo(IEnumerable<int> siteIds, IEnumerable<PTUser> users)
        {
            List<PTInfo> infos = new List<PTInfo>();
            string sql = string.Format("SELECT * FROM PERSONINFO A WHERE NOT EXISTS(SELECT 1 FROM PERSONINFO B WHERE A.PTSITEID = B.PTSITEID AND A.LASTSYNCDATE < B.LASTSYNCDATE) AND A.PTSITEID IN ({0}) AND ( A.NAME IN ('{1}') OR  A.USERID IN ({2}) )", string.Join(",", siteIds), string.Join("','", users.Select(x => x.UserName).Distinct()), string.Join(",", users.Select(x => x.UserId).Distinct()));
            SQLiteDataReader reader = DBUtils.ExecuteReader(sql);
            while (reader.Read())
            {
                PTInfo info = new PTInfo();
                info.Fid = reader["FID"].TryPareValue<string>();
                info.SiteId = (YUEnums.PTEnum)reader["PTSITEID"].TryPareValue<int>();
                info.SiteName = reader["SITENAME"].TryPareValue<string>();
                info.UserId = reader["USERID"].TryPareValue<int>();
                info.Url = reader["URL"].TryPareValue<string>();
                info.Name = reader["NAME"].TryPareValue<string>();
                info.DownSize = reader["DOWNSIZE"].TryPareValue<string>();
                info.UpSize = reader["UPSIZE"].TryPareValue<string>();
                info.ShareRate = reader["SHARERATE"].TryPareValue<string>();
                info.SeedNumber = reader["SEEDNUMBER"].TryPareValue<string>();
                info.SeedTimes = reader["SEEDTIMES"].TryPareValue<string>();
                info.DownTimes = reader["DOWNTIMES"].TryPareValue<string>();
                info.SeedRate = reader["SEEDRATE"].TryPareValue<string>();
                info.Bonus = reader["BONUS"].TryPareValue<double>();
                info.RegisterDate = reader["REGISTERDATE"].TryPareValue<DateTime>();
                info.Rank = reader["RANK"].TryPareValue<string>();
                info.LastSyncDate = reader["LASTSYNCDATE"].TryPareValue<DateTime>();
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="sites"></param>
        /// <returns></returns>
        public static List<PTUser> GetAllUsers(List<PTSite> sites)
        {
            var users = new List<PTUser>();

            IDataReader dr = DBUtils.ExecuteReader("SELECT * FROM USER");
            while (dr.Read())
            {
                int siteId = dr["PTSITEID"].TryPareValue(0);
                PTUser user = new PTUser();
                user.Fid = dr["FID"].TryPareValue<string>();
                user.UserId = dr["USERID"].TryPareValue<int>();
                user.UserName = dr["USERNAME"].TryPareValue<string>();
                user.Mail = dr["MAIL"].TryPareValue<string>();
                user.PassWord = dr["PASSWORD"].TryPareValue<string>();
                user.isEnableTwo_StepVerification = dr["ISENABLETWO_STEPVERIFICATION"].TryPareValue(false);
                user.SecuityAnswer = dr["SECUITYANSWER"].TryPareValue(string.Empty);
                user.SecurityQuestionOrder = dr["SECURITYQUESTIONORDER"].TryPareValue(-1);
                user.Site = sites.Where(x => (int)x.Id == siteId).FirstOrDefault();
                //如果找不到相应站点，这里就直接跳过该用户了。
                if (user.Site == null)
                    continue;
                users.Add(user);
            }
            users = users.OrderBy(x => x.Site.Order).ToList();
            return users;
        }

        /// <summary>
        /// 插入或更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int UpdateOrInsertUser(PTUser user)
        {
            //目前同一个站点有且只能有一个用户存在
            string selectSql = " SELECT PTSITEID FROM USER WHERE PTSITEID = @PTSITEID ";
            SQLiteParameter param = new SQLiteParameter("@PTSITEID", DbType.Int32);
            param.Value = user.Site.Id;
            string sql = string.Empty;
            if (DBUtils.ExecuteScalar<int>(selectSql, -1, param) != (int)user.Site.Id)
                sql = @" INSERT INTO USER(FID,PTSITEID,USERNAME,MAIL,PASSWORD,SECURITYQUESTIONORDER,SECUITYANSWER,ISENABLETWO_STEPVERIFICATION) VALUES(@FID,@PTSITEID,@USERNAME,@MAIL,@PASSWORD,@SECURITYQUESTIONORDER,@SECUITYANSWER,@ISENABLETWO_STEPVERIFICATION) ";
            else
                sql = @" UPDATE USER SET USERNAME = @USERNAME, MAIL = @MAIL, PASSWORD = @PASSWORD, SECURITYQUESTIONORDER = @SECURITYQUESTIONORDER, SECUITYANSWER = @SECUITYANSWER , ISENABLETWO_STEPVERIFICATION = @ISENABLETWO_STEPVERIFICATION WHERE PTSITEID = @PTSITEID ";

            SQLiteParameter[] parms = new SQLiteParameter[]
            {
                        new SQLiteParameter("@FID", DbType.String),
                        new SQLiteParameter("@PTSITEID", DbType.Int32),
                        new SQLiteParameter("@USERNAME", DbType.String),
                        new SQLiteParameter("@MAIL", DbType.String),
                        new SQLiteParameter("@PASSWORD", DbType.String),
                        new SQLiteParameter("@SECURITYQUESTIONORDER", DbType.Int32),
                        new SQLiteParameter("@SECUITYANSWER", DbType.String),
                        new SQLiteParameter("@ISENABLETWO_STEPVERIFICATION", DbType.Boolean),
            };
            parms[0].Value = Convert.ToString(Guid.NewGuid());
            parms[1].Value = user.Site.Id;
            parms[2].Value = user.UserName;
            parms[3].Value = user.Mail;
            parms[4].Value = user.PassWord;
            parms[5].Value = user.SecurityQuestionOrder;
            parms[6].Value = user.SecuityAnswer;
            parms[7].Value = user.isEnableTwo_StepVerification;
            return DBUtils.ExecuteNonQuery(sql, parms);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static int DeleteUser(YUEnums.PTEnum siteId)
        {
            string delSql = "DELETE FROM USER WHERE PTSITEID = @PTSITEID";
            SQLiteParameter parm = new SQLiteParameter("@PTSITEID", DbType.Int32);
            parm.Value = (int)siteId;
            return DBUtils.ExecuteNonQuery(delSql, parm);
        }

        /// <summary>
        /// 获取个人Id更新参数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static KeyValuePair<string, SQLiteParameter[]> GetUpdateUserParameter(PTUser user)
        {
            //目前一个站点有且只会存在一个用户。
            //后面如果支持多用户，考虑使用FID，不过感觉不会有那么的一天。
            string sql = "UPDATE USER SET USERID = @USERID,USERNAME = @USERNAME  WHERE PTSITEID = @PTSITEID ";
            SQLiteParameter[] parms = new SQLiteParameter[]
            {
                    new SQLiteParameter("@USERID", DbType.Int32),
                    new SQLiteParameter("@USERNAME", DbType.String),
                    new SQLiteParameter("@PTSITEID", DbType.Int32),
            };
            parms[0].Value = user.UserId;
            parms[1].Value = user.UserName;
            parms[2].Value = user.Site.Id;
            return new KeyValuePair<string, SQLiteParameter[]>(sql, parms);
        }

    }
}
