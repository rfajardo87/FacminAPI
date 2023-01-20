using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace Models
{
    public class FCtx
    {

        private const string FileName = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        private const string path = "../Dts/";
        private string DbPathFileName;
        private string DbPathFile;

        public FCtx()
        {
            this.DbPathFileName = $"{path}{FileName}";
            this.openCreateDBFile();
        }

        private void openCreateDBFile()
        {
            try
            {
                foreach (string line in File.ReadLines(this.DbPathFileName))
                {
                    this.DbPathFile = $"{path}{line}";
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("No se pudo abrir el archivo");
                Console.WriteLine(ex.Message);
            }
        }

        public string getDB()
        {
            return this.DbPathFile;
        }
    }
}
