using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Excel;
using Flobot.Logging;

namespace Flobot.Messages.Handlers.PsychoRaid
{
    public class GoogleDocProxy
    {
        private string fileName;
        private ILog logger;

        private string FileName
        {
            get
            {
                if (fileName == null)
                {
                    fileName = HttpRuntime.AppDomainAppPath + "Files/psycho/" + Guid.NewGuid().ToString("D");
                }

                return fileName;
            }
        }

        public GoogleDocProxy()
        {
            logger = this.GetLogger();
        }

        public RaidMember GetRaidMember(string name)
        {
            var allRaidMembers = InternalGetAllRaidMembers();
            return allRaidMembers.FirstOrDefault(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
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
                    client.DownloadFile("https://docs.google.com/spreadsheets/d/1qUVcXXFcp9Zi6qr1Zj5HGbR-4u6MJGMZyXkie58Qehw/export?format=xlsx&id=1qUVcXXFcp9Zi6qr1Zj5HGbR-4u6MJGMZyXkie58Qehw", FileName);
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
