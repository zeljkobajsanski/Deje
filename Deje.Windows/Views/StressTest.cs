using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Deje.Windows.Model;
using Shell;
using Shell.Events;

namespace Deje.Windows.Views
{
    public partial class StressTest : ViewBase
    {
        private readonly List<BackgroundWorker> m_Workers = new List<BackgroundWorker>(); 

        private readonly BindingList<WorkerStat> m_WorkerStats = new BindingList<WorkerStat>(); 

        private List<string> m_Terms = new List<string>(); 

        public StressTest()
        {
            InitializeComponent();
            simpleButton1.Click += (s, e) => Start();
            simpleButton2.Click += (s, e) => Stop();
            if (!Directory.Exists("Results"))
            {
                Directory.CreateDirectory("Results");
            }
            workerStatBindingSource.DataSource = m_WorkerStats;
            UcitajFile();
        }

        private void UcitajFile()
        {
            using (var r = File.OpenText("stress.txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    m_Terms.Add(line);    
                }
            }
        }

        private void Stop()
        {
            m_Workers.ForEach((w) =>
            {
                if (w.IsBusy)
                {
                    w.CancelAsync();
                }
            });
        }

        private void Start()
        {
            m_WorkerStats.Clear();
            m_Workers.Clear();
            var results = Directory.GetFiles("Results");
            foreach (var result in results)
            {
                File.Delete(result);
            }
            var adresa = textEdit1.Text;
            if (String.IsNullOrEmpty(adresa))
            {
                OnAlertChanged(new WarningAlertEventArgs("Adresa servisa nije uneta"));
                return;
            }

            for (int i = 0; i < (int)spinEdit1.Value; i++)
            {
                var worker = new BackgroundWorker();
                m_Workers.Add(worker);
                worker.WorkerSupportsCancellation = true;
                var workerStat = m_WorkerStats.AddNew();
                workerStat.Name = "Worker " + (i + 1);
                worker.DoWork += (s, e) => Pretrazuj(workerStat);
                worker.RunWorkerCompleted += (s, e) =>
                {
                    var ws = workerStat;
                    marqueeProgressBarControl1.Invoke(
                        new Action(
                            () => marqueeProgressBarControl1.Properties.Stopped = true));
                    simpleButton1.Invoke(new Action(() => simpleButton1.Enabled = true));
                    simpleButton2.Invoke(new Action(() => simpleButton2.Enabled = false));
                };
                
                worker.RunWorkerAsync(workerStat);
            }
        }

        private void Pretrazuj(object workerStat)
        {
            simpleButton1.Invoke(new Action(() => simpleButton1.Enabled = false));
            simpleButton2.Invoke(new Action(() => simpleButton2.Enabled = true));
            var rand = new Random(DateTime.Now.Millisecond);
            
            marqueeProgressBarControl1.Invoke(new Action(() => marqueeProgressBarControl1.Properties.Stopped = false));
            var wc = new WebClient();
            var ws = (WorkerStat) workerStat;
            
            for (int i = 0; i < spinEdit2.Value; i++)
            {
                var t = rand.Next(m_Terms.Count);
                var adress = textEdit1.Text + "?latituda=45.2485584&longituda=19.8762081&razdaljina=20000&naziv=" + m_Terms[t];
                gridControl1.Invoke(new Action(ws.Start));
                try
                {
                    wc.DownloadFile(adress, "Results/" + Guid.NewGuid().ToString());
                }
                catch (WebException we)
                {
                    gridControl1.Invoke(new Action(() =>
                    {
                        ws.Errors++;
                    }));
                }
                gridControl1.Invoke(new Action(() =>
                {
                    ws.Requests++;
                    ws.Stop();
                    Visualize();
                }));
                
                Thread.Sleep(100);
            }
        }

        private void Visualize()
        {
            var requests = m_WorkerStats.Sum(x => x.Requests);
            textEdit2.EditValue = requests;
            var time = m_WorkerStats.Sum(x => x.ElapsedTime);
            textEdit3.EditValue = time;
            if (requests != 0)
            {
                var avg = time/(float) requests;
                textEdit4.EditValue = avg;
                gaugeControl1.Invoke(new Action(() => arcScaleComponent1.Value = avg));
            }
        }
    }
}
