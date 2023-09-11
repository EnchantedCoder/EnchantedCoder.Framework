using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using EnchantedCoder.Services.FileStorage;

namespace EnchantedCoder.Services.Azure.FileStorage
{
	/// <summary>
	/// Parametry pro AzureFileStorageService.
	/// Generický parametr TFileStorageContext určen pro možnost použití několika různých služeb v IoC containeru.
	/// </summary>
	public class AzureFileStorageServiceOptions<TFileStorageServiceContext> : AzureFileStorageServiceOptions
		where TFileStorageServiceContext : FileStorageContext
	{
	}
}
