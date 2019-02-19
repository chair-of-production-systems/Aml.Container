namespace Aml.ViewModel
{
	public interface IFileViewModelFactory
	{
		/// <summary>
		/// List of possible file extensions.
		/// </summary>
		string[] Extensions { get; }

		/// <summary>
		/// Human readable name of the format.
		/// </summary>
		string Name { get; }
	}
}