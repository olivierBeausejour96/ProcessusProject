using System;
using System.Windows;
using System.Windows.Forms;
using Hotkeys;
using VersionOfficielle.Helpers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace VersionOfficielle
{
    public partial class frmMain : Form
    {
        private GlobalHotkey FFStartHotKey;
        private GlobalHotkey FFStopHotKey;
        private CGGPokerSoftwareAction FFGGPoker;
        private CBot FFBot;

        public frmMain()
        {
            InitializeComponent();

            FFStartHotKey = new GlobalHotkey(GlobalHotkey.NOMOD, Keys.F5, this);
            FFStopHotKey = new GlobalHotkey(GlobalHotkey.NOMOD, Keys.F6, this);

            FFStartHotKey.Register();
            FFGGPoker = null;            
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartBot();           
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopBot();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == GlobalHotkey.WM_HOTKEY_MSG_ID)                
                HandleHotkey();

            base.WndProc(ref m);
        }

        private void HandleHotkey()
        {
            // Since we only registered 2 hotkeys (which are start and stop the bot), 
            // we know implicitely that it's either we start the bot or we stop the bot 
            // depending if the stop button is enabled or not.
            if (btnStop.Enabled)
                StopBot();
            else
                StartBot();
        }

        private void StartBot()
        {
            FFStartHotKey.UnRegister();
            btnStart.Enabled = false;

            btnStop.Enabled = true;
            FFStopHotKey.Register();

            FFBot = new CBot();            
            FFGGPoker = new CGGPokerSoftwareAction(FFBot.getDecision, this);
            FFGGPoker.StartWatching();
        }

        private void StopBot()
        {
            FFStopHotKey.UnRegister();
            btnStop.Enabled = false;            
            
            btnStart.Enabled = true;
            FFStartHotKey.Register();

            FFGGPoker.StopWatching();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CGGPokerBoardReader dsa = new CGGPokerBoardReader(FFGGPoker.FFWindowsDictionary[0]);
            CGGPokerTableInformations dsad = new CGGPokerTableInformations(FFGGPoker.FFWindowsDictionary[0]);
            CGGPokerHandReader qwe = new CGGPokerHandReader(FFGGPoker.FFWindowsDictionary[0]);
            dsa.ActualizeCurrentImage();
            qwe.ActualizeHand();

            MessageBox.Show(qwe.valueStatement1());
            MessageBox.Show(qwe.valueStatement2());
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopBot();
        }
    }
}
