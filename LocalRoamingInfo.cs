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
                                l.Add(new(r.ReadString(), r.ReadString(), r.ReadString(), r.ReadString()));
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

    }
}
