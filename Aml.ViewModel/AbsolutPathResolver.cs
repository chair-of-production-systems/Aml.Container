using System;
using System.IO;
using Aml.Contracts;

namespace Aml.ViewModel
{
	/// <summary>
	/// Resolves any absolute file URI.
	/// </summary>
	public class AbsolutPathResolver : ILocationResolver
	{
		/// <inheritdoc />
		public Stream GetStream(Uri location)
		{
			if (location == null) return null;
			if (location.Scheme != Uri.UriSchemeFile) return null;
			if (!location.IsAbsoluteUri) return null;

			var fullPath = Path.GetFullPath(location.LocalPath);
			return GetStream(fullPath);
		}

		private Stream GetStream(string fullPath)
		{
			if (!File.Exists(fullPath)) return null;
			return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
	}
}