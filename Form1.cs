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


        public Form1()
        {
            InitializeComponent();
            Program.path[0] = @"C:"; //начальный каталог
            Program.Intro(textBoxOut);
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            command = textBoxIn.Text;
            textBoxOut.Clear();
            textBoxIn.Clear();
            Program.Request(command, textBoxOut);
        }

        private void textBoxIn_KeyPress(object sender, KeyPressEventArgs e) // обработка нажатия клавиши Enter для исполнения прописанной команды
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                buttonRun_Click(sender, e);
            }
        }
    }
}
