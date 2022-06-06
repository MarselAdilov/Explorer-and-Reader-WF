using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    internal class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        public static string[] path = new string[1000]; // хранит путь поэлементно
        private static string[] pathBackup = new string[1000]; // копия path на случай ошибки, дающая возможность отката
        private static int pathIndex = 0; // хранит кол-во объектов в пути
        private static int pathIndexBackup = 0; // копия pathIndex на случай ошибки, дающая возможность отката
        private static string[] dirList; // хранит список файлов в текущем каталоге
        private static int dirIndex = 0; // кол-во файлов в текущем каталоге
        private static string command; // хранит прописанную с командной строки команду
        private static bool status = true; // пока true, цикл обработки запросов работает
        public static bool pause = false; //индикатор паузы

        [STAThread]

        public static void Intro(TextBox textBoxOut) //вступительное сообщение
        {

            textBoxOut.Text = ("+-----------------------------------------------------------------------------------------------------------------+\r\n");
            textBoxOut.Text += ("|\t\t\tПроводник\t\t\t\t|\r\n");
            textBoxOut.Text += ("+-------------------------------------------------------+--------------------------------------------------------+\r\n");
            textBoxOut.Text += ("|\tКоманда\t\t\t|\t\tКод\t\t|\r\n");
            textBoxOut.Text += ("+-------------------------------------------------------+--------------------------------------------------------+\r\n");
            textBoxOut.Text += ("| · Открыть файл из списка\t\t| <номер файла>\t\t\t|\r\n");
            textBoxOut.Text += ("| · Открыть путь\t\t\t| -open [path]\t\t\t|\r\n");
            textBoxOut.Text += ("| · Открыть файл с кодировкой\t| -open [path / number] [page_code]\t|\r\n");
            textBoxOut.Text += ("| · Выйти из программы\t\t| -exit\t\t\t\t|\r\n");
            textBoxOut.Text += ("| · Перейти к текущему каталогу\t| -cur\t\t\t\t|\r\n");
            textBoxOut.Text += ("| · Перейти к корневой папке\t| -top\t\t\t\t|\r\n");
            textBoxOut.Text += ("+-------------------------------------------------------+--------------------------------------------------------+\r\n");
            FileList(PathLinker(pathIndex), textBoxOut);
        }

        private static void FileList(string path, TextBox textBoxOut) //выводит список файлов в директории
        {
            dirIndex = 0;
            textBoxOut.Text += ($"\r\n>>>\t{path}\t<<<\r\n");
            DirectoryInfo dInfo = new DirectoryInfo(path);
            DirectoryInfo[] dir = dInfo.GetDirectories();
            FileInfo[] spisok = dInfo.GetFiles();
            
            dirList = new string[1000];
            textBoxOut.Text += ($"\r\n№\t{"Имя файла",-50}\tРазмер (байт)");
            if (path != @"C:\") textBoxOut.Text += ($"\r\n0.\tНазад\r\n");

            foreach (var d in dir)
            {
                dirList[dirIndex++] = d.Name;
                textBoxOut.Text += ($"\r\n{dirIndex}.\t[{d.Name.ToUpper() + "]",-50}");
            }

            foreach (var f in spisok)
            {
                dirList[dirIndex++] = f.Name;
                textBoxOut.Text += ($"\r\n{dirIndex}.\t{f.Name,-50}\t{f.Length:#,#}");
            }
            textBoxOut.Text += ("\r\n<Введите номер или команду>");
        }

        private static string PathLinker(int i) //собирает путь воедино
        {
            string pathLinked = "";
            for (int j = 0; j <= i; j++) pathLinked = pathLinked + path[j] + @"\";
            return pathLinked;
        }

        public static void Request(string command, TextBox textBoxOut) //обрабатывает введенную команду
        {
            textBoxOut.Text = command;
        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
