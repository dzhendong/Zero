using System;
using System.IO;

namespace ILvYou.Zero.Utility
{
    public class FileUtil
    {
        #region APIs
        public static string ResolveFile(string fName)
        {
            if (fName != null && fName.StartsWith("~"))
            {
                fName = fName.Substring(1);
                if (fName.StartsWith("/") || fName.StartsWith("\\"))
                {
                    fName = fName.Substring(1);
                }
                fName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, fName);
            }
            return fName;
        }
        #endregion
    }
}
