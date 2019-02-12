using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace Aml.Container
{
	internal static class InternalExtensions
	{
		/// <summary>
		/// Copies the specified input stream to the output stream
		/// (it seems like .NET does not provide that functionality inherently).<br/>
		/// May be used to copy a memory stream into a file stream (for instance) or any other type of streams.
		/// Doesn't close either stream.
		/// </summary>
		/// <param name="input">The input stream.</param>
		/// <param name="output">The output stream.</param>
		/// <example>
		///     The following sample shows how to copy a memory stream into a file stream:
		///     <code>
		/// using (var memStream = new MemoryStream())
		/// {
		/// 	using (var fileStream = new FileStream(@"outfile.bin", FileMode.Create))
		/// 	{
		///			memStream.CopyStreamTo(fileStream);
		/// 	}
		/// }
		/// </code>
		/// </example>
		public static void CopyStreamTo(this Stream input, Stream output)
		{
			var buffer = new byte[8192];
			var bytesWritten = 0;

			int bytesRead;
			while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				output.Write(buffer, 0, bytesRead);
				bytesWritten += bytesRead;
				if (bytesWritten > 7000000)
				{
					// flush file stream, in order to prevent creation of an IsolatedStorage,
					// which becomes automatically created above 8 MB and since we get an
					// IsolatedStorageException if the package tries to create one
					output.Flush();
					bytesWritten = 0;
				}
			}
		}

		/// <summary>
		/// Given an absolute or relative <see cref="Uri" /> with a fragment or not
		/// this method extracts the filename including its extension.
		/// </summary>
		/// <param name="uri">The uri.</param>
		/// <returns>The name of the file and its extension or null,
		/// if the given uri does not contain any filename.</returns>
		/// <exception cref="System.ArgumentNullException">uri</exception>
		public static string GetFileName(this Uri uri)
		{
			if (uri == null) throw new ArgumentNullException(nameof(uri));

			string result;
			if (uri.IsAbsoluteUri)
			{
				if (uri.IsFile)
				{
					result = uri.LocalPath;
				}
				else
				{
					if (uri.Segments.Length > 0)
					{
						// in case of absolute uri (not a file), take the last segment 
						// (a fragment will not be part of a segment so it should do the job)
						return Path.GetFileName(uri.Segments.Last());
					}
					throw new UriFormatException("Unhandled Uri " + uri);
				}
			}
			else
			{
				result = uri.OriginalString;
			}

			// case: relative uri:
			var fragmentIndex = result.IndexOf("#", StringComparison.Ordinal);
			if (fragmentIndex >= 0)
			{
				result = result.Substring(0, fragmentIndex);
			}

			result = Path.GetFileName(result);
			return result;
		}

		/// <summary>
		/// Gets the relative path of the specified <see cref="PackagePart"/>.
		/// </summary>
		/// <param name="part">The part.</param>
		/// <returns>The relative path of the specified <see cref="PackagePart"/> or <c>null</c>.</returns>
		public static string GetRelativePath(this PackagePart part)
		{
			if (part == null) return null;
			var partUri = part.Uri;
			var path = partUri.ToString();
			if (path.StartsWith("/")) path = "." + path;
			if (!path.StartsWith("./")) path = "./" + path;
			return path;
		}
	}
}
