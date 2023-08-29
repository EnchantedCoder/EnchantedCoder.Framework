using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Diagnostics.Contracts;

namespace EnchantedCoder.Services.FileStorage
{
	/// <summary>
	/// Parametry šifrování file storage.
	/// </summary>
	public abstract class EncryptionOptions
	{
		/// <summary>
		/// Vrátí encryptor.
		/// </summary>
		public abstract ICryptoTransform CreateEncryptor();

		/// <summary>
		/// Vrátí decryptor.
		/// </summary>
		public abstract ICryptoTransform CreateDecryptor();
	}
}
