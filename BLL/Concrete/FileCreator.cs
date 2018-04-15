using System;
using System.IO;
using System.Text;
using System.Threading;

namespace BLL.Concrete
{
    public class FileCreator
    {
        /// <summary>
        /// Holds the path.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private string path;

        /// <summary>
        /// Holds the text.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private string text;

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="path">The path.</param>
        /// <param name="text">The text.</param>
        public void CreateFile(string path, string text)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException($"{nameof(path)} should be specified.");

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException($"{nameof(text)} should be specified.");

            this.CreateFile();
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        public void CreateFile()
        {
            var fileName = $"{Path.GetFileName(this.path)}_{DateTime.Now.Ticks}.txt";
            var fullPath = $"{Path.GetDirectoryName(this.path)}\\{fileName}";

            using (var stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.Default))
                {
                    Thread.Sleep(5000);
                    streamWriter.Write(this.text);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCreator"/> class.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        public FileCreator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCreator"/> class.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="path">The path.</param>
        /// <param name="text">The text.</param>
        public FileCreator(string path, string text)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));

            this.path = path;
            this.text = text;
        }
    }
}