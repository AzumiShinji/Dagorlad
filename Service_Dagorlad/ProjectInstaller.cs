﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Service_Dagorlad
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            //InitializeComponent();
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller1.ServiceName = "Service_of_Dagorlad";
            serviceInstaller1.DisplayName = "Service of Dagorlad";
            serviceInstaller1.Description = "Service of Dagorlad for Hosting";
            serviceInstaller1.StartType = ServiceStartMode.Automatic;
            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);
        }
    }
}
