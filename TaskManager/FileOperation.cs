using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;//для запуска файлов

namespace TaskManager
{
    class FileOperation
    {
        /// <summary>
        /// копирование одного файла
        /// </summary>
        /// <param name="inFile">исходный файл</param>
        /// <param name="outFile">целевой файл</param>
        /// <returns></returns>
        public bool CopyFile(string inFile, string outFile)
        {
            try
            {
                System.IO.File.Copy(inFile, outFile, true);
            }
            catch (IOException copyError)
            {
                MessageBox.Show(copyError.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// перенос одного файла
        /// </summary>
        /// <param name="inFile">исходный файл</param>
        /// <param name="outFile">целевой файл</param>
        public bool MoveFile(string inFile, string outFile)
        {
            if (System.IO.File.Exists(outFile) == false)
            {
                System.IO.File.Move(inFile, outFile);
            }
            else
            {
                try
                {
                    DeleteFile(outFile);
                    System.IO.File.Move(inFile, outFile);
                }
                catch (IOException copyError)
                {
                    MessageBox.Show(copyError.Message);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// удаление одного файла без подтверждения
        /// </summary>
        /// <param name="inFile">файл</param>
        public void DeleteFile(string inFile)
        {
            if (System.IO.File.Exists(inFile))
            {
                try
                {
                    System.IO.File.Delete(inFile);
                }
                catch (System.UnauthorizedAccessException)
                {
                    File.SetAttributes(inFile, File.GetAttributes(inFile) & ~(FileAttributes.Hidden |
                        FileAttributes.ReadOnly | FileAttributes.System));
                    System.IO.File.Delete(inFile);
                }
            }
        }
        /// <summary>
        /// выполнить файл
        /// </summary>
        /// <param name="inFile">полное имя файла</param>
        public void RunFile(string inFile)
        {
            if (System.IO.File.Exists(inFile))
            {
                try
                {
                    System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(inFile));
                    ProcessStartInfo PInfo = new ProcessStartInfo();
                    PInfo.FileName = inFile;
                    PInfo.UseShellExecute = true;
                    Process p = Process.Start(PInfo);
                }
                catch
                {
                    //
                }
            }
        }
        /// <summary>
        /// создать папку
        /// </summary>
        /// <param name="nameNewDir">имя папки</param>
        public void MakeDir(string nameNewDir)
        {
            try
            {
                System.IO.Directory.CreateDirectory(nameNewDir);
            }
            catch (IOException error)
            {
                MessageBox.Show(error.Message);
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Програма не має прав на створення папки", nameNewDir);
            }
        }
        /// <summary>
        /// вернуть список файлов
        /// </summary>
        /// <param name="folder">имя папки</param>
        /// <param name="pattern">шаблон имён файлов</param>
        /// <param name="csvFiles">список имён файлов</param>
        public void FilesToList(string folder, string pattern, ref List<string> csvFiles)
        {
            csvFiles.Clear();
            if (System.IO.Directory.Exists(folder))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder);
                FileInfo[] fiArr = di.GetFiles(pattern);
                foreach (var fi in fiArr)
                {
                    csvFiles.Add(fi.Name);
                }
            }
        }
    }
}
