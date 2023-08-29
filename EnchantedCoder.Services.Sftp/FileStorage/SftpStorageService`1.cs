﻿using EnchantedCoder.Services.FileStorage;

namespace EnchantedCoder.Services.Sftp.FileStorage
{
	/// <summary>
	/// Úložiště souborů jako klient SFTP serveru.
	/// </summary>
	public class SftpStorageService<TFileStorageContext> : SftpStorageService, IFileStorageService<TFileStorageContext>
		where TFileStorageContext : FileStorageContext
	{
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public SftpStorageService(SftpStorageServiceOptions<TFileStorageContext> options) : base(options)
		{
			// NOOP
		}
	}
}