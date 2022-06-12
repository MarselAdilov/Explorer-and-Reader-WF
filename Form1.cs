using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private static string command; // хранит прописанную с командной строки команду

        private string FileName { get; set; } = string.Empty;
        public static string encodingPage = string.Empty;

        public Form1()
        {
            InitializeComponent(); 
            Program.path[0] = @"C:"; //начальный каталог
            Program.Intro(textBoxOut);
        }

        private void buttonRun_Click(object sender, EventArgs e) // Выполнить
        {
            textBoxOut.ReadOnly = true;
            command = textBoxIn.Text;
            textBoxOut.Clear();
            textBoxIn.Clear();
            encodingPage = (string)code.Items[code.SelectedIndex];
            Program.Request(command, textBoxOut);
        }

        private void textBoxIn_KeyPress(object sender, KeyPressEventArgs e) // обработка нажатия клавиши Enter для исполнения прописанной команды
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                e.Handled = true; // убираем системный звук при нажатии Enter
                buttonRun_Click(sender, e);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e) // Назад
        {
            textBoxOut.ReadOnly = true;
            command = "0";
            textBoxOut.Clear();
            textBoxIn.Clear();
            Program.Request(command, textBoxOut);
        }

        private void buttonTop_Click(object sender, EventArgs e) // Корневой каталог
        {
            textBoxOut.ReadOnly = true;
            command = "-top";
            textBoxOut.Clear();
            textBoxIn.Clear();
            Program.Request(command, textBoxOut);
        }

        private void buttonCur_Click(object sender, EventArgs e) // Текущий каталог
        {
            textBoxOut.ReadOnly = true;
            command = "-cur";
            textBoxOut.Clear();
            textBoxIn.Clear();
            Program.Request(command, textBoxOut);
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxOut.ReadOnly = true;
            command = "-info";
            textBoxOut.Clear();
            textBoxIn.Clear();
            Program.Request(command, textBoxOut);
        }

        private void newFile(object sender, EventArgs e)
        {
            textBoxOut.Text = String.Empty;
            textBoxOut.ReadOnly = false;
        }

        private void openFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|" + "Все файлы (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxOut.ReadOnly = false;
                FileName = openFileDialog.FileName;
                encodingPage = (string)code.Items[code.SelectedIndex];
                StreamReader fStr = new StreamReader(FileName, Encoding.GetEncoding(encodingPage));
                textBoxOut.Text = fStr.ReadToEnd();
                fStr.Close();
            }
        }

        private void saveFile(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog(); 
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|" + "Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
            }
            encodingPage = (string)code.Items[code.SelectedIndex];
            StreamWriter fStr = new StreamWriter(FileName, false, Encoding.GetEncoding(encodingPage));
            fStr.Write(textBoxOut.Text);
            fStr.Close();
        }

        private void exit(object sender, EventArgs e)
        {
            textBoxOut.ReadOnly = true;
            this.Close();
        }

        private void openFileList(object sender, EventArgs e)
        {
            textBoxOut.ReadOnly = true;
            textBoxOut.Text = String.Empty;
            Program.FileList(Program.PathLinker(Program.pathIndex), textBoxOut);
        }
    }
}
