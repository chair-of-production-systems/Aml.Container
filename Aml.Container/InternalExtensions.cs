using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Http;

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

		public static Stream GetStreamForAbsoluteUri(this Uri uri, HttpClient client)
		{
			if (!uri.IsAbsoluteUri) throw new Exception("Uri must be absolute");

			if (uri.Scheme == Uri.UriSchemeFile)
			{
				return new FileStream(uri.AbsolutePath, FileMode.Open, FileAccess.Read);
			}

			var stream = client.GetStreamAsync(uri).Result;
			return stream;
		}
	}
}
