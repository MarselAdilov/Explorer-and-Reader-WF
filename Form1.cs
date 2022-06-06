using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private static string[] path = new string[1000]; // хранит путь поэлементно
        private static string[] pathBackup = new string[1000]; // копия path на случай ошибки, дающая возможность отката
        private static int pathIndex = 0; // хранит кол-во объектов в пути
        private static int pathIndexBackup = 0; // копия pathIndex на случай ошибки, дающая возможность отката
        private static string[] dirList; // хранит список файлов в текущем каталоге
        private static int dirIndex = 0; // кол-во файлов в текущем каталоге
        private static string command; // хранит прописанную с командной строки команду
        private static bool status = true; // пока true, цикл обработки запросов работает
        public Form1()
        {
            InitializeComponent();
        }

        private void Request(string command) //обрабатывает введенную команду
        {
            
            textBoxOut.Text = command;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            command = textBoxIn.Text;
            textBoxOut.Clear();
            textBoxIn.Clear();
            Request(command); 
        }
    }
}
