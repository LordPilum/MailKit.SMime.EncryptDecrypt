using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using MimeKit.Cryptography;

namespace MailKit.SMime.EncryptDecrypt.Common
{
    public class CertBase
    {
        private static SqliteCertificateDatabase Database { get; set; }

        public static IX509CertificateDatabase OpenDatabase(string fileName)
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                DateTimeFormat = SQLiteDateFormats.Ticks,
                DataSource = fileName
            };

            if (!File.Exists(fileName))
                SQLiteConnection.CreateFile(fileName);

            var sqlite = new SQLiteConnection(builder.ConnectionString);
            sqlite.Open();

            Database = new SqliteCertificateDatabase(sqlite, "password");

            return Database;
        }
    }
}
