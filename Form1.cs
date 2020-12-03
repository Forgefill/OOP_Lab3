using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace OOP_Lab3
{
    public partial class Form1 : Form
    {
        FileManager fileManager = new FileManager();
        ReportManager reportManager;
        public List<string> words = new List<string>();
        public Form1()
        {
            InitializeComponent();
            DialogResult dr = MessageBox.Show("Зберігати звіт про роботу в txt-файлі?\n(інакше буде використано html файл)",
                      "Важливо!", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                reportManager = new ReportManager(new TXTReportBuilder());
            }
            else
            {
                reportManager = new ReportManager(new HTMLReportBuilder());
            }
            Help.Click += HelpClick;
            GetDrives();
        }

        private void Back(string crrPath)
        {
            if (crrPath == "") return;
            else if (crrPath.Last() == '\\')
            {
                crrPath = crrPath.Remove(crrPath.Length - 1, 1);
            }

            try
            {
                while (crrPath.Last() != '\\')
                {
                    crrPath = crrPath.Remove(crrPath.Length - 1);
                }
            }
            catch(Exception)
            {
                crrPath = "";
            }   
            Path.Text = crrPath;
            if (crrPath != "")
            {
                GetFolders(Path.Text);
                GetFiles(Path.Text, comboBoxFileType.Text);
            }
            else
            {
                GetDrives();
                listViewFile.Items.Clear();
            }
        }
        private void GetDrives()
        {
            listViewPath.Items.Clear();
            foreach (var i in fileManager.GetDrives())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = i;
                lvi.ImageIndex = 2;
                listViewPath.Items.Add(lvi);
            }
        }
        private void GetFiles(string path, string filetype)
        {
            List<string> files = fileManager.GetFiles(path, filetype);
            listViewFile.Items.Clear();
            foreach (var file in files)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = file;
                lvi.ImageIndex = 1;
                listViewFile.Items.Add(lvi);
            }
        }
        private void GetFolders(string path)
        {
            List<string> folders = fileManager.GetFolders(path);
            listViewPath.Items.Clear();
            foreach (var folder in folders)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = folder;
                lvi.ImageIndex = 0;
                listViewPath.Items.Add(lvi);
            }
        }
        private void FileTypeTextChanged(object sender, EventArgs e)
        {
            GetFiles(Path.Text, comboBoxFileType.Text);
        }
        private void listViewPath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Path.Text = System.IO.Path.Combine(Path.Text, listViewPath.SelectedItems[0].Text);
            GetFolders(Path.Text);
            GetFiles(Path.Text, comboBoxFileType.Text);
            reportManager.AddOperation("Перехід", "Відкриття шляху " + Path.Text);
        }
        private void BackClick(object sender, EventArgs e)
        {
            Back(Path.Text);
            reportManager.AddOperation("Перехід", "Відкриття шляху  " + Path.Text);
        }
        private void GoToClick(object sender, EventArgs e)
        {
            try
            {
                GetFolders(textBox1.Text);
            }
            catch(Exception)
            {
                reportManager.AddOperation("Помилка", "Шлях вказано невірно  " + textBox1.Text);
                DialogResult dr = MessageBox.Show("Шлях вказано невірно",
                      "Важливо!", MessageBoxButtons.OK);
                return;
            }
            Path.Text = textBox1.Text;
            GetFiles(Path.Text, comboBoxFileType.Text);
            textBox1.AutoCompleteCustomSource.Add(Path.Text);
            reportManager.AddOperation("Перехід", "Відкриття шляху  " + Path.Text);
        }

        private void listViewFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (listViewFile.SelectedItems.Count != 0)
                {
                    reportManager.AddOperation("Зміни", "Зміни в файлі  " + listViewFile.SelectedItems[0].Text);
                    TextEditorForm form = new TextEditorForm(Path.Text + '\\' + listViewFile.SelectedItems[0].Text);
                    form.ShowDialog();
                    GetFiles(Path.Text, comboBoxFileType.Text);
                }
            }
            else if(e.KeyCode == Keys.F4)
            {
                reportManager.AddOperation("Зміни", "Пошук html файлів за словами");
                Form2 f = new Form2(this);
                f.ShowDialog();
                HTMLFinder h = new HTMLFinder(words);
                listViewFile.Items.Clear();
                foreach (var file in h.GetHTML(Path.Text))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = file;
                    lvi.ImageIndex = 1;
                    listViewFile.Items.Add(lvi);
                }
            }
            else if(e.KeyCode == Keys.F5)
            {
                reportManager.AddOperation("Зміни", "Видалення файлу " + listViewFile.SelectedItems[0].Text);
                File.Delete(Path.Text + "\\" + listViewFile.SelectedItems[0].Text);
                listViewFile.Items.Remove(listViewFile.SelectedItems[0]);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            reportManager.saveReport();
        }

        private void ReportSaveClick(object sender, EventArgs e)
        {
            reportManager.saveReport();
        }
        private void HelpClick(object sender, EventArgs e)
        {
            string help = "\tДовідка про використання текстового редактора" +
                "\n1) Ви можете відкривати папки подвійним натисканням або використовуючи перехід під вікном." +
                "\n2) Ви можете обрати тип файлів який буде відображення у правому вікні" +
                "\n3) Використовуйте натискання кнопок F3-F5 з обраним файлом" +
                "\n4) Програма утворює звіт роботи, який буде збережено при натисканні кнопки або при виході" +
                "\n5) F3 - Відкриття текстового редактору для відповідного файлу, рекомендую лише для html, txt" +
                "\n6) F4 - Пошук файлу за словами, які буде потрібно вказати у відповідному полі." +
                "\n7) F5 - Видалить обраний файл";
            DialogResult dr = MessageBox.Show(help,
                      "Довідка", MessageBoxButtons.OK);
        }
    }
}
