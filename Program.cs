using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        private static string[] path = new string[1000]; // хранит путь поэлементно
        private static string[] pathBackup = new string[1000]; // копия path на случай ошибки, дающая возможность отката
        private static int pathIndex = 0; // хранит кол-во объектов в пути
        private static int pathIndexBackup = 0; // копия pathIndex на случай ошибки, дающая возможность отката
        private static string[] dirList; // хранит список файлов в текущем каталоге
        private static int dirIndex = 0; // кол-во файлов в текущем каталоге
        private static string command; // хранит прописанную с командной строки команду
        private static bool status = true; // пока true, цикл обработки запросов работает

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
            //Pause();
            //FileList(PathLinker(pathIndex));

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

            path[0] = @"C:"; //начальный каталог
        }
    }
}
