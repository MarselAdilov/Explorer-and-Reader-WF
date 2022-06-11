using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
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
            textBoxOut.Text += ("| · Перейти к текущему каталогу\t| -cur\t\t\t\t|\r\n");
            textBoxOut.Text += ("| · Перейти к корневой папке\t| -top\t\t\t\t|\r\n");
            textBoxOut.Text += ("| · Справка\t\t\t| -info\t\t\t\t|\r\n");
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
            if (path != @"C:\" && path != @"c:\" && path != @"C:" && path != @"c:") textBoxOut.Text += ($"\r\n0.\tНазад\r\n");

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
            textBoxOut.ReadOnly = true;
            try
            {
                char symbol = command[0];

                if (symbol == '-') //обрабока команд
                {
                    if (command.StartsWith("-open"))
                    {
                        string[] tmp = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 2) //если задана кодировка
                        {
                            try //если задан файл числом
                            {
                                Form1.encodingPage = tmp[2];
                                Open(textBoxOut, Convert.ToInt32(tmp[1]) - 1, null, Form1.encodingPage);
                            }
                            catch //если задан путь
                            {
                                Form1.encodingPage = tmp[2];
                                Open(textBoxOut , - 1, tmp[1], Form1.encodingPage);
                            }
                            return;
                        }
                        else //если кодировка не задана
                        {
                            Open(textBoxOut, - 1, tmp[1], Form1.encodingPage);
                            return;
                        }
                    }
                    else if (command.StartsWith("-top"))
                    {
                        Open(textBoxOut, -1);
                        return;
                    }
                    else if (command.StartsWith("-cur"))
                    {
                        Open(textBoxOut, -1, Directory.GetCurrentDirectory());
                        return;
                    }
                    else if (command.StartsWith("-info"))
                    {
                        Intro(textBoxOut);
                        return;
                    }
                    else
                    {
                        textBoxOut.Text += ("\r\nТакой команды не существует. Попробуйте снова.");
                        FileList(PathLinker(pathIndex), textBoxOut);
                        return;
                    }
                }
                else //обработка числа (выбор файла)
                {
                    try
                    {
                        int num = Convert.ToInt32(command);
                        if (num <= dirIndex && num > 0)
                        {
                            Open(textBoxOut, num - 1, null, Form1.encodingPage);
                            return;
                        }
                        else if (num == 0 && PathLinker(pathIndex) != @"C:\" && PathLinker(pathIndex) != @"c:\" && PathLinker(pathIndex) != @"C:" && PathLinker(pathIndex) != @"c:") // "назад"
                        {
                            pathIndex--;
                            FileList(PathLinker(pathIndex), textBoxOut);
                        }
                        else
                        {
                            textBoxOut.Text += ("\r\nОшибка. Попробуйте снова.");
                            FileList(PathLinker(pathIndex), textBoxOut);
                            return;
                        }

                    }
                    catch
                    {
                        textBoxOut.Text += ("\r\nОшибка. Попробуйте снова.");
                        FileList(PathLinker(pathIndex), textBoxOut);
                        return;
                    }
                }
            }
            catch
            {
                textBoxOut.Text += ("\r\nОшибка. Попробуйте снова.");
                FileList(PathLinker(pathIndex), textBoxOut);
                return;
            }
        }

        private static void Open(TextBox textBoxOut, int num = -1, string p = @"C:", string codePage = "65001") //Открывает файлы и папки
        {
            if (num != -1) //открытие файла по числу
            {
                try
                {
                    path[++pathIndex] = dirList[num];
                    textBoxOut.Text += ($"\r\nОткрытие {PathLinker(pathIndex)}...");

                    // проверяем на поддерживаемый файл для открытия
                    if (path[pathIndex].ToLower().EndsWith(".txt") ||
                        path[pathIndex].ToLower().EndsWith(".log") ||
                        path[pathIndex].ToLower().EndsWith(".md") ||
                        path[pathIndex].ToLower().EndsWith(".cs") ||
                        path[pathIndex].ToLower().EndsWith(".c") ||
                        path[pathIndex].ToLower().EndsWith(".cpp") ||
                        path[pathIndex].ToLower().EndsWith(".html") ||
                        path[pathIndex].ToLower().EndsWith(".xml") ||
                        path[pathIndex].ToLower().EndsWith(".fb2"))
                    {
                        OpenFile(textBoxOut, codePage);
                        FileList(PathLinker(--pathIndex), textBoxOut);
                        return;
                    }
                    else
                    {
                        FileList(PathLinker(pathIndex), textBoxOut);
                    }
                }
                catch
                {
                    textBoxOut.Text += ($"\r\nОшибка: невозможно открыть {PathLinker(pathIndex)}");
                    --pathIndex;
                    FileList(PathLinker(pathIndex), textBoxOut);
                }
            }
            else //открытие файла по пути
            {
                try
                {
                    // сделать резервную копию "path[]" и "pathIndex" на случай ошибки открытия
                    pathBackup = path;
                    pathIndexBackup = pathIndex;
                    // разделить путь "р" на состовляющие и узнать новый "pathIndex"
                    // записать поверх путь из "р" в "path[]"
                    path = new string[1000];
                    int tempIndex = 0;
                    string[] temp = p.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    pathIndex = temp.Length - 1;
                    foreach (string s in temp)
                    {
                        path[tempIndex++] = s;
                    }

                    textBoxOut.Text += ($"\r\nОткрытие {PathLinker(pathIndex)}...");

                    // проверяем на поддерживаемый файл для открытия
                    if (path[pathIndex].ToLower().EndsWith(".txt") ||
                        path[pathIndex].ToLower().EndsWith(".log") ||
                        path[pathIndex].ToLower().EndsWith(".md") ||
                        path[pathIndex].ToLower().EndsWith(".cs") ||
                        path[pathIndex].ToLower().EndsWith(".c") ||
                        path[pathIndex].ToLower().EndsWith(".cpp") ||
                        path[pathIndex].ToLower().EndsWith(".html") ||
                        path[pathIndex].ToLower().EndsWith(".xml") ||
                        path[pathIndex].ToLower().EndsWith(".fb2"))
                    {
                        OpenFile(textBoxOut, codePage);
                        path = pathBackup;
                        pathIndex = pathIndexBackup;
                        FileList(PathLinker(pathIndex), textBoxOut);
                        return;
                    }
                    else
                    {
                        FileList(PathLinker(pathIndex), textBoxOut);
                    }
                }
                catch
                {
                    textBoxOut.Text += ($"\r\nОшибка: невозможно открыть {p}");
                    path = pathBackup;
                    pathIndex = pathIndexBackup;
                    FileList(PathLinker(pathIndex), textBoxOut);
                }
            }
        }

        private static void OpenFile(TextBox textBoxOut, string codePage = "65001") //Выводит текстовые файлы на консоль
        {
            string fName = path[pathIndex];
            string[] fFormat = fName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string filePath = PathLinker(pathIndex).TrimEnd('\\');
            // страница 1251 имеет имя "windows-1251", а страница 866 – "cp866" и, соответственно, 65001 – "UTF-8".
            textBoxOut.Text += ($"\r\n\n>>>\tФайл: {fName}\t|\tКодировка: {codePage}\t<<<\n");
            try
            {
                switch (fFormat.Last())
                {
                    case "xml":
                        XDocument xmlBook = XDocument.Load(filePath);
                        textBoxOut.Text += ($"\r\n{xmlBook.Declaration}");
                        foreach (XNode element in xmlBook.Nodes()) OpenXML(textBoxOut, element);
                        break;

                    case "fb2":
                        XElement doc = XElement.Load(filePath);

                        //Вывод "шапки"
                        string lastName = null, firstName = null,
                              title = null, sequence = null, annotation = null,
                              year = null, id = null;
                        string[] genre = new string[10];
                        int genreIndex = 0;

                        var titleinfo = doc.Elements().First(x =>
                          x.Name.LocalName == "description").Elements().First(
                                x => x.Name.LocalName == "title-info").Elements();
                        foreach (var element in titleinfo)
                        {
                            switch (element.Name.LocalName)
                            {

                                case "genre": genre[genreIndex++] = element.Value; break;
                                case "author"://Извлекаем элементы этого узла
                                    foreach (var el in element.Elements())
                                    {
                                        switch (el.Name.LocalName)
                                        {
                                            case "last-name": lastName = el.Value; break;
                                            case "first-name": firstName = el.Value; break;
                                            default: break;
                                        }
                                    }
                                    break;
                                case "book-title": title = element.Value; break;
                                case "date": year = element.Value; break;
                                //Наименование серии – атрибут элемента sequence
                                case "sequence":
                                    sequence = element.Attribute("name").Value;
                                    break;
                                case "annotation": annotation = element.Value; break;
                                default: break;
                            }
                        }

                        var documentinfo = doc.Elements().First(x =>
                            x.Name.LocalName == "description").Elements().First(
                            x => x.Name.LocalName == "document-info").Elements();
                        foreach (var element in documentinfo)
                        {
                            switch (element.Name.LocalName)
                            {
                                case "id": id = element.Value; break;
                            }
                        }

                        textBoxOut.Text += ($"Тематика: ");
                        for (int g = 0; g < genreIndex; g++)
                            textBoxOut.Text += ($"{genre[g]} ");
                        textBoxOut.Text += ($"\r\n\nАвтор: " +
                            $"{lastName} {firstName}\nНазвание: {title}\n" +
                            $"Серия: {sequence}\nАннотация: {annotation}\nДата: {year}\nid: {id}\n");

                        //Вывод "тушки"
                        int loop = 0; //для вывода документа по частям
                        var body = doc.Elements().First(x =>
                            x.Name.LocalName == "body").Elements();
                        foreach (var item in body)
                        {
                            if (item.Name.LocalName == "section") //section
                                foreach (var el in item.Elements())
                                {
                                    switch (el.Name.LocalName)
                                    {
                                        case "title":
                                            foreach (var el_title in el.Elements())
                                            {
                                                if (el_title.Name.LocalName == "p")
                                                {
                                                    textBoxOut.Text += ($"\r\n\t{el_title.Value.ToUpper()}");
                                                    loop += (Convert.ToString(el_title.Value)).Length;
                                                }
                                            }
                                            break;
                                        case "epigraph":
                                            foreach (var el_epigraph in el.Elements())
                                            {
                                                if (el_epigraph.Name.LocalName == "p")
                                                {
                                                    textBoxOut.Text += ($"\r\n{el_epigraph.Value}");
                                                    loop += (Convert.ToString(el_epigraph.Value)).Length;
                                                }
                                            }
                                            break;
                                        case "cite":
                                            foreach (var el_cite in el.Elements())
                                            {
                                                if (el_cite.Name.LocalName == "p")
                                                {
                                                    textBoxOut.Text += ($"\r\n{el_cite.Value}");
                                                    loop += (Convert.ToString(el_cite.Value)).Length;
                                                }
                                            }
                                            break;
                                        case "p":
                                            textBoxOut.Text += ($"\r\n   {el.Value}");
                                            loop += (Convert.ToString(el.Value)).Length;
                                            break;
                                        case "empty-line":
                                            textBoxOut.Text += ($"\r\n\n");
                                            loop += (Convert.ToString(el.Value)).Length;
                                            break;
                                        case "empty-line/":
                                            textBoxOut.Text += ($"\r\n\n");
                                            loop += (Convert.ToString(el.Value)).Length;
                                            break;
                                        default: break;
                                    }
                                    if (loop > 1000)
                                    {
                                        loop = 0;
                                        //break;
                                    }
                                }

                            if (item.Name.LocalName == "title")
                            {
                                foreach (var el_title in item.Elements())
                                {
                                    if (el_title.Name.LocalName == "p")
                                    {
                                        textBoxOut.Text += ($"\r\n\t{el_title.Value.ToUpper()}");
                                        loop += (Convert.ToString(el_title.Value)).Length;
                                    }
                                }
                            }
                            if (item.Name.LocalName == "epigraph")
                            {
                                foreach (var el_epigraph in item.Elements())
                                {
                                    if (el_epigraph.Name.LocalName == "p")
                                    {
                                        textBoxOut.Text += ($"\r\n{el_epigraph.Value}");
                                        loop += (Convert.ToString(el_epigraph.Value)).Length;
                                    }
                                }
                            }
                            if (item.Name.LocalName == "cite")
                            {
                                foreach (var el_cite in item.Elements())
                                {
                                    if (el_cite.Name.LocalName == "p")
                                    {
                                        textBoxOut.Text += ($"\r\n{el_cite.Value}");
                                        loop += (Convert.ToString(el_cite.Value)).Length;
                                    }
                                }
                            }

                            if (item.Name.LocalName == "p")
                            {
                                textBoxOut.Text += ($"\r\n{item.Value}");
                                loop += (Convert.ToString(item.Value)).Length;
                            }
                            if (item.Name.LocalName == "empty-line")
                            {
                                textBoxOut.Text += ($"\r\n{item.Value}");
                                loop += (Convert.ToString(item.Value)).Length;
                            }
                            if (item.Name.LocalName == "empty-line/")
                            {
                                textBoxOut.Text += ($"\r\n{item.Value}");
                                loop += (Convert.ToString(item.Value)).Length;
                            }

                            if (loop > 1000)
                            {
                                loop = 0;
                                //break;
                            }
                        }
                        break;

                    case "html":
                        XElement dochtml = XElement.Load(filePath);
                        var html = dochtml.Elements();
                        foreach (var item in html)
                        {
                            if (item.Name.LocalName == "head")
                            {
                                textBoxOut.Text += ($"Заголовок: ");
                                foreach (var el_title in item.Elements())
                                {
                                    if (el_title.Name.LocalName == "title")
                                        textBoxOut.Text += ($"\r\n{el_title.Value}");
                                }
                            }
                            textBoxOut.Text += ($"\r\n\n\n");

                            if (item.Name.LocalName == "body")
                            {
                                foreach (var el in item.Elements())
                                {
                                    switch (el.Name.LocalName)
                                    {
                                        case "section":
                                            foreach (var el_section in el.Elements())
                                            {
                                                if (el_section.Name.LocalName == "p")
                                                    textBoxOut.Text += ($"\r\n   {el_section.Value}");
                                                if (el_section.Name.LocalName == "h1")
                                                    textBoxOut.Text += ($"\r\n\t\t{el_section.Value.ToUpper()}");
                                                if (el_section.Name.LocalName == "h2")
                                                    textBoxOut.Text += ($"\r\n\t{el_section.Value.ToUpper()}");
                                                if (el_section.Name.LocalName == "h3")
                                                    textBoxOut.Text += ($"\r\n\t{el_section.Value}");
                                            }
                                            break;
                                        case "table":
                                            foreach (var el_table in el.Elements()) // строчки
                                            {
                                                foreach (var el_tr in el_table.Elements()) // столбцы
                                                {
                                                    textBoxOut.Text += ($"{el_tr.Value}\t");
                                                }
                                                textBoxOut.Text += ($"\r\n\n");
                                            }
                                            break;
                                        case "ul":
                                            foreach (var el_ul in el.Elements())
                                            {
                                                if (el_ul.Name.LocalName == "li")
                                                    textBoxOut.Text += ($"\r\n   · {el_ul.Value}");
                                            }
                                            break;
                                        case "p": textBoxOut.Text += ($"\r\n   {el.Value}"); break;
                                        case "h1": textBoxOut.Text += ($"\r\n\t\t{el.Value.ToUpper()}"); break;
                                        case "h2": textBoxOut.Text += ($"\r\n\t{el.Value.ToUpper()}"); break;
                                        case "h3": textBoxOut.Text += ($"\r\n\t{el.Value}"); break;
                                        default: break;
                                    }
                                }
                            }

                            if (item.Name.LocalName == "p")
                                textBoxOut.Text += ($"\r\n   {item.Value}");
                            if (item.Name.LocalName == "h1")
                                textBoxOut.Text += ($"\r\n\t\t{item.Value.ToUpper()}");
                            if (item.Name.LocalName == "h2")
                                textBoxOut.Text += ($"\r\n\t{item.Value.ToUpper()}");
                            if (item.Name.LocalName == "h3")
                                textBoxOut.Text += ($"\r\n\t{item.Value}");
                        }
                        break;

                    default:
                        try
                        {
                            StreamReader fStr = new StreamReader(filePath, Encoding.GetEncoding(codePage));
                            string s;
                            while ((s = fStr.ReadLine()) != null) textBoxOut.Text += ($"\r\n{s}");
                            fStr.Close();
                        }
                        catch
                        {
                            StreamReader fStr = new StreamReader(filePath, Encoding.GetEncoding(Convert.ToInt32(codePage)));
                            string s;
                            while ((s = fStr.ReadLine()) != null) textBoxOut.Text += ($"\r\n{s}");
                            fStr.Close();
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                textBoxOut.Text += ($"\r\nОшибка открытия файла: {e.Message}");
            }
        }

        private static void OpenXML(TextBox textBoxOut, XNode node)
        {
            if (node.NodeType == XmlNodeType.Comment)
            {
                textBoxOut.Text += ($"\r\n{node}"); return; }
            XElement e = (XElement)node;
            textBoxOut.Text += ($"{e.Name} : ");
            if (!e.HasElements)//Если элемент не имеет дочерних элементов
            {
                textBoxOut.Text += ($"\r\n{e.Value}");   //вывод элемента
                if (e.HasAttributes)          //и его атрибутов
                    foreach (var at in e.Attributes()) textBoxOut.Text += ($"\r\n{at}");
            }
            else
            {
                textBoxOut.Text += ($"\r\n");
                foreach (var nd in e.Nodes()) OpenXML(textBoxOut, nd);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
