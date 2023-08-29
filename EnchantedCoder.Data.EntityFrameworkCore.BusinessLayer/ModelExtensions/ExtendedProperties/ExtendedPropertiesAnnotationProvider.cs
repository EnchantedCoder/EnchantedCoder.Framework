using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.ExtendedProperties;
using EnchantedCoder.Data.EntityFrameworkCore.Migrations.ModelExtensions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.ModelExtensions.ExtendedProperties
{
	public class ExtendedPropertiesAnnotationProvider : IModelExtensionAnnotationProvider
	{
		public List<IAnnotation> GetAnnotations(IModelExtension dbAnnotation, MemberInfo memberInfo)
		{
			var attributes = memberInfo.GetCustomAttributes(typeof(ModelExtensionExtendedPropertiesAttribute), false).Cast<ModelExtensionExtendedPropertiesAttribute>();
			return attributes.SelectMany(attr => ExtendedPropertiesForExtraDatabaseObjectsBuilder.ForExtraDatabaseObject(attr.GetExtendedProperties(memberInfo), attr.ObjectType, dbAnnotation.ObjectName))
				.ToList();
		}

		public List<IModelExtension> GetModelExtensions(List<IAnnotation> annotations)
		{
			return new List<IModelExtension>();
		}
	}
}