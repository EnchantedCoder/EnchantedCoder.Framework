﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Havit.Business.CodeMigrations.ExtendedProperties;
using Havit.Business.CodeMigrations.ExtendedProperties.Attributes;
using Havit.Business.CodeMigrations.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Havit.Business.CodeMigrations.Conventions
{
    public static class IndexesConventions
    {
        private const string GenerateIndexesPropertyName = "GenerateIndexes";

        public static void ApplyBusinessLayerIndexes(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => e.GetBoolExtendedProperty(GenerateIndexesPropertyName) ?? true))
            {
	            RenameForeignKeyIndexes(entityType.GetForeignKeys());

                if (entityType.IsJoinEntity())
				{
					// TODO
				}
                else
                {
                    AddNormalTableIndexes(entityType);
                }
            }
        }

        private static void AddNormalTableIndexes(IMutableEntityType entityType)
        {
            IMutableProperty deletedProperty = entityType.GetDeletedProperty();

            foreach (IMutableProperty property in entityType.GetNotIgnoredProperties()
                .Where(e => e.GetBoolExtendedProperty(GenerateIndexesPropertyName) ?? true))
            {
                if (property.IsPrimaryKey() || !property.IsForeignKey())
                {
                    continue;
                }

                IMutableIndex index;
                if (deletedProperty != null)
                {
                    index = entityType.GetOrAddIndex(new[] { property, deletedProperty });
                }
                else
                {
                    index = entityType.GetOrAddIndex(property);
                }

                ReplaceIndexPrefix(index);
            }

	        CreateCollectionOrderIndex(entityType);

			if (entityType.IsLocalizationEntity())
            {
                IMutableProperty parentLocalizationProperty = entityType.FindProperty(entityType.GetLocalizationParentEntityType().FindPrimaryKey().Properties[0].Name);
                IMutableProperty languageProperty = entityType.GetLanguageProperty();

                ReplaceIndexPrefix(entityType.GetOrAddIndex(new[] { parentLocalizationProperty, languageProperty }));
            }

            if (entityType.IsLanguageEntity())
            {
                IMutableProperty uiCultureProperty = entityType.GetUICultureProperty();
                if (uiCultureProperty != null)
                {
                    ReplaceIndexPrefix(entityType.GetOrAddIndex(uiCultureProperty));
                }
            }
        }

	    private static void CreateCollectionOrderIndex(IMutableEntityType entityType)
	    {
		    IMutableProperty deletedProperty = entityType.GetDeletedProperty();

			// Find matching navigations using foreign keys
			// We need navigations, since we want to extract Order By properties from Collection attribute (Sorting property)
		    var collectionsIntoEntity = entityType.GetForeignKeys()
			    .Select(fk => fk.PrincipalEntityType.GetNavigations().FirstOrDefault(n => n.ForeignKey == fk))
			    .Where(n => n != null)
			    .ToArray();

			foreach (IMutableNavigation navigation in collectionsIntoEntity)
		    {
			    string sorting = new CollectionAttributeAccessor(navigation).Sorting;
			    if (string.IsNullOrEmpty(sorting))
			    {
				    continue;
			    }

			    List<string> orderByProperties = Regex.Matches(sorting, "(^|[^{])({([^{}]*)}|\\[([^\\[\\]]*)\\])")
				    .Cast<Match>()
				    .Where(m => m.Success && m.Groups[4].Success)
				    .Select(m => m.Groups[4].Value)
				    .ToList();

			    if (orderByProperties.Count == 0)
			    {
				    continue;
			    }

			    orderByProperties.Remove(navigation.ForeignKey.Properties[0].Name);
			    orderByProperties.Insert(0, navigation.ForeignKey.Properties[0].Name);

			    List<IMutableProperty> indexProperties = orderByProperties.Select(entityType.FindProperty).ToList();

			    if (deletedProperty != null)
			    {
				    indexProperties.Add(deletedProperty);
			    }

			    entityType.GetOrAddIndex(indexProperties);
		    }
	    }

	    private static void RenameForeignKeyIndexes(IEnumerable<IMutableForeignKey> foreignKeys)
	    {
		    foreach (IMutableIndex index in foreignKeys
			    .Select(k => k.DeclaringEntityType.FindIndex(k.Properties))
			    .Where(index => index != null))
		    {
		        ReplaceIndexPrefix(index);
		    }
	    }

        private static void ReplaceIndexPrefix(IMutableIndex index)
        {
            if (index.Relational().Name.StartsWith("IX_"))
            {
                index.Relational().Name = "FKX_" + index.Relational().Name.Substring(3);
            }
        }
    }
}