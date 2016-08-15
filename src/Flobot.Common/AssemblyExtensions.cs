using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flobot.Common
{
    public static class AssemblyExtensions
    {
        private const int peHeaderOffset = 60;
        private const int linkerTimestampOffset = 8;

        public static DateTime GetCompileDate(this Assembly assembly)
        {
            var linkerData = new byte[2048];

            using (FileStream fs = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
            {
                fs.Read(linkerData, 0, 2048);
            }

            var compileDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(BitConverter.ToInt32(linkerData, BitConverter.ToInt32(linkerData, peHeaderOffset) + linkerTimestampOffset));
            return compileDate.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(compileDate).Hours);
        }
    }
}
