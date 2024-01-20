using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA2;
using System.Diagnostics;
using Gma.System.MouseKeyHook;
using System.Linq;
using System.Windows.Forms;
using System.Windows;
using System;
using CasePaster.Properties;
using Clipboard = System.Windows.Forms.Clipboard;
using Squirrel;
using System.Threading.Tasks;




namespace CasePaster
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;

        public Form1()
        {
            InitializeComponent();
            Subscribe();
            LoadSettings();
            CheckFormUpdates();
        }

        private async Task CheckFormUpdates()
        {   //TODO update repo addy / config file
            using (var manager = new UpdateManager(@"githubREPO"))
            {
                await manager.UpdateApp();
            }
            
        }

        public void Main_Function()
        {
            string caseNumberToPaste = "";
            string DRbuttonID = "1452";
            string processSpillman = "Mobile";
            string processEticket = "CT.ECWS.WinForms";
            string CaseNumberBoxID = "txtCADNumber";
            var retrySetting = new RetrySettings()
            {
                Timeout = TimeSpan.FromMilliseconds(1500),
                Interval = TimeSpan.FromMilliseconds(200),
            };

            using (var automation = new UIA2Automation())
            {
                //look for Spillman Mobile Process
                var process1 = Process.GetProcessesByName(processSpillman).FirstOrDefault();

                if (process1 == null) //Exit and notifiction if not found
                {
                    BalloonNotification("Spillman process not found.");
                    return;
                }

                using (var appMobile = FlaUI.Core.Application.Attach(process1))
                {
                    var windows = Retry.Find(() => appMobile.GetAllTopLevelWindows(automation), retrySetting);

                    if (windows != null)
                    {
                        AutomationElement window = windows.FirstOrDefault(w => w.Name.Contains("Flex Mobile"));

                        if (window.IsOffscreen)
                        {
                            window.Patterns.Window.Pattern.SetWindowVisualState(FlaUI.Core.Definitions.WindowVisualState.Normal);
                            window.WaitUntilClickable();
                        }
                        window.Focus();

                        //which button to click based on Case # Search setting                        
                        AutomationElement tabs;

                        var pane = Retry.Find(() => window.FindFirstDescendant(c => c.ByName("CADBar")), retrySetting);

                        var buttonToClick = Retry.Find(() => pane.FindFirstDescendant(cd => cd.ByName("My Call")), retrySetting);

                        if (buttonToClick.IsEnabled == false)
                        {
                            BalloonNotification("You do not appear to be on a call", "FAILED");

                            return;
                        }

                        int j = 0;
                        do
                        {
                            buttonToClick.Click();
                            j++;

                        } while (j < 3);

                        tabs = Retry.Find(() => window.FindFirstDescendant(cf => cf.ByAutomationId("1120")), retrySetting);

                        var TabToClick = Retry.Find(() => tabs.FindFirstDescendant(cf => cf.ByName("Other")), retrySetting).AsTabItem();

                        TabToClick.WaitUntilClickable(new TimeSpan(500));

                        int i = 0;
                        do
                        {
                            TabToClick.Click();
                            i++;

                        } while (TabToClick.IsSelected == false && i < 3);

                        if (!TabToClick.IsSelected)
                        {
                            BalloonNotification("Could not locate an open call", "FAILED");
                            return;
                        }

                        var DrButton = Retry.Find(() => window.FindFirstDescendant(cf => cf.ByAutomationId(DRbuttonID)), retrySetting);

                        if (DrButton != null)
                        {
                            BalloonNotification(DrButton.Name);
                            caseNumberToPaste = DrButton.Name;
                            if (Settings.Default.Clipboard) { Clipboard.SetText(DrButton.Name); }
                        }
                        else
                        {
                            BalloonNotification("DR number was not located...", "FAILED");
                        }                        

                    }
                    else
                    {
                        BalloonNotification("Spillman window not found.");
                    }
                }

                //if Add to eTicket is enabled, run this seciton of code
                if (Settings.Default.eticket)
                {
                    var process2 = Process.GetProcessesByName(processEticket).FirstOrDefault();

                    if (process2 == null)
                    {
                        BalloonNotification("eTicket process was not found.");
                        return;
                    }

                    //look for ticket windown
                    using (var appTicket = FlaUI.Core.Application.Attach(process2))
                    {
                        var DashBoard = Retry.Find(() => appTicket.GetAllTopLevelWindows(automation), retrySetting);

                        if (DashBoard != null && DashBoard[0].AsWindow().ModalWindows.Count() > 0)
                        {
                            var Ticket = DashBoard[0].AsWindow().ModalWindows.First(w => w.Name == "Ticket");

                            if (Ticket.IsOffscreen)
                            {
                                Ticket.Patterns.Window.Pattern.SetWindowVisualState(FlaUI.Core.Definitions.WindowVisualState.Normal);
                            }

                            Ticket.Focus();

                            var tabs = Retry.Find(() => Ticket.FindFirstDescendant(cf => cf.ByAutomationId("tabTicket")), retrySetting);

                            var NotesTab = Retry.Find(() => tabs.FindFirstDescendant(cf => cf.ByName(@"Notes/Comments")), retrySetting).AsTabItem();

                            NotesTab.WaitUntilClickable(new TimeSpan(500));
                            AutomationElement CaseBox;
                            int i = 0;
                            do
                            {
                                NotesTab.Click();
                                CaseBox = Retry.Find(() => Ticket.FindFirstDescendant(cf => cf.ByAutomationId(CaseNumberBoxID)), retrySetting);
                                i++;
                            } while (CaseBox == null || i < 3);

                            if (CaseBox == null)
                            {
                                BalloonNotification("eTicket error", "FAILED");
                                return;
                            }
                            CaseBox.AsTextBox().Text = caseNumberToPaste;
                        }
                        else
                        {
                            BalloonNotification("Ticket Window Not Located", "FAILED");
                        }


                    }


                }

            }

        }

        private void LoadSettings()
        {
            this.checkBoxClipBoard.Checked = Settings.Default.Clipboard;
            this.checkBox_eTicket.Checked = Settings.Default.eticket;

        }

        private void BalloonNotification(string message, string title = "Case Number:")
        {
            if (this.notifyIcon1.Visible == false)
            {
                this.notifyIcon1.Visible = true;
            }
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = message;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.None;
            notifyIcon1.ShowBalloonTip(1);
        }

        private void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += GlobalHookKeyDown;
        }

        private void GlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F12) // For Ctrl + F12
            {
                var s = Settings.Default;
                if (s.Clipboard | s.eticket)
                {
                    Main_Function();
                }
                else
                {
                    BalloonNotification("No Case # Behavior Setting Saved", "Settings");
                }

            }
        }

        private void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= GlobalHookKeyDown;
            m_GlobalHook.Dispose();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Settings.Default.Clipboard = this.checkBoxClipBoard.Checked;
            Settings.Default.eticket = this.checkBox_eTicket.Checked;
            try
            {
                Settings.Default.Save();
                BalloonNotification("Settings Saved Successfully", "Settings:");
            }
            catch (Exception)
            {
                BalloonNotification("Settings FAILED to save.");
            }
        }
    }
}