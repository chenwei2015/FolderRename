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

namespace ProjectRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> filetypes = new List<string>();

        
        void searchTypes(string path)
        {
            filetypes = new List<string>();
            checkedListBox1.Items.Clear();
            DirectoryInfo Dir = new DirectoryInfo(path);
            {
                foreach (FileInfo f in Dir.GetFiles("*", SearchOption.AllDirectories)) //查找文件
                {
                  if (!filetypes.Contains(f.Extension))
                  {
                      filetypes.Add(f.Extension);
                      checkedListBox1.Items.Add(f.Extension);
                      checkedListBox1.SetItemChecked(checkedListBox1.Items.Count-1,true);
                      if (f.Extension.ToLower().Contains("xls")|| f.Extension.ToLower().Contains("pdf"))
                      {
                          checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, false);
                      }
                  }
                }
              
            }
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                operatorfolder.Text = folderBrowserDialog1.SelectedPath;
                searchTypes(operatorfolder.Text);
            }
        }
        bool Rename(string name,string repls,string tor)
        {
            try
            {
                string name_org = name;
                name = name.Replace(repls, tor);
                FileInfo fi = new FileInfo(name_org);
                fi.MoveTo(name);
                replaced = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return true;
        }

        bool RenameContent(string name, string repls, string tor)
        {
          //  try
            {
                string str = File.ReadAllText(name);
                str = str.Replace(repls, tor);
                File.WriteAllText(name, str);
                replaced = true;
             }
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    return false;
            //}
            return true;
        }

        bool replaced = false;
        bool Replace(string path)
        {
            replaced = false;
            DirectoryInfo Dir = new DirectoryInfo(path);          
            {

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        foreach (FileInfo f in Dir.GetFiles("*" + filetypes[i], SearchOption.AllDirectories)) //查找文件
                        {
                            if (renamect.Checked)
                            {
                                if (!RenameContent(f.FullName, source.Text, dest.Text))
                                {
                                    return false;
                                }
                            }
                            if(RenameFile.Checked)
                            {
                                if (f.FullName.EndsWith(filetypes[i]))
                                {
                                    if(!Rename(f.FullName, source.Text, dest.Text) )
                                    {
                                        return false;
                                    }
                                    
                                }
                            }
                            
                           
                        }
                    }
                }
             
               
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(operatorfolder.Text))
            {
                MessageBox.Show(string.Format("目录不存在:{0}，请确认！",operatorfolder.Text));
                return;
            }
            if(Replace(operatorfolder.Text))
                MessageBox.Show("替换成功");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox3.Checked);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }
    }
}
