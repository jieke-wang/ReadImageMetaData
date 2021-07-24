using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Shell32;
using System.Text;

namespace ReadFileMetaData
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = ofdFile.FileName;
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                txtResult.Text = string.Empty;
                return;
            }

            string filePath = txtFilePath.Text;
            Shell shell = new Shell();
            Folder folder = shell.NameSpace(Path.GetDirectoryName(filePath));
            FolderItem item = folder.ParseName(Path.GetFileName(filePath));

            Dictionary<string, string> Properties = new Dictionary<string, string>();
            int i = 0;
            while (true)
            {
                string key = folder.GetDetailsOf(null, i);
                if (string.IsNullOrEmpty(key))
                {
                    break;
                }
                string value = folder.GetDetailsOf(item, i);
                i++;
                if (string.IsNullOrEmpty(value))
                    continue;
                Properties.Add(key, value);
            }

            Properties = Properties.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);

            StringBuilder result = new StringBuilder();
            foreach (var Property in Properties)
            {
                result.AppendFormat("{0}: {1}{2}", Property.Key, Property.Value, Environment.NewLine);
            }
            txtResult.Text = result.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }
    }
}
