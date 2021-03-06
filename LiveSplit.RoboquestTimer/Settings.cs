using LiveSplit.ComponentUtil;
using LiveSplit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RoboquestTimer
{
    partial class Settings : UserControl
    {
        public string ProcessName { get; set; }
        public string RQVersion { get; set; }
        public bool ResetDeath { get; set; }
        public bool ResetGame { get; set; }

        public Settings()
        {
            InitializeComponent();

            ProcessName = "RoboQuest-Win64-Shipping";
            RQVersion = "Steam";
            ResetDeath = false;
            ResetGame = true;

            cmbRQVersion.DataBindings.Add("SelectedItem", this, "RQVersion", false, DataSourceUpdateMode.OnPropertyChanged);
            cbResetDeath.DataBindings.Add("Checked", this, "ResetDeath", false, DataSourceUpdateMode.OnPropertyChanged);
            cbResetGame.DataBindings.Add("Checked", this, "ResetGame", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void SetSettings(System.Xml.XmlNode node)
        {
            System.Xml.XmlElement element = (System.Xml.XmlElement)node;

            ProcessName = SettingsHelper.ParseString(element["ProcessName"]);
            RQVersion = SettingsHelper.ParseString(element["RQVersion"]);
            ResetDeath = SettingsHelper.ParseBool(element["ResetDeath"]);
            ResetGame = SettingsHelper.ParseBool(element["ResetGame"]);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            System.Xml.XmlElement parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        public static int CreateSetting(XmlDocument document, XmlElement parent, string name, IEnumerable<Color> colors)
        {
            if (document != null)
            {
                var element = document.CreateElement(name);
                element.InnerText = String.Join(",", colors.Select(c => c.ToArgb().ToString("X8")));
                parent.AppendChild(element);
            }
            return colors.GetHashCode();
        }

        private int CreateSettingsNode(System.Xml.XmlDocument document, System.Xml.XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) ^
            SettingsHelper.CreateSetting(document, parent, "ProcessName", ProcessName) ^
            SettingsHelper.CreateSetting(document, parent, "RQVersion", RQVersion) ^
            SettingsHelper.CreateSetting(document, parent, "ResetDeath", ResetDeath) ^
            SettingsHelper.CreateSetting(document, parent, "ResetGame", ResetGame);
        }

        private void CmbRQVersion_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((string)cmbRQVersion.SelectedItem == "Steam")
            {
                RQVersion = "Steam";
                ProcessName = "RoboQuest-Win64-Shipping";
            }
            else
            {
                RQVersion = null;
                ProcessName = null;
            }
        }

        private void GrpRoboquest_Enter(object sender, EventArgs e)
        {

        }

        private void CBResetDeath_CheckedChanged(object sender, EventArgs e)
        {
            if (cbResetDeath.Checked == true)
            {
                ResetDeath = true;
            }
            else
            {
                ResetDeath = false;
            }
        }

        private void CBResetGame_CheckedChanged(object sender, EventArgs e)
        {
            if (cbResetGame.Checked == true)
            {
                ResetGame = true;
            }
            else
            {
                ResetGame = false;
            }
        }
    }
}
