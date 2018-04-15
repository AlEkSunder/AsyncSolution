using System;
using System.Windows;
using System.Windows.Threading;
using ViewModel.Interfaces;

namespace Presentation.Concrete
{
    public class ViewService : IViewService
    {
        /// <summary>
        /// Provides the service.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="DataContext">The data context.</param>
        public void ProvideService(object DataContext)
        {
            if (DataContext is Exception)
                this.ShowExceptionMessage((DataContext as Exception).Message);
            else if (DataContext is string && !string.IsNullOrWhiteSpace(DataContext as string))
                this.ShowCompleteMessage(DataContext as string);
        }

        /// <summary>
        /// Shows the exception.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="exception">The exception with message to show.</param>
        private void ShowMessageBox(string message, string caption, MessageBoxImage image)
        {
            Action action = new Action(() =>
            {
                MessageBox.Show(message, caption, MessageBoxButton.OK, image);
            });
            this.View?.Dispatcher.BeginInvoke(action, DispatcherPriority.Normal, null).Wait();
        }

        /// <summary>
        /// Shows the exception message.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="message">The message.</param>
        private void ShowExceptionMessage(string message)
        {
            this.ShowMessageBox(message, "Warning", MessageBoxImage.Warning);
        }

        /// <summary>
        /// Shows the complete message.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="message">The message.</param>
        private void ShowCompleteMessage(string message)
        {
            this.ShowMessageBox(message, "Complete", MessageBoxImage.Information);
        }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The view.
        /// </value>
        private DependencyObject View { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewService"/> class.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="view">The view.</param>
        public ViewService(DependencyObject view)
        {
            this.View = view;
        }
    }
}