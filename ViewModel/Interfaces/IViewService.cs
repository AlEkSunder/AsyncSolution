namespace ViewModel.Interfaces
{
    /// <summary>
	/// Defines the functionality for services to handle presentation responsibilities.
    /// </summary>
    /// <owner>Aleksey Beletsky</owner>
    public interface IViewService
    {
        /// <summary>
        /// Provides the service.
        /// </summary>
        /// <owner>Aleksey Beletsky</owner>
        /// <param name="DataContext">The data context.</param>
        void ProvideService(object DataContext);
    }
}