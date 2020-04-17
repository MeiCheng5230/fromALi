using PXin.Commu.Facade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PXin.WinService
{
    public partial class Service : ServiceBase
    {
        private ServerFacade _facade = new ServerFacade();
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _facade.StartServer();
        }

        protected override void OnStop()
        {
            _facade.StopServer();
        }
    }
}
