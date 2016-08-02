using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Flobot.Common
{
    public class StringBuilderEx
    {
        protected const string SkypeNewLine = "\n\n";

        private StringBuilder stringBuilder;
        private StringBuilderExMode mode;

        public StringBuilderEx(StringBuilderExMode mode)
        {
            this.mode = mode;
            stringBuilder = new StringBuilder();
        }

        public void AppendLine()
        {
            AppendLine(string.Empty);
        }

        public void AppendLine(string value)
        {
            if (mode == StringBuilderExMode.Skype)
            {
                InternalAppendSkypeLine(value);
            }
            else
            {
                InternalAppendLine(value);
            }
        }

        public void Append()
        {
            Append(string.Empty);
        }

        public void Append(string value)
        {
            stringBuilder.Append(value);
        }

        public void AppendFormat(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args);
        }

        public void AppendFormatLine(string format, params object[] args)
        {
            AppendFormat(format, args);
            AppendLine();
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        private void InternalAppendLine(string value)
        {
            stringBuilder.AppendLine();
        }

        private void InternalAppendSkypeLine(string value)
        {
            stringBuilder.Append(value + SkypeNewLine);
        }
    }
}
