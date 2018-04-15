using System;
using System.Windows.Input;
using ViewModel.Interfaces;

namespace ViewModel.Concrete
{
    /// <summary>
    /// Provides logic for command handling. 
    /// </summary>
    /// <owner>Aleksey Beletsky</owner>
    /// <seealso cref="ICommand" />
    public class CommandHandler : ICommand
    {
        /// <summary>
        /// Holds the action.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private Action action;

        /// <summary>
        /// Holds the view service for showing information for user.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        private IViewService viewService;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="action">The action.</param>
        /// <param name="viewService">The view service.</param>
        public CommandHandler(Action action, IViewService viewService)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.viewService = viewService ?? throw new ArgumentNullException(nameof(viewService));
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            try
            {
                this.action();
            }
            catch (Exception exception)
            {
                this.viewService.ProvideService(exception);
            }
        }
    }
}