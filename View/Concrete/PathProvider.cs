using Microsoft.Win32;
using System;
using ViewModel.Interfaces;

namespace View.Concrete
{
    public class PathProvider : IPathProvider
    {
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <returns>The path.</returns>
        public string GetPath()
        {
            var dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            dialog.ShowDialog();

            return dialog.FileName;
        }
    }
}