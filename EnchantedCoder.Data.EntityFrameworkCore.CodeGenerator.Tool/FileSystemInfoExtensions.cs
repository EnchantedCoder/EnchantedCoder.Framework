using System.IO;

namespace EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator.Tool
{
	public static class FileSystemInfoExtensions
	{
		public static string GetRelativePath(this FileSystemInfo fsObject, DirectoryInfo parent)
		{
			return fsObject.FullName.Replace(parent.FullName, string.Empty).Trim(Path.DirectorySeparatorChar);
		}
	}
}