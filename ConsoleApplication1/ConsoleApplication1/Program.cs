using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace ConsoleApplication1
{
    class Program
    {
        static BackgroundWorker bw = new BackgroundWorker();
        static void Main(string[] args)
        {
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.Disposed += new EventHandler(bw_Disposed);

            AutoResetEvent are = new AutoResetEvent(false);
            Console.WriteLine("任务开始");
            bw.RunWorkerAsync(are);

            Thread.Sleep(1000);
            are.Set();

            Console.Read();
        }
        static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} : {1}% completed", DateTime.Now.ToString("mm:ss"), e.ProgressPercentage));
        }

        static void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("任务完成了，结果是" + e.Result);
            Console.WriteLine("释放任务：");
            bw.Dispose();
        }
        static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            (e.Argument as AutoResetEvent).WaitOne();

            for (int j = 0; j <= 100; j += 20)
            {
                bw.ReportProgress(j);
                Thread.Sleep(500);
            }
            e.Result = 100;
        }
        static void bw_Disposed(object sender, EventArgs e)
        {
            Console.WriteLine("disposed");
        }
    }


}
