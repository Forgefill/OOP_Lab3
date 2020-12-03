using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Net;

namespace OOP_Lab3
{
    public partial class TextEditorForm : Form
    {
        string FilePath;

        public TextEditorForm(string FilePath)
        {
            InitializeComponent();
            textBox1.TextChanged += textchanged;
            textBox2.TextChanged += textchanged;
            FileClear.Click += ClearClick;
            Save.Click += SaveClick;
            Help.Click += HelpClick;
            ChangeButton.Enabled = false;
            this.FilePath = FilePath;
            richTextBox1.LoadFile(this.FilePath, RichTextBoxStreamType.PlainText);
            if (System.IO.Path.GetExtension(this.FilePath) != ".html") MoveToNewHTML.Enabled = false;
        }

        private void SaveClick(object sender, EventArgs e)
        {
            richTextBox1.SaveFile(FilePath, RichTextBoxStreamType.PlainText);
        }

        private void textchanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "") ChangeButton.Enabled = true;
            else
            {
                ChangeButton.Enabled = false;
            }
        }

        private void HelpClick(object sender, EventArgs e)
        {
            string help = "\tДовідка про використання текстового редактора" +
                "\n1) Ви можете редагувати текст у вікні." +
                "\n2) У пункті \"файл\" ви можете зберігти та очистити вікно " +
                "\n3) Наявний функціонал заміни одного підрядка на інший за допомогою кнопки заміна" +
                "\n4) Перетворення тексту до формату, у якому перші і останні букви речення будуть великими, а інші малими" +
                "\n5) Перенести частину html до нового файлу html";
            DialogResult dr = MessageBox.Show(help,
                      "Довідка", MessageBoxButtons.OK);
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Replace(textBox1.Text, textBox2.Text);
        }

        private void ClearClick(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void ChangeClick(object sender, EventArgs e)
        {
            RegisterChanger ch = new RegisterChanger(richTextBox1.Text);
            richTextBox1.Text = ch.GetChangedText();
        }

        private void MoveClick(object sender, EventArgs e)
        {
            MoveToNewHtml(richTextBox1.SelectedText);
        }
        private void MoveToNewHtml(string crrText)
        {
            List<string> BadTegs = new List<string>() { "<head>", "<html>", "<body>", "<!DOCTYPE html>", "</head>", "/body", "/html" };
            if (crrText != "")
            {
                foreach (string i in BadTegs)
                {
                    if (crrText.Contains(i))
                    {
                        DialogResult dr = MessageBox.Show("Виділений текст не може містити наступні теги <html>,<body>,<head>,<!DOCTYPE html>", "Помилка");
                        return;
                    }
                }
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFileDialog1.FileName;
                    HTMLMover.WriteNewHTML(path, crrText);
                    foreach (string way in HTMLMover.GetImgPath(crrText))
                    {
                        if (!way.Contains(':'))
                        {

                            string first = Path.GetDirectoryName(FilePath) + '\\' + way;
                            string second = Path.GetDirectoryName(path) + '\\' + way;
                            System.IO.File.Move(first, second);
                        }
                    }
                    string text = richTextBox1.Text;
                    string selectedText = richTextBox1.SelectedText;
                    richTextBox1.Text = text.Remove(text.IndexOf(selectedText), selectedText.Length);
                    richTextBox1.SaveFile(FilePath, RichTextBoxStreamType.PlainText);
                }
            }
        }

        private void TextEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Зберегти файл перед виходом?",
                     "Важливо!", MessageBoxButtons.YesNoCancel);

            if (dr == DialogResult.Yes)
            {
                e.Cancel = false;
                SaveClick(sender, e);
                return;
            }
            else if (dr == DialogResult.No)
            {
                e.Cancel = false;
            }
            else if(dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
