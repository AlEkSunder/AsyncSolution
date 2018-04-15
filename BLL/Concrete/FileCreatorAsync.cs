using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Concrete
{
    public class FileCreatorAsync
    {
        /// <summary>
        /// Creates the file asynchronous.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="path">The path.</param>
        /// <param name="text">The text.</param>
        /// <returns>The task for creation file.</returns>
        public async Task CreateFileAsync(string path, string text)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException($"{nameof(path)} should be specified.");

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException($"{nameof(text)} should be specified.");

            var fileName = $"{Path.GetFileName(path)}_{DateTime.Now.Ticks}.txt";
            var fullPath = $"{Path.GetDirectoryName(path)}\\{fileName}";

            using (var stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.Default))
                {
                    text = await this.GetFullText(text);
                    await streamWriter.WriteAsync(text);
                }
            }
        }

        /// <summary>
        /// Gets the full text.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="text">The text.</param>
        /// <returns>The full text.</returns>
        private Task<string> GetFullText(string text)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(5000);
                return text;
            });
        }
    }
}