﻿using Havit.Data.EntityFrameworkCore.Patterns.Caching;
using Havit.Data.EntityFrameworkCore.Patterns.Tests.DataLoader.Model;
using Havit.Services;
using Havit.Services.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Data.EntityFrameworkCore.Patterns.Tests.Caching
{
	[TestClass]
	public class EntityCacheManagerTests
	{
		[TestMethod]
		public void EntityCacheManager_Store_CallsShouldCacheEntity()
		{
            // Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            Role role = new Role { Id = 100 };
            dbContext.Attach(role);
			
			Mock<IEntityCacheSupportDecision> entityCacheSupportDecisionMock = new Mock<IEntityCacheSupportDecision>(MockBehavior.Strict);
			entityCacheSupportDecisionMock.Setup(m => m.ShouldCacheEntity(role)).Returns(false);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                entityCacheSupportDecision: entityCacheSupportDecisionMock.Object
            );

            // Act
			entityCacheManager.StoreEntity(role);

			// Assert
			entityCacheSupportDecisionMock.Verify(m => m.ShouldCacheEntity(role), Times.Once);
		}

		[TestMethod]
		public void EntityCacheManager_Store_CallsCacheServiceAddWhenShouldCache()
		{
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            Role role = new Role { Id = 100 };
            dbContext.Attach(role);

            Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			cacheServiceMock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheOptions>()));

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                cacheService: cacheServiceMock.Object);

			entityCacheManager.StoreEntity(role);

			// Assert
			cacheServiceMock.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheOptions>()), Times.Once);
		}

		[TestMethod]
		public void EntityCacheManager_Store_DoesNotCallCacheServiceAddWhenShouldNotCache()
		{
            // Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            Role role = new Role { Id = 100 };
            dbContext.Attach(role);

            Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			cacheServiceMock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheOptions>()));

			Mock<IEntityCacheSupportDecision> entityCacheSupportDecisionMock = new Mock<IEntityCacheSupportDecision>(MockBehavior.Strict);
			entityCacheSupportDecisionMock.Setup(m => m.ShouldCacheEntity(role)).Returns(false);

            EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                entityCacheSupportDecision: entityCacheSupportDecisionMock.Object,
                cacheService: cacheServiceMock.Object);

            // Act
			entityCacheManager.StoreEntity(role);

			// Assert
			cacheServiceMock.Verify(m => m.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheOptions>()), Times.Never);
		}
        
		[TestMethod]
		public void EntityCacheManager_TryGet_CallsCacheServiceTryGetWhenCouldCache()
		{
            // Arrange
			Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			object tryGetOutParameter;
			cacheServiceMock.Setup(m => m.TryGet(It.IsAny<string>(), out tryGetOutParameter)).Returns(false);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(cacheService: cacheServiceMock.Object);

            // Act
            var result = entityCacheManager.TryGetEntity<Role>(555, out Role role);

			// Assert
			cacheServiceMock.Verify(m => m.TryGet(It.IsAny<string>(), out tryGetOutParameter), Times.Once);
		}
        
		[TestMethod]
		public void EntityCacheManager_TryGet_DoesNotCallCacheServiceTryGetWhenShouldNotCache()
		{
            // Arrange
			Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			object tryGetOutParameter;
			cacheServiceMock.Setup(m => m.TryGet(It.IsAny<string>(), out tryGetOutParameter)).Returns(false);

			Mock<IEntityCacheSupportDecision> entityCacheSupportDecisionMock = new Mock<IEntityCacheSupportDecision>(MockBehavior.Strict);
			entityCacheSupportDecisionMock.Setup(m => m.ShouldCacheEntity<Role>()).Returns(false);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                cacheService: cacheServiceMock.Object, 
                entityCacheSupportDecision: entityCacheSupportDecisionMock.Object);

            // Act
            var result = entityCacheManager.TryGetEntity<Role>(555, out Role langauge);

			// Assert
			cacheServiceMock.Verify(m => m.TryGet(It.IsAny<string>(), out tryGetOutParameter), Times.Never);
		}
        
		[TestMethod]
		public void EntityCacheManager_Scenarion_Store_And_TryGet()
		{
            // Arrange
			ICacheService cacheService = new DictionaryCacheService();

            DataLoaderTestDbContext dbContext1 = new DataLoaderTestDbContext();
			var entityCacheManager1 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext1, cacheService: cacheService);

            DataLoaderTestDbContext dbContext2 = new DataLoaderTestDbContext();
			var entityCacheManager2 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext2, cacheService: cacheService);

            Role role = new Role { Id = 100, Name = "Reader" };
			dbContext1.Attach(role);

			// Act
			entityCacheManager1.StoreEntity(role);
			var success = entityCacheManager2.TryGetEntity<Role>(role.Id, out Role roleResult);

			// Assert
			Assert.IsTrue(success);
			Assert.IsNotNull(roleResult);
			Assert.AreNotSame(role, roleResult);
			Assert.AreEqual(roleResult.Name, roleResult.Name);
			Assert.AreEqual(dbContext1.Entry(role).CurrentValues.GetValue<string>(nameof(Role.Name)), dbContext2.Entry(roleResult).CurrentValues.GetValue<string>(nameof(Role.Name)));
			Assert.AreEqual(dbContext1.Entry(role).OriginalValues.GetValue<string>(nameof(Role.Name)), dbContext2.Entry(roleResult).OriginalValues.GetValue<string>(nameof(Role.Name)));
			Assert.AreEqual(Microsoft.EntityFrameworkCore.EntityState.Unchanged, dbContext2.Entry(roleResult).State);
		}

        [TestMethod]
        public void EntityCacheManager_Scenarion_StoreAllKeys_And_TryGetAllKeys()
        {
            // Arrange
            ICacheService cacheService = new DictionaryCacheService();

            var entityCacheManager1 = CachingTestHelper.CreateEntityCacheManager(cacheService: cacheService);
            var entityCacheManager2 = CachingTestHelper.CreateEntityCacheManager(cacheService: cacheService);

            object allKeys = new object(); // just a marker object
            // Act
            entityCacheManager1.StoreAllKeys<Role>(allKeys);
            bool success = entityCacheManager2.TryGetAllKeys<Role>(out object allKeysResult);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNotNull(allKeysResult);
            Assert.AreSame(allKeys, allKeysResult);
        }

        [TestMethod]
        public void EntityCacheManager_Scenarion_OneToMany_StoreCollection_And_TryGetCollection()
        {
            // Arrange
            ICacheService cacheService = new DictionaryCacheService();

            DataLoaderTestDbContext dbContext1 = new DataLoaderTestDbContext();

            Master master = new Master { Id = 1 };
            Child child1 = new Child { Id = 100, ParentId = 1, Parent = master };
            Child child2 = new Child { Id = 101, ParentId = 1, Parent = master, Deleted = DateTime.Now };
            master.ChildrenWithDeleted.Add(child1);
            master.ChildrenWithDeleted.Add(child2);
            dbContext1.Attach(master);
            //dbContext1.Attach(child1);
            //dbContext1.Attach(child2);

            var entityCacheManager1 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext1, cacheService: cacheService);

            DataLoaderTestDbContext dbContext2 = new DataLoaderTestDbContext();
            Master masterResult = new Master { Id = 1 };
            dbContext2.Attach(masterResult);

            var entityCacheManager2 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext2, cacheService: cacheService);

            // Act
            // TODO JK: Testy failují, protože nejsou v cache položky, ale jen jejich ID. Pokud je do cache přidáme (následující dva zakomentované řádky), testy jsou v pořádku.
            //entityCacheManager1.StoreEntity(child1);
            //entityCacheManager1.StoreEntity(child2);
            entityCacheManager1.StoreCollection<Master, Child>(master, nameof(Master.ChildrenWithDeleted));
            bool success = entityCacheManager2.TryGetCollection<Master, Child>(masterResult, nameof(Master.ChildrenWithDeleted));

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(master.ChildrenWithDeleted.Count, masterResult.ChildrenWithDeleted.Count);
            Assert.IsTrue(masterResult.ChildrenWithDeleted.Any(child => child.Id == child1.Id));
            Assert.IsTrue(masterResult.ChildrenWithDeleted.Any(child => child.Id == child2.Id));
            Assert.AreEqual(4, master.ChildrenWithDeleted.Union(masterResult.ChildrenWithDeleted).Distinct().Count()); // nejsou sdílené žádné instance (tj. master.Children[0] != master.Children[1] != masterResult.Children[0] != masterResult.Children[1]
        }

        [TestMethod]
        public void EntityCacheManager_Scenarion_ManyToMany_StoreCollection_And_TryGetCollection()
        {
            // Arrange
            ICacheService cacheService = new DictionaryCacheService();

            DataLoaderTestDbContext dbContext1 = new DataLoaderTestDbContext();
            
            LoginAccount loginAccount = new LoginAccount { Id = 1 };
            Membership membership = new Membership { LoginAccountId = 1, RoleId = 1234 };
            loginAccount.Roles = new List<Membership> { membership };

            var entityCacheManager1 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext1, cacheService: cacheService);

            DataLoaderTestDbContext dbContext2 = new DataLoaderTestDbContext();
            LoginAccount loginAccountResult = new LoginAccount { Id = 1 };
            dbContext2.Attach(loginAccountResult);

            var entityCacheManager2 = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext2, cacheService: cacheService);

            // Act
            entityCacheManager1.StoreCollection<LoginAccount, Membership>(loginAccount, nameof(LoginAccount.Roles));
            bool success = entityCacheManager2.TryGetCollection<LoginAccount, Membership>(loginAccountResult, nameof(LoginAccount.Roles));

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(1, loginAccountResult.Roles.Count);
            Assert.AreEqual(membership.RoleId, loginAccountResult.Roles[0].RoleId);
            Assert.AreNotSame(loginAccount.Roles[0], loginAccountResult.Roles[0]);
        }
        
        [TestMethod]
		public void EntityCacheManager_InvalidateEntity_RemovesEntityOnUpdate()
		{
            // Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            LoginAccount loginAccount = new LoginAccount { Id = 1 };
            dbContext.Attach(loginAccount);

			Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			cacheServiceMock.Setup(m => m.Remove(It.IsAny<string>()));
			cacheServiceMock.SetupGet(m => m.SupportsCacheDependencies).Returns(false);

			var entityCacheKeyGenerator = new EntityCacheKeyGenerator();
			string cacheKey = entityCacheKeyGenerator.GetEntityCacheKey(typeof(LoginAccount), loginAccount.Id);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                cacheService: cacheServiceMock.Object,
                entityCacheKeyGenerator: entityCacheKeyGenerator);

			// Act
			entityCacheManager.InvalidateEntity(Patterns.UnitOfWorks.ChangeType.Update, loginAccount);

			// Assert
			cacheServiceMock.Verify(m => m.Remove(cacheKey), Times.Once); // volá se ještě pro AllKeys, tak musíme kontrolovat jen klíč pro entitu
		}
        
		[TestMethod]
		public void EntityCacheManager_InvalidateEntity_DoesNotRemoveEntityOnInsert()
		{
            // Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            LoginAccount loginAccount = new LoginAccount { Id = 1 };
            dbContext.Attach(loginAccount);

			Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			cacheServiceMock.Setup(m => m.Remove(It.IsAny<string>()));
			cacheServiceMock.SetupGet(m => m.SupportsCacheDependencies).Returns(false);

			var entityCacheKeyGenerator = new EntityCacheKeyGenerator();
			string cacheKey = entityCacheKeyGenerator.GetEntityCacheKey(typeof(LoginAccount), loginAccount.Id);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                cacheService: cacheServiceMock.Object,
                entityCacheKeyGenerator: entityCacheKeyGenerator);

			// Act
			entityCacheManager.InvalidateEntity(Patterns.UnitOfWorks.ChangeType.Insert, loginAccount);

			// Assert
			cacheServiceMock.Verify(m => m.Remove(cacheKey), Times.Never); // volá se ještě pro AllKeys, tak musíme kontrolovat jen klíč pro entitu
		}
        
		[TestMethod]
		public void EntityCacheManager_InvalidateEntity_RemovesDependencies()
		{
            // Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
            LoginAccount loginAccount = new LoginAccount { Id = 1 };
            dbContext.Attach(loginAccount);

            Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
			cacheServiceMock.Setup(m => m.Contains(It.IsAny<string>())).Returns(false);
			cacheServiceMock.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheOptions>()));
			cacheServiceMock.Setup(m => m.Remove(It.IsAny<string>()));
			cacheServiceMock.SetupGet(m => m.SupportsCacheDependencies).Returns(true);

			EntityCacheDependencyManager entityCacheDependencyManager = new EntityCacheDependencyManager(cacheServiceMock.Object);
			string cacheKey = entityCacheDependencyManager.GetSaveCacheDependencyKey(typeof(LoginAccount), loginAccount.Id);

			EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(
                dbContext: dbContext,
                cacheService: cacheServiceMock.Object,
                entityCacheDependencyManager: entityCacheDependencyManager);

			// Act
			entityCacheManager.InvalidateEntity(Patterns.UnitOfWorks.ChangeType.Update, loginAccount);

			// Assert
			cacheServiceMock.Verify(m => m.Remove(cacheKey), Times.Once); // volá se ještě pro entitu a AllKeys, tak musíme kontrolovat jen klíč pro entitu
		}
       
		[TestMethod]
		public void EntityCacheManager_InvalidateEntity_SupportsManyToMany()
		{
			// Arrange
            DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();
			Membership membership = new Membership { LoginAccountId = 100, RoleId = 999 };
            dbContext.Attach(membership);

            EntityCacheManager entityCacheManager = CachingTestHelper.CreateEntityCacheManager(dbContext: dbContext);

			// Act
			entityCacheManager.InvalidateEntity(Patterns.UnitOfWorks.ChangeType.Delete, membership);

			// Assert
			// no exception was thrown
		}               
	}
}
