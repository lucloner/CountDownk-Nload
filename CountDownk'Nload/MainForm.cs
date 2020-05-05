using Amib.Threading;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountDownk_Nload
{
    public partial class MainForm : Form
    {
        private readonly static SmartThreadPool pool = new SmartThreadPool();
        private readonly static IWorkItemsGroup single = pool.CreateWorkItemsGroup(1);
        private readonly static List<IWorkItemResult> waitQueue = new List<IWorkItemResult>();
        private static string shorten = null;
        private const string regTips = "输入正则表达式选择目录:\r\n^   表示开头;\r\n$   表示结尾;\r\n\\d  表示任意数字;\r\n\\D  表示任意非数字;\r\n{n} 表示重复n次;\r\n*   表示无限次重复;\r\n其他请百度!\r\n";

        public MainForm()
        {
            InitializeComponent();
            this.toolTip.SetToolTip(this.btnSelect, regTips);
            this.toolTip.SetToolTip(this.btnSave, "将窗口信息保存为文本文件:" + saveFileFullName.FullName);
            Shown += (s, e) => Reset();
        }

        private void OnShown()
        {
            var sortedDirs = from o in Dirs orderby o.Key select o;
            Parallel.ForEach(Dirs.Keys, d =>
            {
                waitQueue.Add(pool.QueueWorkItem(() => Console.WriteLine("++" + Invoke(new invAddDir(clistDirs.Items.Add), new object[] { GetShorten(d), false }))));
            });
        }

        private static string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
        private delegate int invAddDir(object o, bool b);
        private void OnLoad()
        {
            Reset();
            var BaseDir = new DirectoryInfo(appPath);
            ShortenNames(BaseDir.FullName);
            Console.WriteLine("开始:" + BaseDir.FullName);
            ListDirs(BaseDir);
        }

        private readonly static ConcurrentDictionary<string, List<FileInfo>> Dirs = new ConcurrentDictionary<string, List<FileInfo>>();
        private readonly static ConcurrentDictionary<string, IEnumerable<string>> cacheLost = new ConcurrentDictionary<string, IEnumerable<string>>();
        private readonly static ConcurrentDictionary<string, IEnumerable<FileInfo>> cacheBrowse = new ConcurrentDictionary<string, IEnumerable<FileInfo>>();
        private void ListDirs(DirectoryInfo dir)
        {
            var dirs = dir.GetDirectories();
            Console.WriteLine("读取目录信息:" + dirs.Length);
            if (dirs != null && dirs.Length > 0)
            {
                Parallel.ForEach(dirs, d => FoundDir(d));
            }

        }
        private void FoundDir(DirectoryInfo dir)
        {
            Console.WriteLine("=>" + AddDir(dir));
            ListDirs(dir);
            ShortenNames(dir.FullName);
        }

        private delegate void invRemove(object dir);
        private string AddDir(DirectoryInfo dir)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            Dirs["" + dir.FullName] = fileInfos;
            var maxPb = new invClear(MaxProcess);
            var add = new invClear(AddProcess);
            single.QueueWorkItem(() => Invoke(maxPb));
            var remove = new invRemove(clistDirs.Items.Remove);
            waitQueue.Add(pool.QueueWorkItem(() =>
            {
                var files = dir.GetFiles();
                Console.WriteLine("读取文件信息:" + files.Length);
                if (files != null && files.Length > 0)
                {
                    fileInfos.AddRange(files);
                    Console.WriteLine("+>" + files.Length);
                }
                else
                {
                    Dirs.TryRemove(dir.FullName, out fileInfos);
                    single.QueueWorkItem(() => Invoke(remove, new object[] { GetShorten(dir.FullName) }));
                }
                single.QueueWorkItem(() => Invoke(add));
            }));
            return dir.FullName;
        }

        private delegate void invClear();
        private delegate void invAddRang(object[] items);
        private delegate void invSetStatue(string s);
        private bool cyborgCheck = false;
        private void ClistDirs_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (cyborgCheck)
            {
                return;
            }
            var clist = (CheckedListBox)sender;
            var selected = clist.SelectedItem;
            CheckFiles(selected, clist.GetItemChecked(clist.SelectedIndex));
        }

        private void CheckFiles()
        {
            CheckFiles(null, false);
        }

        private void CheckFiles(object checkedObject = null, bool isChecked = false)
        {
            cyborgCheck = false;
            var clist = new ArrayList(clistDirs.CheckedItems);
            var setStatus = new invClear(ChangeStatus);
            if (checkedObject != null && !isChecked && !clist.Contains(checkedObject))
            {
                clist.Add(checkedObject);
            }
            else
            {
                clist.Remove(checkedObject);
            }
            var waitFor = waitQueue.ToArray<IWorkItemResult>();
            var browseClear = new invClear(listBrowse.Items.Clear);
            var lostClear = new invClear(listLost.Items.Clear);
            var browseAddRange = new invAddRang(listBrowse.Items.AddRange);
            var lostAddRange = new invAddRang(listLost.Items.AddRange);
            var maxPb = new invClear(MaxProcess);
            var add = new invClear(AddProcess);
            Invoke(maxPb);
            var work = pool.QueueWorkItem(() =>
              {
                  var browse = new List<FileInfo>();
                  var lost = new List<string>();
                  SmartThreadPool.WaitAll(waitFor);
                  waitQueue.RemoveAll(q => waitFor.Contains<IWorkItemResult>(q));
                  Parallel.For(0, clist.Count, i =>
                  {
                      Invoke(maxPb);
                      try
                      {
                          var item = shorten + clist[i].ToString();
                          if (cacheBrowse.ContainsKey(item) && cacheLost.ContainsKey(item))
                          {
                              browse.AddRange(cacheBrowse[item]);
                              lost.AddRange(cacheLost[item]);
                              return;
                          }
                          var files = from f in Dirs[item] orderby f.FullName descending select f;

                          if (files == null || files.Count<FileInfo>() == 0)
                          {
                              return;
                          }
                          Console.WriteLine($"==>{item.GetType()} {item} {files.Count<FileInfo>()}");

                          browse.AddRange(files);
                          var thislLost = CheckContinuous(files.ToArray<FileInfo>());
                          lost.AddRange(thislLost);
                          cacheBrowse[item] = files;
                          cacheLost[item] = thislLost;
                      }
                      catch (Exception ex)
                      {
                          System.Console.WriteLine(ex.Message + ex.StackTrace);
                      }
                      finally
                      {
                          Invoke(add);
                      }

                  });
                  Invoke(browseClear);
                  Invoke(lostClear);
                  Invoke(browseAddRange, new object[] { Array.ConvertAll<FileInfo, string>(browse.ToArray(), f => GetShorten(f.FullName)) });
                  Invoke(lostAddRange, new object[] { Array.ConvertAll<string, string>(lost.ToArray(), s => GetShorten(s)) });
                  single.QueueWorkItem(() => Invoke(setStatus));
                  Invoke(add);
              });
            waitQueue.Add(work);
            Invoke(new invProcess(AddProcess), new object[] { 100 });
            waitQueue.Add(pool.QueueWorkItem(() =>
            {
                SmartThreadPool.WaitAll(new IWaitableResult[] { work });
                Invoke(new invClear(ResetProcess));
                single.QueueWorkItem(() => Invoke(setStatus));
                //Interaction.MsgBox("查询完成！", MsgBoxStyle.Information);
            }));
        }

        private IEnumerable<string> CheckContinuous(FileInfo[] files)
        {
            var lostList = new List<string>();
            var firstFile = files.First();
            var filesToNums = new ConcurrentDictionary<int, FileInfo>();
            Invoke(new invProcess(MaxProcess), new object[] { files.Length });
            Parallel.ForEach(files, f =>
            {
                filesToNums[PickNums(((FileInfo)f).Name)] = f;
                Invoke(new invClear(AddProcess));
            });
            var sortedNums = from num in filesToNums.Keys orderby num select num;
            var iStart = 1 + 10 * (int)(sortedNums.First() / 10);
            var iEnd = sortedNums.First() - 1;
            if (iStart <= iEnd)
            {
                lostList.Add(firstFile.DirectoryName + lostSeparator + iStart + (iStart < iEnd ? "" + lostToSeparator + iEnd : ""));
            }
            else
            {
                lostList.Add("");
            }
            Invoke(new invProcess(MaxProcess), new object[] { sortedNums.Count() - 1 });
            for (int k = 1; k < sortedNums.Count(); k++)
            {
                var kNum = sortedNums.ElementAt(k);
                if (!sortedNums.Contains(iStart))
                {
                    iEnd = kNum - 1;
                    var lostMsg = firstFile.DirectoryName + lostSeparator + iStart;
                    if (iStart < iEnd)
                    {
                        lostMsg = firstFile.DirectoryName + lostSeparator + iStart + lostToSeparator + iEnd;
                        lostList.RemoveAt(lostList.Count - 1);
                    }
                    lostList.Add(lostMsg);
                }
                iStart = kNum + 1;
                Invoke(new invClear(AddProcess));
            }
            lostList.RemoveAll(s => s.Length == 0);
            return lostList;
        }

        private int PickNums(string s)
        {
            var i = 0;
            MatchCollection maches = Regex.Matches(s, @"\d");
            foreach (var m in maches)
            {
                i = i * 10 + int.Parse("" + m);
                //System.Console.WriteLine($"000>{m},{i}");
            }

            return i;
        }

        private void ShortenNames(string s)
        {
            if (shorten == null)
            {
                shorten = appPath != s ? s : shorten;
                return;
            }
            var min = Math.Min(shorten.Length, s.Length);
            var shortenPos = min;
            Parallel.For(0, min - 1, i =>
              {
                  try
                  {
                      if (shorten[i] != s[i])
                      {
                          shortenPos = Math.Min(shortenPos, i);
                      }
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex.Message + ex.StackTrace);
                  }
              });
            shorten = shorten.Substring(0, Math.Min(shorten.Length, shortenPos));
        }

        private string GetShorten(string s) => ("" + s).Replace(shorten, "");

        private static readonly FolderBrowserDialog openDir = new FolderBrowserDialog();
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            btnOpen.Enabled = false;
            openDir.SelectedPath = appPath;
            if (openDir.ShowDialog() == DialogResult.OK)
            {
                appPath = openDir.SelectedPath;
                OnLoad();
                OnShown();
                var works = waitQueue.ToArray();
                waitQueue.Add(pool.QueueWorkItem(() =>
                {
                    try
                    {
                        if (works != null && works.Length > 0)
                        {
                            SmartThreadPool.WaitAll(works);
                            waitQueue.RemoveAll(q => works.Contains(q));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                    Invoke(new invProcess(AddProcess), new object[] { 100 });
                }));
                single.QueueWorkItem(() => Invoke(new invClear(ChangeStatus)));
            }
            btnOpen.Enabled = true;
        }

        private void Reset()
        {
            shorten = null;
            clistDirs.Items.Clear();
            listBrowse.Items.Clear();
            listLost.Items.Clear();
            lblStatus.Text = "空";
            cacheBrowse.Clear();
            cacheLost.Clear();
            ResetProcess();
        }

        private void ChangeStatus()
        {
            if (DateTime.Now.Ticks - now < 100000 && pbMain.Visible)
            {
                return;
            }
            now = DateTime.Now.Ticks;
            var status = new StringBuilder($"选中目录:{clistDirs.CheckedItems.Count}个 ");
            if (listBrowse.Items.Count > 0)
            {
                status.Append($"找到文件:{listBrowse.Items.Count}个 ");
            }
            var lostCnt = listLost.Items.Count;
            if (listLost.Items.Count > 0)
            {
                status.Append($"发现缺失文件:{lostCnt}处 ");
            }
            var cnt = lostCnt;
            Parallel.For(0, lostCnt, i =>
            {
                var item = "" + listLost.Items[i];
                if (item.Contains(lostToSeparator))
                {
                    var lostParam = item.Split(lostToSeparator);
                    var lostEnd = lostParam[1];
                    var lostStart = lostParam[0];
                    lostStart = lostStart.Substring(lostStart.IndexOf(lostSeparator) + lostSeparator.Length);
                    cnt += int.Parse(lostEnd) - int.Parse(lostStart);
                }
            });
            if (cnt > 0)
            {
                status.Append($"共计文件:{cnt}个 ");
            }
            lblStatus.Text = status.ToString().TrimEnd();
        }

        private void ClistDirs_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine($"aaa>{e.GetType()}");
            var item = shorten + ((ListBox)sender).SelectedItem;
            // opens the folder in explorer
            Process.Start(item);
        }

        private void BtnClear_Click(object sender, EventArgs e) => Reset();

        private void BtnAll_Click(object sender, EventArgs e)
        {
            btnAll.Enabled = false;
            ResetProcess();
            cyborgCheck = true;
            var cnt = clistDirs.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                MaxProcess();
                clistDirs.SetItemChecked(i, true);
                AddProcess();
            }
            CheckFiles();
            btnAll.Enabled = true;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            btnSelect.Enabled = false;
            ResetProcess();
            var reg = Interaction.InputBox(regTips, "多选目录");
            var failed = false;
            cyborgCheck = true;
            var cnt = clistDirs.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                try
                {
                    var item = clistDirs.Items[i].ToString();
                    if (Regex.IsMatch(item, reg))
                    {
                        clistDirs.SetItemChecked(i, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                    failed = true;
                }
            }
            pool.QueueWorkItem(() => CheckFiles());
            if (failed)
            {
                Interaction.MsgBox($"输入{reg}出现错误！已经尝试跳过错误!", MsgBoxStyle.Critical, "警告");
            }
            btnSelect.Enabled = true;
        }
        private delegate void invProcess(int value);
        private void MaxProcess()
        {
            MaxProcess(1);
        }
        private void MaxProcess(int value = 1)
        {
            pbMain.Maximum += (pbMain.Visible ? 0 : 100) + value;
            Invoke(new invVisible(SetVisible), new object[] { true });
            Invoke(new invClear(ShowProcess));
        }

        private void AddProcess()
        {
            AddProcess(1);
        }
        private void AddProcess(int value = 1)
        {
            try
            {
                pbMain.Value += value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                pbMain.Value = pbMain.Maximum;
            }
            Invoke(new invClear(ShowProcess));
        }

        private void ShowProcess()
        {
            var v = pbMain.Value;
            var m = pbMain.Maximum;
            Invoke(new invVisible(SetVisible), new object[] { v != m });
            if (pbMain.Visible && DateTime.Now.Ticks - now <= 100000)
            {
                return;
            }
            double p = m <= 0 ? 0 : v * 100 / m;
            now = DateTime.Now.Ticks;
            toolTip.SetToolTip(pbMain, $"完成:{v} 总数:{m} 完成度:{p}%");
        }

        long now = DateTime.Now.Ticks;
        private void ResetProcess()
        {
            pbMain.Minimum = 0;
            pbMain.Maximum = 0;
            pbMain.Value = 0;
            Invoke(new invVisible(SetVisible), new object[] { false });
        }

        private void BtnClearSelect_Click(object sender, EventArgs e)
        {
            btnClearSelect.Enabled = false;
            cyborgCheck = true;
            Parallel.For(0, clistDirs.Items.Count, i =>
             {
                 clistDirs.SetItemChecked(i, false);
             });
            cyborgCheck = false;
            Invoke(new invClear(ResetProcess));
            Invoke(new invClear(listBrowse.Items.Clear));
            Invoke(new invClear(listLost.Items.Clear));
            btnClearSelect.Enabled = true;
        }

        private delegate void invVisible(bool isVisible);
        public void SetVisible(bool isVisible)
        {
            pbMain.Visible = isVisible;
        }

        private const string lostSeparator = "缺=>";
        private const char lostToSeparator = '…';
        private void ListBrowse_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = "" + ((ListBox)sender).SelectedItem;
            var index = selected.LastIndexOf(Path.DirectorySeparatorChar);
            selected = selected.Substring(0, index > 0 ? index : selected.Length);
            index = clistDirs.FindString(selected);
            if (index == ListBox.NoMatches)
            {
                return;
            }
            clistDirs.SelectedIndex = index;

        }

        private void ListLost_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = "" + ((ListBox)sender).SelectedItem;
            selected = selected.Split(lostSeparator[0])[0];
            var index = selected.LastIndexOf(Path.DirectorySeparatorChar);
            selected = selected.Substring(0, index > 0 ? index : selected.Length);
            index = listBrowse.FindString(selected);
            if (index == ListBox.NoMatches)
            {
                return;
            }
            listBrowse.SelectedIndex = index;
        }

        private const string saveFile = "save.txt";
        private static readonly FileInfo saveFileFullName = new FileInfo(appPath + saveFile);
        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            var save = new StringBuilder($"\r\n==当前文件夹/前缀缩写:{shorten}\r\n目录列表:\r\n");
            var wait = waitQueue.ToArray();
            waitQueue.Add(single.QueueWorkItem(() =>
            {
                try
                {
                    SmartThreadPool.WaitAll(wait);
                    waitQueue.RemoveAll(q => wait.Contains(q));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
                var p=Parallel.For(0, clistDirs.Items.Count, i => save.Append(clistDirs.Items[i] + "\r\n"));
                while (!p.IsCompleted)
                {
                    Thread.Sleep(1000);
                }
                save.Append("\r\n文件列表:\r\n");
                p = Parallel.For(0, listBrowse.Items.Count, i => save.Append(listBrowse.Items[i] + "\r\n"));
                while (!p.IsCompleted)
                {
                    Thread.Sleep(1000);
                }
                save.Append("\r\n缺失列表:\r\n");
                p = Parallel.For(0, listLost.Items.Count, i => save.Append(listLost.Items[i] + "\r\n"));
                while (!p.IsCompleted)
                {
                    Thread.Sleep(1000);
                }
                save.Append("\r\n报告:\r\n");
                save.Append(lblStatus.Text);

                var writer = saveFileFullName.AppendText();
                var reader = new StringReader(save.ToString() + $"\r\n{DateTime.Now}完成==\r\n");
                int len = 4096;
                char[] strToWrite = new char[len];
                while (0 < (len = reader.Read(strToWrite, 0, 4096)))
                {
                    writer.Write(strToWrite,0,len);
                }
                writer.Close();
                reader.Close();
                save.Clear();
            }));
            btnSave.Enabled = true;
        }
    }
}
