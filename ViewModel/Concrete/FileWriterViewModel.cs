using BLL.Concrete;
using BLL.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModel.Concrete;
using ViewModel.Interfaces;

namespace ViewModel
{
    public class FileWriterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Holds the browse command.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private ICommand browse;

        /// <summary>
        /// Holds available engine types.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private IEnumerable<EngineType> engineTypes;

        /// <summary>
        /// Holds a value indicating is command enabled.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private bool isCommandEnabled = true;

        /// <summary>
        /// Holds the path.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private string path;

        /// <summary>
        /// Holds the path provider.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private IPathProvider pathProvider;

        /// <summary>
        /// Holds the view service.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private IViewService viewService;

        /// <summary>
        /// Holds the write command.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private ICommand write;

        /// <summary>
        /// Gets the browse command.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The browse.
        /// </value>
        public ICommand Browse
        {
            get
            {
                return this.write ?? (this.browse = new CommandHandler(this.BrowseFile, this.viewService));
            }
        }

        /// <summary>
        /// Browses the file.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private void BrowseFile()
        {
            this.Path = this.pathProvider.GetPath();
        }

        /// <summary>
        /// Gets the engine types.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The engine types.
        /// </value>
        public IEnumerable<EngineType> EngineTypes
        {
            get
            {
                if (this.engineTypes == null)
                    this.engineTypes = this.GetEngineTypes();

                return this.engineTypes;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWriterViewModel"/> class.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="viewService">The view service.</param>
        /// <param name="pathProvider">The path provider.</param>
        public FileWriterViewModel(IViewService viewService, IPathProvider pathProvider)
        {
            this.viewService = viewService ?? throw new ArgumentNullException(nameof(viewService));
            this.pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        }

        /// <summary>
        /// Gets the engine types.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <returns>The available engine types.</returns>
        private IEnumerable<EngineType> GetEngineTypes()
        {
            return new[]
            {
                EngineType.AsyncAwait,
                EngineType.BeginEndInvoke,
                EngineType.TaskBasedAsyncPattern,
                EngineType.ThreadBased
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether the command is enabled.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        ///   <c>true</c> if the command is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCommandEnabled
        {
            get
            {
                return this.isCommandEnabled;
            }
            set
            {
                this.isCommandEnabled = value;
                this.OnPropertyChanged(nameof(this.IsCommandEnabled));
            }
        }

        /// <summary>
        /// Called when the property is changed.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The path.
        /// </value>
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                this.OnPropertyChanged(nameof(this.Path));
            }
        }

        /// <summary>
        /// Occurs when a property changes the value.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the selected engine.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The selected engine.
        /// </value>
        public EngineType SelectedEngine { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets the write.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <value>
        /// The write.
        /// </value>
        public ICommand Write
        {
            get
            {
                return this.write ?? (this.write = new CommandHandler(this.WriteFile, this.viewService));
            }
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private async void WriteFile()
        {
            this.IsCommandEnabled = false;
            try
            {
                switch (this.SelectedEngine)
                {
                    case EngineType.AsyncAwait:
                        {
                            var creatorAsync = new FileCreatorAsync();
                            await creatorAsync.CreateFileAsync(this.Path, this.Text);
                            break;
                        }

                    case EngineType.BeginEndInvoke:
                        {
                            var creator = new FileCreator();
                            var delegat = new Action<string, string>(creator.CreateFile);
                            IAsyncResult result = delegat.BeginInvoke(this.Path, this.Text, null, null);
                            result.AsyncWaitHandle.WaitOne();
                            delegat.EndInvoke(result);
                            break;
                        }

                    case EngineType.TaskBasedAsyncPattern:
                        var creatorTap = new FileCreator();
                        await Task.Run(() =>
                        {
                            creatorTap.CreateFile(this.Path, this.Text);
                        });
                        break;

                    case EngineType.ThreadBased:
                        var creatorThread = new FileCreator(this.Path, this.Text);
                        var thread = new Thread(creatorThread.CreateFile);
                        thread.Start();
                        break;
                }

                this.viewService.ProvideService("The file was created successfully.");
                this.IsCommandEnabled = true;
            }
            catch (Exception exception)
            {
                this.viewService.ProvideService(exception);
            }
        }
    }
}