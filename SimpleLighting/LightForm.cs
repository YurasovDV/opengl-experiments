using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleLighting
{
    public partial class LightForm : Form
    {
        public Object LockObj { get; set; }

        SendOrPostCallback refreshMethod;

        public LightForm()
        {
            InitializeComponent();

            LockObj = new object();

            refreshMethod = new SendOrPostCallback((state) =>
                {
                    if (!portraitControl.IsDisposed && !portraitControl.Disposing)
                    {
                        portraitControl.MakeCurrent();
                        portraitControl.SwapBuffers();
                        portraitControl.Invalidate();
                        portraitControl.Context.MakeCurrent(null);
                    }

                });
        }

        private void LightForm_Load(object sender, EventArgs e)
        {

            var syncContext = WindowsFormsSynchronizationContext.Current;

            if (syncContext != null)
            {
                Action refresh = () =>
                {
                    lock (LockObj)
                    {
                        syncContext.Post(refreshMethod, null);
                    }
                };

                Engine en = new Engine(portraitControl, refresh, LockObj);
                portraitControl.Context.MakeCurrent(null);
                en.Start();
            }
        }
    }
}
