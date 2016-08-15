using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Excel;
using Flobot.Common.Algorithms;
using Flobot.Logging;

namespace Flobot.Messages.Handlers.PsychoRaid
{
    public class GoogleDocProxy
    {
        private string fileName;
        private readonly ILog logger;

        private string FileName
        {
            get
            {
                if (fileName == null)
                {
                    fileName = HttpRuntime.AppDomainAppPath + @"Files\psycho\" + Guid.NewGuid().ToString("D");

#if DEBUG
                    fileName = HttpRuntime.AppDomainAppPath + Guid.NewGuid().ToString("D");
#endif
                }

                return fileName;
            }
        }

        public GoogleDocProxy(ILoggingService loggingService)
        {
            logger = loggingService.GetLogger(this);
        }

        public IEnumerable<RaidMember> GetRaidMembers(string name)
        {
            var allRaidMembers = InternalGetAllRaidMembers();
            Levenshtein levenshtein = new Levenshtein();

            foreach (var member in allRaidMembers)
            {
                if (levenshtein.GetDistance(name, member.Name, true) <= 1)
                {
                    yield return member;
                }
            }
        }

        public IEnumerable<RaidMember> GetAllRaidMembers()
        {
            return InternalGetAllRaidMembers();
        }

        /// <summary>
        /// This implementation is dumb and unsafe, but who cares?
        /// </summary>
        /// <returns></returns>
        private List<RaidMember> InternalGetAllRaidMembers()
        {
            List<RaidMember> raidMembers = new List<RaidMember>();

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(ConfigurationManager.AppSettings["PsychoRaidSpreadsheet"], FileName);
                }

                FileStream stream = File.Open(FileName, FileMode.Open, FileAccess.Read);

                using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    DataSet result = excelReader.AsDataSet();
                    var raidMembersInfo = result.Tables.Cast<DataTable>().FirstOrDefault();
                    for (int i = 0; i < raidMembersInfo.Rows.Count; i++)
                    {
                        DataRow row = raidMembersInfo.Rows[i];

                        if (row.ItemArray != null)
                        {
                            string name = row.ItemArray[2].ToString();
                            double sum;
                            if (!string.IsNullOrEmpty(row.ItemArray[0]?.ToString()) && !string.IsNullOrEmpty(name))
                            {
                                if (!double.TryParse(row.ItemArray[3].ToString(), out sum))
                                {
                                    sum = 0;
                                }

                                RaidMember rm = new RaidMember();
                                rm.Name = name;
                                rm.Sum = sum;
                                raidMembers.Add(rm);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
            }

            return raidMembers;
        }
    }
}
