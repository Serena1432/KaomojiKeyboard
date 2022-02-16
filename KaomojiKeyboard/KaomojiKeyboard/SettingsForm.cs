using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace KaomojiKeyboard
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        Dictionary<string, object> Settings = new Dictionary<string, object>();
        Dictionary<string, Dictionary<string, string>> Hotkeys = new Dictionary<string, Dictionary<string, string>>();
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Creating/reading the settings data
            if (!File.Exists(".\\settings.json"))
            {
                Settings.Add("emoji_data_path", ".\\emoji_data.json");
                
                Settings.Add("show_window_when_start", true);
                File.WriteAllText(".\\settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
            }
            else
            {
                string SettingsText = File.ReadAllText(".\\settings.json");
                try
                {
                    Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(SettingsText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while reading the settings data file! Please check the Settings data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Creating/reading the hotkeys data
            if (!File.Exists(".\\hotkeys.json"))
            {
                Hotkeys.Add("show_keyboard", new Dictionary<string, string>(){
                    {"key", "1.OemPeriod"}
                });
                File.WriteAllText(".\\hotkeys.json", JsonConvert.SerializeObject(Hotkeys, Formatting.Indented));
            }
            else
            {
                string HotkeysText = File.ReadAllText(".\\hotkeys.json");
                try
                {
                    Hotkeys = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(HotkeysText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while reading the hotkeys data file! Please check the Settings data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (rkApp.GetValue("KaomojiKeyboard") != null) checkBox1.Checked = true;
            if ((bool)Settings["show_window_when_start"] == true) checkBox2.Checked = true;
            textBox1.Text = Settings["emoji_data_path"].ToString();
            var ShowKey = Hotkeys["show_keyboard"];
            string[] ShowKeyData = ShowKey["key"].Split('.');
            var KeyVocab = new Dictionary<string, string>() { { "1", "Alt" }, { "2", "Control" }, { "4", "Shift" }, { "8", "Windows" } };
            comboBox1.Text = KeyVocab[ShowKeyData[0]];
            textBox2.Text = ShowKeyData[1]
                    .Replace("OemPeriod", ".")
                    .Replace("Oemcomma", ",")
                    .Replace("OemBackslash", "\\")
                    .Replace("OemCloseBrackets", "]")
                    .Replace("OemMinus", "-")
                    .Replace("OemOpenBrackets", "[")
                    .Replace("OemSemicolon", ";")
                    .Replace("OemQuotes", "'");
            HotkeysLoad();
        }

        void HotkeysLoad()
        {
            int x = 0;
            foreach (var HotkeyData in Hotkeys)
            {
                if (HotkeyData.Key.Contains("."))
                {
                    string[] KeyData = HotkeyData.Key.Split('.');
                    Panel panel = new Panel();
                    panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    panel.Location = new Point(5, 5 + x * 45);
                    panel.Size = new Size(346, 40);
                    var label = new Label();
                    label.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
                    label.Location = new System.Drawing.Point(0, 0);
                    label.Size = new System.Drawing.Size(167, 38);
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    label.TabIndex = 0;
                    label.Text = HotkeyData.Value["emoji"].ToString();
                    var HotKeyLabel = new Label();
                    HotKeyLabel.Location = new System.Drawing.Point(167, 0);
                    HotKeyLabel.Size = new System.Drawing.Size(96, 38);
                    HotKeyLabel.TabIndex = 1;
                    HotKeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    var KeyVocab = new Dictionary<string, string>() { { "1", "Alt" }, { "2", "Control" }, { "4", "Shift" }, { "8", "Windows" } };
                    HotKeyLabel.Text = KeyVocab[KeyData[0]] + " + " + KeyData[1]
                    .Replace("OemPeriod", ".")
                    .Replace("Oemcomma", ",")
                    .Replace("OemBackslash", "\\")
                    .Replace("OemCloseBrackets", "]")
                    .Replace("OemMinus", "-")
                    .Replace("OemOpenBrackets", "[")
                    .Replace("OemSemicolon", ";")
                    .Replace("OemQuotes", "'");
                    var RemoveButton = new Button();
                    RemoveButton.Location = new System.Drawing.Point(261, 0);
                    RemoveButton.Name = HotkeyData.Key;
                    RemoveButton.Size = new System.Drawing.Size(83, 38);
                    RemoveButton.TabIndex = 20;
                    RemoveButton.Text = "Remove";
                    RemoveButton.UseVisualStyleBackColor = true;
                    RemoveButton.Click += new EventHandler(Remove);
                    panel.Controls.Add(label);
                    panel.Controls.Add(HotKeyLabel);
                    panel.Controls.Add(RemoveButton);
                    panel1.Controls.Add(panel);
                    x++;
                }
            }
        }

        void Remove(object sender, EventArgs e)
        {
            Hotkeys.Remove(((Button)sender).Name);
            panel1.Controls.Clear();
            HotkeysLoad();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) this.DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked) rkApp.SetValue("KaomojiKeyboard", Application.ExecutablePath);
            else rkApp.DeleteValue("KaomojiKeyboard", false);
            if (checkBox2.Checked) Settings["show_window_when_start"] = true;
            else Settings["show_window_when_start"] = false;
            if (String.IsNullOrEmpty(textBox1.Text)) MessageBox.Show("You need to type the Emoji Database Path first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!File.Exists(textBox1.Text)) MessageBox.Show("The typed Emoji Database Path doesn't exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Settings["emoji_data_path"] = textBox1.Text;
                var KeyVocab = new Dictionary<string, string>() { { "Alt", "1" }, { "Control", "2" }, { "Shift", "4" }, { "Windows", "8" } };
                Hotkeys["show_keyboard"]["key"] = KeyVocab[comboBox1.Text] + "." + textBox2.Text
                        .Replace(".", "OemPeriod")
                        .Replace(",", "Oemcomma")
                        .Replace("\\", "OemBackslash")
                        .Replace("]", "OemCloseBrackets")
                        .Replace("-", "OemMinus")
                        .Replace("[", "OemOpenBrackets")
                        .Replace(";", "OemSemicolon")
                        .Replace("'", "OemQuotes");
                File.WriteAllText(".\\settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
                File.WriteAllText(".\\hotkeys.json", JsonConvert.SerializeObject(Hotkeys, Formatting.Indented));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox4.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(comboBox2.Text)) MessageBox.Show("You need to type enough information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var KeyData = new Dictionary<string, string>()
                {
                    {"type", "emoji"},
                    {"emoji", textBox4.Text}
                };
                var KeyVocab = new Dictionary<string, string>() { { "Alt", "1" }, { "Control", "2" }, { "Shift", "4" }, { "Windows", "8" } };
                Hotkeys.Add(KeyVocab[comboBox2.Text] + "." + textBox3.Text
                    .Replace(".", "OemPeriod")
                    .Replace(",", "Oemcomma")
                    .Replace("\\", "OemBackslash")
                    .Replace("]", "OemCloseBrackets")
                    .Replace("-", "OemMinus")
                    .Replace("[", "OemOpenBrackets")
                    .Replace(";", "OemSemicolon")
                    .Replace("'", "OemQuotes"), KeyData);
                comboBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                HotkeysLoad();
            }
        }
    }
}
