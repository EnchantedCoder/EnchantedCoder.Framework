﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Services.FileStorage
{
	/// <summary>
	/// IFileStorageService s file systémem pro datové úložiště.
	/// </summary>
	public class FileSystemStorageService : FileStorageServiceBase, IFileStorageService
	{
		private readonly string storagePath;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		/// <param name="storagePath">Cesta k "rootu" použitého úložiště ve file systému.</param>
		public FileSystemStorageService(string storagePath) : this(storagePath, null)
		{
		}

		/// <summary>
		/// Konstruktor.
		/// </summary>
		/// <param name="storagePath">Cesta k "rootu" použitého úložiště ve file systému.</param>
		/// <param name="encryptionOptions">Parametry pro šifrování storage. Nepovinné.</param>
		// TODO šifrování storage: public
		internal FileSystemStorageService(string storagePath, EncryptionOptions encryptionOptions) : base(encryptionOptions)
		{
			this.storagePath = storagePath;
		}

		/// <summary>
		/// Vrátí true, pokud uložený soubor v úložišti existuje. Jinak false.
		/// </summary>
		public override bool Exists(string fileName)
		{
			return File.Exists(System.IO.Path.Combine(storagePath, fileName));
		}

		/// <summary>
		/// Vrátí stream s obsahem soubor z úložiště.
		/// </summary>
		protected override Stream PerformRead(string fileName)
		{
			return File.OpenRead(System.IO.Path.Combine(storagePath, fileName));
		}

		/// <summary>
		/// Zapíše obsah souboru z úložiště do streamu.
		/// </summary>
		protected override void PerformReadToStream(string fileName, Stream stream)
		{
			using (Stream fileStream = File.OpenRead(System.IO.Path.Combine(storagePath, fileName)))
			{
				fileStream.CopyTo(stream);
			}
		}

		/// <summary>
		/// Uloží stream do úložiště.
		/// </summary>
		protected override void PerformSave(string fileName, Stream fileContent, string contentType)
		{
			using (FileStream fileStream = File.Create(System.IO.Path.Combine(storagePath, fileName)))
			{
				fileContent.CopyTo(fileStream);
			}
		}

		/// <summary>
		/// Smaže soubor v úložišti.
		/// </summary>
		public override void Delete(string fileName)
		{
			System.IO.File.Delete(System.IO.Path.Combine(storagePath, fileName));
		}

		/// <summary>
		/// Vylistuje seznam souborů v úložišti.
		/// </summary>
		public override IEnumerable<string> EnumerateFiles(string searchPattern = null)
		{
			return System.IO.Directory.EnumerateFiles(storagePath, searchPattern ?? "*", SearchOption.TopDirectoryOnly).Select(filename => filename.Substring(storagePath.Length).TrimStart('\\'));
		}

		/// <summary>
		/// Vrátí čas poslední modifikace souboru v UTC timezone.
		/// </summary>
		public override DateTime? GetLastModifiedTimeUtc(string fileName)
		{
			return File.GetLastWriteTimeUtc(Path.Combine(storagePath, fileName));
		}
	}
}
