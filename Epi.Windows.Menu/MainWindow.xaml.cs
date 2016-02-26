﻿using Epi.Windows.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epi.Windows.Menu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :System.Windows.Controls.UserControl
    {
        MainForm mainform = null;
        public event EventHandler<RoutedEventArgs> Close;
        /// <summary>
        /// Redefined Module property
        /// </summary>      

        # region Constructor

        public MainWindow()           
        {
            InitializeComponent();          
                ApplicationIdentity appId = new ApplicationIdentity(typeof(Configuration).Assembly);               
                this.tsslLocale.Text = Thread.CurrentThread.CurrentUICulture.Name;
                this.tsslVersion.Text = appId.Version;
                mainform = new MainForm();
        }

        #endregion

        private void button_MouseOver(object sender, EventArgs e)
        {         
        }

        private void enterData_Click(object sender, EventArgs e)
        {
            LoadModule("ENTER");
        }

        private void createMaps_Click(object sender, EventArgs e) 
        {
            LoadModule("EPIMAP");
        }

        private void statCalc_Click(object sender, EventArgs e)
        {
            LoadModule("StatCalc");
        }

        private void classicAnalyze_Click(object sender, EventArgs e)
        {
            LoadModule("ANALYSIS");
        }

        private void visualAnalyze_Click(object sender, EventArgs e)
        {
            LoadModule("DASHBOARD");
        }

        private void createForms_Click(object sender, MouseButtonEventArgs e)
        {
            LoadModule("MAKEVIEW");
        }

        //Link for Epi Info Website - located in footer
        private void Hyperlink_epiInfoWebsite(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void contents_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.cdc.gov/epiinfo/user-guide/index.htm");
        }

        private void howToVideo_Click(object sender, EventArgs e)
        {
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load("http://ftp.cdc.gov/pub/software/epi_info/sites.xml");
                List<System.Xml.XmlElement> elements = new List<System.Xml.XmlElement>();
                foreach (System.Xml.XmlElement element in doc.DocumentElement.ChildNodes)
                {
                    if (element.Name.Equals("videos"))
                    {
                        string url = element.InnerText;
                        System.Diagnostics.Process.Start(url);
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void epiInfoQA_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://epiinfo.atlassian.net/wiki/questions");
        }

        private void contactHelpDesk_Click(object sender, EventArgs e)
        {
            Epi.Windows.MsgBox.ShowInformation(SharedStrings.HELPDESK_LOGIN);
            System.Diagnostics.Process.Start("https://epiinfo.atlassian.net/servicedesk/customer/portal/1");
        }

        private void activEpi_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://activepi.com");
        }

        private void openEpi_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://openepi.com");
        }

        private void epi_InfoLogs_Click(object sender, EventArgs e)
        {
            string logFilePath = Logger.GetLogFilePath();
            //WinUtil.OpenTextFile(Logger.GetLogFilePath());

            if (File.Exists(logFilePath))
            {
                string containingFolder = System.IO.Path.GetDirectoryName(logFilePath);
                System.Diagnostics.Process.Start(containingFolder);
            }
            else if (Directory.Exists(logFilePath))
            {
                System.Diagnostics.Process.Start(logFilePath);
            }
        }

        private void options_Click(object sender, EventArgs e)
        {
            OnOptionsClicked(OptionsDialog.OptionsTabIndex.General);
        }

        /// <summary>
        /// Handles the Click event of the About Epi Info menu item
        /// </summary>
        protected void OnAboutClicked()
        {
            if (mainform == null)
                mainform = new MainForm();
            AboutEpiInfoDialog dlg = new AboutEpiInfoDialog(mainform);
            dlg.ShowDialog();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            /*if (mainform == null)
                mainform = new MainForm();
            IModuleManager manager = mainform.Module.GetService(typeof(IModuleManager)) as IModuleManager;
            if (manager != null)
            {
                manager.UnloadAll();
            }*/          
            //((System.Windows.Threading.DispatcherObject)(this.Parent)).Dispatcher.InvokeShutdown();   
            if (Close != null)
                Close(this, new RoutedEventArgs());   
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private void LoadModule(string moduleType)
        {
            try
            {
               // BeginBusy(SharedStrings.LOADING_MODULE);

                string commandText = null;

                switch (moduleType/*.ToUpper()*/) // to fix a problem for Turkish users where ToUpper doesn't do what it's supposed to do
                {
                    case "ENTER":
                        commandText = Environment.CurrentDirectory + "\\Enter.exe";
                        break;
                    case "MAKEVIEW":
                        commandText = Environment.CurrentDirectory + "\\MakeView.exe";
                        break;
                    case "ANALYSIS":
                        commandText = Environment.CurrentDirectory + "\\Analysis.exe";
                        break;
                    case "DASHBOARD":
                        commandText = Environment.CurrentDirectory + "\\AnalysisDashboard.exe";
                        break;
                    case "EPIMAP":
                        commandText = Environment.CurrentDirectory + "\\Mapping.exe";
                        break;
                    case "EPIREPORT":
                        commandText = Environment.CurrentDirectory + "\\EpiReport.exe";
                        break;
                    case "StatCalc":
                        commandText = Environment.CurrentDirectory + "\\StatCalc.exe";
                        break;
                }

                if (!string.IsNullOrEmpty(commandText))
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = commandText;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();
                }

                /*
                // service locator pattern
                IModuleManager manager = this.Module.GetService(typeof(IModuleManager)) as IModuleManager;
                if (manager == null)
                {
                    MessageBox.Show("Module manager service not registered."); // TODO: Hard coded string
                    return;
                }

                IModule module = manager.CreateModuleInstance(moduleType);
                module.Load(manager, null);
                 */
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                EndBusy();
            }
        }

        public void BeginBusy(string str)
        {
            UpdateStatus(str, false);
            this.Cursor = Cursors.Wait;
        }
        /// <summary>
        /// Sets the cursor to normal and all status messages to default
        /// </summary>
        public void EndBusy()
        {
            this.Cursor = Cursors.None;
            UpdateStatus(SharedStrings.READY, false);
        }

        public void UpdateStatus(string str, bool shouldAddLogEntry, bool doEvents = false)
        {
           // this.tsslMessage.Text = str;
            if (shouldAddLogEntry)
            {
                Logger.Log(DateTime.Now + ":  " + str);
            }
            if (doEvents)
            {
               // Application.do();
            }
        }
     

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnOptionsClicked(OptionsDialog.OptionsTabIndex.General);
        }

        private void btnLocaleClick(object sender, RoutedEventArgs e)
        {
            OnOptionsClicked(OptionsDialog.OptionsTabIndex.General);
        }

        /// <summary>
        /// Handles the Click event of the Options menu item
        /// </summary>
        /// <param name="tabIndex">The index of the options dialog tab </param>
        protected void OnOptionsClicked(OptionsDialog.OptionsTabIndex tabIndex)
        {
            if (mainform == null)
                mainform = new MainForm();
            OptionsDialog dlg = new OptionsDialog(mainform, tabIndex);
            try
            {
                dlg.ApplyChanges += new OptionsDialog.ApplyChangesHandler(OnApplyChanges);
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    OnOptionsChanged();
                }
            }
            finally
            {
                dlg.Close();
                dlg.Dispose();
            }
        }

        /// <summary>
        /// On Apply Changes
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="args">Arguments</param>
        protected virtual void OnApplyChanges(object sender, EventArgs args)
        {
            OnOptionsChanged();
        }

        /// <summary>
        /// Called when Option settings are changed by the user.
        /// </summary>
        protected virtual void OnOptionsChanged()
        {
        }

        private void aboutEpiInfo_click(object sender, MouseEventArgs e)
        {
            OnAboutClicked();
        }

        private void btnRightMenuHide_Click(object sender, RoutedEventArgs e)
        {

            var pnlRightMenuHeight = pnlRightMenu.Height.ToString();

            if (pnlRightMenuHeight == "0")
            {
                this.BeginStoryboard(FindResource("sbShowRightMenu") as Storyboard);

            }
            else if (pnlRightMenuHeight == "200")
            {
                this.BeginStoryboard(FindResource("sbHideRightMenu") as Storyboard);


            }
            else if (pnlRightMenuHeight == "240")
            {
                this.BeginStoryboard(FindResource("sbHideRightMenu") as Storyboard);


            }

            //MessageBox.Show("Hide");
        }

        private void Other_EpiResource_Click(object sender, EventArgs e)
        {
            var submenuEpiResoucesHeight = submenuEpiResouces.Height.ToString();

            if (submenuEpiResoucesHeight == "0")
            {
                this.BeginStoryboard(FindResource("showEpiResource") as Storyboard);


            }
            else if (submenuEpiResoucesHeight == "50")
            {
                this.BeginStoryboard(FindResource("hideEpiResource") as Storyboard);



            }
        }    
    }
}