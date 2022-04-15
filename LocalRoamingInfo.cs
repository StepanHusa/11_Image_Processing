using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Image_Processing
{
    static class LI
    {
        private static string projectInfosFilename { get { return ST.roamingFolder + "\\ProjectInfos.file"; } }
        public static List<ProjectInfo> projectInfosInLocalFile
        {
            get
            {
                var l= new List<ProjectInfo>();
                if (File.Exists(projectInfosFilename))
                {
                    using (FileStream ms = new(projectInfosFilename,FileMode.Open))
                    {
                        using(BinaryReader r = new(ms))
                        {
                            int length = r.ReadInt32();
                            for(int i = 0; i < length ; i++)
                            {
                                l.Add(new(r.ReadString(), r.ReadString(), r.ReadString(), r.ReadString(),r.ReadBoolean()));
                            }
                        }
                    }
                }
                return l;
            }
            set
            {
                if (value == null) return;
                using (FileStream fs = new(projectInfosFilename, FileMode.OpenOrCreate))
                {
                    using(BinaryWriter bw = new(fs))
                    {
                        bw.Write(value.Count);
                        for (int i = 0; i < value.Count; i++)
                        {
                            bw.Write(value[i].Name);
                            bw.Write(value[i].Location);
                            bw.Write(value[i].DateLastOpened);
                            bw.Write(value[i].DateLastEdit);
                            bw.Write(value[i].IsLocjed);
                        }
                    }
                }
            }
        }
       public static void AddProjectInfoToLocalFile(this ProjectInfo projectInfo)
        {
            var l = projectInfosInLocalFile;
            l.Add(projectInfo);
            projectInfosInLocalFile = l;
        }


        private static string languageselectionfilename { get { return ST.roamingFolder + "\\languageselection.file"; } }
        public static System.Globalization.CultureInfo languageselection 
        { 
            get
            {
                if (File.Exists(languageselectionfilename))
                {
                    using (FileStream ms = new(languageselectionfilename, FileMode.Open))
                    {
                        using (BinaryReader r = new(ms))
                        {
                            return new(r.ReadInt32());
                        }
                    }
                }
                else return null;

            }
            set
            {
                if (value == null)
                {
                    if (File.Exists(languageselectionfilename)) File.Delete(languageselectionfilename);
                    return;
                }
                else
                    using (FileStream fs = new(languageselectionfilename, FileMode.OpenOrCreate))
                    {
                        using (BinaryWriter bw = new(fs))
                        {
                            bw.Write(value.LCID);
                        }
                    }

            }
        }
    }
}
