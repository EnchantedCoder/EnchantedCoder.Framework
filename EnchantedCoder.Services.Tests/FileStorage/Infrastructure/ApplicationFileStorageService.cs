using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Services.FileStorage;

namespace EnchantedCoder.Services.Tests.FileStorage.Infrastructure
{
	public class ApplicationFileStorageService : FileStorageWrappingService<TestUnderlyingFileStorage>, IApplicationFileStorageService
	{
		public ApplicationFileStorageService(IFileStorageService<TestUnderlyingFileStorage> fileStorageService) : base(fileStorageService)
		{
		}
	}
}
