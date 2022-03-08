#region <remarks> using namespace </remarks>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

#endregion

namespace ECS.Core.Util
{
    /// <summary> class : CsvFile - control class </summary>
    #region <remarks> class : CsvFile </remarks>

    public class CsvFile : IDisposable
    {
        #region <remarks> attributes of class CsvFile : private </remarks>

        private List<string> list = new List<string>();
        private FileStream fs = null;

        #endregion
        #region <remarks> attributes of class CsvFile : public </remarks>


        #endregion

        #region <remarks> initialize of class CsvFile : constructor </remarks>

        public CsvFile()
        {
        }

        #endregion
        #region <remarks> initialize of class CsvFile : destructor </remarks>

        public void Dispose()
        {
            if (fs != null) { fs.Close(); }

            list.Clear();
            list = null;
        }

        #endregion

        #region <remarks> operations of class CsvFile : Open </remarks>

        public bool Open(string file)
        {
            bool res = true;

            try
            {
                string path = Path.GetDirectoryName(file);

                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                fs = new FileStream(file, FileMode.Create);

                return true;
            }
            catch (Exception)
            {
                res = false;
            }

            return res;
        }

        #endregion
        #region <remarks> operations of class CsvFile : Close </remarks>

        public bool Close()
        {
            bool res = false;

            try
            {
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }

                res = true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                res = false;
            }

            return res;
        }

        #endregion

        #region <remarks> operations of class CsvFile : Read </remarks>

        public List<string> ReadFile(string file)
        {
            List<string> res = null;

            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    res = new List<string>();

                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        res.Add(line);
                    }

                    sr.Close();
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                res = null;
            }

            return res;
        }

        #endregion
        #region <remarks> operations of class CsvFile : Write </remarks>

        public bool Write()
        {
            bool res = false;

            try
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);

                    int count = 0;
                    int i = 0;

                    count = list.Count;

                    for (i = 0; i < count; i++)
                    {
                        sw.WriteLine(list[i]);
                    }

                    sw.Flush();
                    sw.Close();

                    res = true;
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
                res = false;
            }

            return res;
        }

        #endregion

        #region <remarks> operations of class CsvFile : AddRow </remarks>

        public void AddRow(List<string> src)
        {
            try
            {
                if (fs == null || src.Count <= 0) { return; }

                string line = null;
                string temp = null;

                int count  = src.Count;
                int i = 0;

                for (i = 0; i < count; i++)
                {
                    temp = String.Format("{0}{1}{0}", (char)34, src[i]);
                    if (i < count - 1) { temp += ","; }

                    line += temp;
                }

                list.Add(line);
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        #endregion
        #region <remarks> operations of class CsvFile : AddRowNoQuotations </remarks>

        public void AddRowNoQuotations(List<string> src)
        {
            try
            {
                if (fs == null || src.Count <= 0) { return; }

                string line = null;
                string temp = null;

                int count = src.Count;;
                int i = 0;

                for (i = 0; i < count; i++)
                {
                    temp = String.Format("{0}", src[i]);
                    if (i < count - 1) { temp += ","; }

                    line += temp;
                }
                
                list.Add(line);

            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        #endregion
        #region <remarks> operations of class CsvFile : AddRowNoQuotationsToBak </remarks>

        //    DESCRIPT : 데이터를 저장할 파일을 사용자가 이미 Access하고 있는 경우를 대비하여 Bak파일에 저장 후 원본파일로 덮어쓰기 한다.
        //               이때는 사용자가 이미 사용중일때는 덮어쓰기를 실패하지만 그 외의 경우에는 저장된다.
        public void AddRowNoQuotationsToBackup(string file, List<string> captions, List<string> fieldData)
        {
            string path = Path.GetDirectoryName(file);
            string back = Path.Combine(path, Path.GetFileNameWithoutExtension(file) + ".bak");

            try
            {
                FileInfo org = new FileInfo(back);

                if (Directory.Exists(path) == false)
                {
                    DirectoryInfo dir = Directory.CreateDirectory(path);

                    fs = new FileStream(back, FileMode.Create);
                    AddRowNoQuotations(captions);
                    AddRowNoQuotations(fieldData);
                }
                else
                {
                    if (org.Exists == true)
                    {
                        fs = new FileStream(back, FileMode.Append);
                        AddRowNoQuotations(fieldData);
                    }
                    else
                    {
                        fs = new FileStream(back, FileMode.Create);
                        AddRowNoQuotations(captions);
                        AddRowNoQuotations(fieldData);
                    }
                }

                Write();
                org.CopyTo(file, true);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                Write();
            }

            FileInfo dest = null;
            FileInfo info = null;

            try
            {
                string[] files = Directory.GetFiles(path, "*.bak");

                foreach (string src in files)
                {
                    try
                    {
                        if (src == back) { continue; }

                        dest = new FileInfo(src);
                        if (dest.Exists == false) { continue; }

                        info = new FileInfo(Path.Combine(path, Path.GetFileNameWithoutExtension(dest.FullName) + ".csv"));

                        if (info.Exists == false)
                        {
                            dest.CopyTo(info.FullName, true);
                            dest.Delete();

                            continue;
                        }

                        if (dest.LastWriteTime > info.LastWriteTime)
                        {
                            dest.CopyTo(info.FullName, true);
                        }

                        dest.Delete();
                    }
                    catch (Exception e)
                    {
                        string msg = e.Message;
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
        }

        #endregion
    }

    #endregion
}
