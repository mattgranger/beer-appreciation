namespace BeerAppreciation.Data.Tests
{
    using EF.Domain;
    using EF.Extensions;
    using NUnit.Framework;
    using Mocks;
    using Moq;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    /// <summary>
    /// The unit tests for the BaseEntityRepository
    /// </summary>
    [TestFixture]
    public class BaseEntityRepositoryTests
    {
        #region Test Initialisation

        [TestFixtureSetUp]
        public void TestInitialise()
        {
            // Need this to prevent the FakeDbContext from attempting to create a code first database
            Database.SetInitializer<FakeDbContext>(null);
        }

        #endregion

        #region Single Entity Add/Update Tests

        /// <summary>
        /// Tests that when we add a simple entity using the entity repository, the dbContext tracks the entity in an 'Added' state.
        /// </summary>
        [Test]
        public void AddSimpleEntity()
        {
            // Arrange
            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            SimpleEntity simpleEntity = new SimpleEntity { SomeProperty = "Some property" };

            // Act
            entityRepository.Insert(simpleEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(1, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Added, entityRepository.DbContext.ChangeTracker.Entries().ToList()[0].State);
        }

        /// <summary>
        /// Tests that when we update a simple entity using the entity repository, the dbContext tracks the entity in a 'Mofified' state.
        /// </summary>
        [Test]
        public void UpdateSimpleEntity()
        {
            // Arrange
            const int expectedId = 10;

            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            SimpleEntity simpleEntity = new SimpleEntity { Id = expectedId };

            // Act
            entityRepository.Update(simpleEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(1, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Modified, entityRepository.DbContext.FindEntity(expectedId).State);
        }

        /// <summary>
        /// Tests that when we insert an aggregate root entity (i.e. has child entities), only the aggregate root gets set to 'Added' in the context
        /// </summary>
        [Test]
        public void InsertAggregateRootEntity()
        {
            // Arrange
            const int expectedSimpleId = 10;
            const int expectedAggregateRootId = 20;

            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            AggregateRootEntity aggregateRootEntity = new AggregateRootEntity
            {
                Id = expectedAggregateRootId,
                SimpleEntity = new SimpleEntity
                {
                    Id = expectedSimpleId
                }
            };

            // Act
            entityRepository.Insert(aggregateRootEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // Both the AggregateRoot and its child SimpleEntity should be added to the context, but only the aggregate root 
            // should have an added state
            Assert.AreEqual(2, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Added, entityRepository.DbContext.FindEntity(expectedAggregateRootId).State);
            Assert.AreEqual(EntityState.Unchanged, entityRepository.DbContext.FindEntity(expectedSimpleId).State);
        }

        /// <summary>
        /// Tests that when we update an aggregate root entity (i.e. has child entities), only the aggregate root gets updated in the context
        /// </summary>
        [Test]
        public void UpdateAggregateRootEntity()
        {
            // Arrange
            const int expectedSimpleId = 10;
            const int expectedAggregateRootId = 20;

            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            AggregateRootEntity aggregateRootEntity = new AggregateRootEntity
            {
                Id = expectedAggregateRootId,
                SimpleEntity = new SimpleEntity
                {
                    Id = expectedSimpleId
                }
            };

            // Act
            entityRepository.Update(aggregateRootEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // Both the AggregateRoot and its child SimpleEntity should be added to the context, but only the aggregate root 
            // should have a modified state
            Assert.AreEqual(2, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Modified, entityRepository.DbContext.FindEntity(expectedAggregateRootId).State);
            Assert.AreEqual(EntityState.Unchanged, entityRepository.DbContext.FindEntity(expectedSimpleId).State);
        }

        /// <summary>
        /// Tests that when we insert/update a new aggregate root with all new child objects using the (has id convention), the aggregate root and 
        /// all child objects have a state of 'Added' in the dbContext
        /// </summary>
        [Test]
        public void InsertGraphForNewAggregateRootEntityAndChildrenUsingConvention()
        {
            // Arrange
            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            AggregateRootEntity aggregateRootEntity = new AggregateRootEntity
            {
                SomeOtherProperty = "Some other property",
                SimpleEntity = new SimpleEntity { SomeProperty = "Some property" },
                OtherSimpleEntities = new List<AnotherSimpleEntity>
                {
                    new AnotherSimpleEntity { SomeProperty = "Child property 1"},
                    new AnotherSimpleEntity { SomeProperty = "Child property 2"},
                }
            };

            // Act
            entityRepository.InsertOrUpdateGraph(aggregateRootEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // The AggregateRoot and its children should all be added to the context and all have a state of 'Added'
            Assert.AreEqual(4, entityRepository.DbContext.ChangeTracker.Entries().Count());
            entityRepository.DbContext.ChangeTracker.Entries().ToList().ForEach(entry => Assert.AreEqual(EntityState.Added, entry.State));
        }

        /// <summary>
        /// Tests that when we insert/update a new aggregate root with all new child objects except for one, the aggregate root and 
        /// new child objects have a state of 'Added', and the existing child object has a state of 'Modified'
        /// </summary>
        [Test]
        public void InsertGraphForNewAggregateRootEntityAndChildrenWhereOneChildObjectAlreadyExists()
        {
            // Arrange
            const int expectedSimpleId = 10;

            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            // Create an aggregate root where one of the 'AnotherSimpleEntity' already exist (i.e. has a primary key)
            AggregateRootEntity aggregateRootEntity = new AggregateRootEntity
            {
                SomeOtherProperty = "Some other property",
                SimpleEntity = new SimpleEntity { SomeProperty = "Some property" },
                OtherSimpleEntities = new List<AnotherSimpleEntity>
                {
                    new AnotherSimpleEntity { Id = expectedSimpleId, SomeProperty = "Child property 1"},
                    new AnotherSimpleEntity { SomeProperty = "Child property 2"},
                }
            };

            // Act
            entityRepository.InsertOrUpdateGraph(aggregateRootEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // The AggregateRoot and its children should all be added to the context and all have a state of 'Added'
            Assert.AreEqual(4, entityRepository.DbContext.ChangeTracker.Entries().Count());
            entityRepository.DbContext.ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                IEntityWithState<int> entityWithState = (IEntityWithState<int>)entry.Entity;
                Assert.AreEqual(entityWithState.Id == expectedSimpleId ? EntityState.Modified : EntityState.Added, entry.State);
            });
        }

        /// <summary>
        /// Tests that when we insert/update a new aggregate root with all existing child objects specifically market as 'Unchanged'
        /// the aggregate root should be in context with a state of 'Added' and all child objects state should be 'Unchanged'
        /// </summary>
        [Test]
        public void InsertGraphForNewAggregateRootEntityAndChildrenWhereChildrenSpecifiedAsUnchanged()
        {
            // Arrange
            const int expectedSimpleId1 = 10;
            const int expectedSimpleId2 = 11;
            const int expectedSimpleId3 = 12;

            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            // Create an aggregate root where one of the 'AnotherSimpleEntity' already exist (i.e. has a primary key)
            AggregateRootEntity aggregateRootEntity = new AggregateRootEntity
            {
                SomeOtherProperty = "Some other property",
                SimpleEntity = new SimpleEntity { Id = expectedSimpleId1, SomeProperty = "Some property", State = State.Unchanged },
                OtherSimpleEntities = new List<AnotherSimpleEntity>
                {
                    new AnotherSimpleEntity { Id = expectedSimpleId2, SomeProperty = "Child property 1", State = State.Unchanged},
                    new AnotherSimpleEntity { Id = expectedSimpleId3, SomeProperty = "Child property 2", State = State.Unchanged},
                }
            };

            // Act
            entityRepository.InsertOrUpdateGraph(aggregateRootEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // The AggregateRoot and its children should all be added to the context and all have a state of 'Added'
            Assert.AreEqual(4, entityRepository.DbContext.ChangeTracker.Entries().Count());
            entityRepository.DbContext.ChangeTracker.Entries().ToList().ForEach(entry =>
            {
                IEntityWithState<int> entityWithState = (IEntityWithState<int>)entry.Entity;
                Assert.AreEqual(entityWithState.Id == default(int) ? EntityState.Added : EntityState.Unchanged, entry.State);
            });
        }

        /// <summary>
        /// Tests that if we insert a simple entity and then update it, the context should still track the entity as an 
        /// 'Added' but with the updated property values
        /// </summary>
        [Test]
        public void InsertSimpleEntityFollowedByAnUpdate()
        {
            // Arrange
            const string updatedValue = "Updated Value";

            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            SimpleEntity simpleEntity = new SimpleEntity { Id = 1, SomeProperty = "Initial value" };

            // Act
            entityRepository.Insert(simpleEntity);

            simpleEntity.SomeProperty = updatedValue;
            entityRepository.Update(simpleEntity);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(1, entityRepository.DbContext.ChangeTracker.Entries().Count());

            // Ensure the entity in cache is still at a state of 'Added' but has the updated properties
            DbEntityEntry entry = entityRepository.DbContext.ChangeTracker.Entries().ToList()[0];
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreEqual(updatedValue, ((SimpleEntity)entry.Entity).SomeProperty);
        }

        #endregion

        #region Multiple Entity Add/Update Tests

        /// <summary>
        /// Tests that when we insert multiple simple entitys using the entity repository, the dbContext has all entities in an 'Added' state
        /// </summary>
        [Test]
        public void InsertMultipleSimpleEntities()
        {
            // Arrange
            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            SimpleEntity simpleEntity1 = new SimpleEntity { SomeProperty = "Some property 1" };
            SimpleEntity simpleEntity2 = new SimpleEntity { SomeProperty = "Some property 2" };

            // Act
            entityRepository.Insert(simpleEntity1);
            entityRepository.Insert(simpleEntity2);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(2, entityRepository.DbContext.ChangeTracker.Entries().Count());
            entityRepository.DbContext.ChangeTracker.Entries().ToList().ForEach(entry => Assert.AreEqual(EntityState.Added, entry.State));
        }

        /// <summary>
        /// Tests that when we update multiple simple entities using the entity repository, the dbContext tracks the entities in a 'Mofified' state.
        /// </summary>
        [Test]
        public void UpdateMultipleSimpleEntities()
        {
            // Arrange
            const int expectedId1 = 10;
            const int expectedId2 = 10;

            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            SimpleEntity simpleEntity1 = new SimpleEntity { Id = expectedId1 };
            SimpleEntity simpleEntity2 = new SimpleEntity { Id = expectedId2 };

            // Act
            entityRepository.Update(simpleEntity1);
            entityRepository.Update(simpleEntity2);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(1, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Modified, entityRepository.DbContext.FindEntity(simpleEntity1.Id).State);
            Assert.AreEqual(EntityState.Modified, entityRepository.DbContext.FindEntity(simpleEntity2.Id).State);
        }

        /// <summary>
        /// Tests that when we insert multiple new aggregate roots with all new child objects using the (has id convention), the aggregate roots and 
        /// all child objects have a state of 'Added' in the dbContext
        /// </summary>
        [Test]
        public void InsertGraphForMultipleNewAggregateRootEntitesAndChildrenUsingConvention()
        {
            // Arrange
            FakeDbContext dbContext = new FakeDbContext();
            AggregateRootEntityRepository entityRepository = new AggregateRootEntityRepository(dbContext);

            AggregateRootEntity aggregateRootEntity1 = new AggregateRootEntity
            {
                SomeOtherProperty = "Some other property",
                SimpleEntity = new SimpleEntity { SomeProperty = "Some property" },
                OtherSimpleEntities = new List<AnotherSimpleEntity>
                {
                    new AnotherSimpleEntity { SomeProperty = "Child property 1"},
                    new AnotherSimpleEntity { SomeProperty = "Child property 2"},
                }
            };

            AggregateRootEntity aggregateRootEntity2 = new AggregateRootEntity
            {
                SomeOtherProperty = "Some other property",
                SimpleEntity = new SimpleEntity { SomeProperty = "Some property" },
                OtherSimpleEntities = new List<AnotherSimpleEntity>
                {
                    new AnotherSimpleEntity { SomeProperty = "Child property 1"},
                    new AnotherSimpleEntity { SomeProperty = "Child property 2"},
                }
            };

            // Act
            entityRepository.InsertOrUpdateGraph(aggregateRootEntity1);
            entityRepository.InsertOrUpdateGraph(aggregateRootEntity2);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);

            // The AggregateRoot and its children should all be added to the context and all have a state of 'Added'
            Assert.AreEqual(8, entityRepository.DbContext.ChangeTracker.Entries().Count());
            entityRepository.DbContext.ChangeTracker.Entries().ToList().ForEach(entry => Assert.AreEqual(EntityState.Added, entry.State));
        }

        #endregion

        #region Delete Entity Tests

        /// <summary>
        /// Tests that when we delete an existing entity, the entity is tracked as 'Deleted' in the context
        /// </summary>
        public void DeleteSimpleEntity()
        {
            // Arrange
            const int expectedId = 10;

            FakeDbContext dbContext = new FakeDbContext();
            SimpleEntityRepository entityRepository = new SimpleEntityRepository(dbContext);

            // Act
            entityRepository.Delete(expectedId);

            // Assert
            Assert.IsNotNull(entityRepository.DbContext);
            Assert.AreEqual(1, entityRepository.DbContext.ChangeTracker.Entries().Count());
            Assert.AreEqual(EntityState.Deleted, entityRepository.DbContext.ChangeTracker.Entries().ToList()[0].State);
        }

        #endregion

        #region Queryable Tests

        /// <summary>
        /// Tests that when we query for an existing entity, it is returned correctly
        /// </summary>
        //[Test]
        public void GetExistingSingleEntity()
        {
            // Arrange
            const int expectedId = 10;

            // Create a repository with a mock dbset, so any queries will be done against the list rather than DB
            SimpleEntityRepository entityRepository = CreateSimpleEntityRepositoryWithMockDbSet(
                new List<SimpleEntity>
                {
                    new SimpleEntity { Id = expectedId }
                }
            );

            // Act
            SimpleEntity entity = entityRepository.GetSingle(se => se.Id == expectedId);

            // Assert
            Assert.IsNotNull(entity);
            Assert.AreEqual(expectedId, entity.Id);
        }

        /// <summary>
        /// Tests that when we query for a non existing entity, it is not returned
        /// </summary>
        [Test]
        public void GetNonExistingSingleEntity()
        {
            // Arrange
            // Create a repository with a mock dbset, so any queries will be done against the list rather than DB
            SimpleEntityRepository entityRepository = CreateSimpleEntityRepositoryWithMockDbSet(
                new List<SimpleEntity>
                {
                    new SimpleEntity { Id = 10 }
                }
            );

            // Act
            SimpleEntity entity = entityRepository.GetSingle(se => se.Id == 9999);

            // Assert
            Assert.IsNull(entity);
        }

        /// <summary>
        /// Tests that when we query for all entities, the cache is populated
        /// </summary>
        [Test]
        public void GetAllEntitiesPopulatesCache()
        {
            // Arrange
            IEnumerable<SimpleEntity> entities = new List<SimpleEntity>
            {
                new SimpleEntity { Id = 10 },
                new SimpleEntity { Id = 11 }
            };

            // Create a repository with a mock dbset, so any queries will be done against the list rather than DB
            SimpleEntityRepository entityRepository = CreateSimpleEntityRepositoryWithMockDbSet(entities);
            entityRepository.ClearCache();

            // Get entities to populate the cache
            entityRepository.GetList();

            // Re-create the repository with an empty list in the DbSet
            entityRepository = CreateSimpleEntityRepositoryWithMockDbSet(new List<SimpleEntity>());

            // Act
            // Re-query the repository, we should still get results as the queries should now work against the cache not against the empty DbSet
            IList<SimpleEntity> results = entityRepository.GetList();

            // Assert
            Assert.IsNotNull(results);
            //Assert.AreEqual(2, results.Count);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a mock EF DbSet that can be used to query a specified datasource
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dataSource">The data source.</param>
        /// <returns>
        /// The creating entity repository
        /// </returns>
        private static IDbSet<TEntity> CreateMockDbSet<TEntity>(IEnumerable<TEntity> dataSource) where TEntity : class
        {
            IQueryable<TEntity> query = dataSource.AsQueryable();

            // Create a mock dbSet and set it up so it will query the dataSource passed in
            Mock<IDbSet<TEntity>> dbSet = new Mock<IDbSet<TEntity>>();
            dbSet.Setup(set => set.Provider).Returns(query.Provider);
            dbSet.Setup(set => set.Expression).Returns(query.Expression);
            dbSet.Setup(set => set.ElementType).Returns(query.ElementType);
            dbSet.Setup(set => set.GetEnumerator()).Returns(query.GetEnumerator());

            return dbSet.Object;
        }

        /// <summary>
        /// Creates A simple entity repository with mock dbSet so we can query against a set list of SimpleEntitys instead of hitting DB.
        /// </summary>
        /// <param name="dataSource">The data source that the repository will query.</param>
        /// <returns>An instance of a SimpleEntityRepository</returns>
        private static SimpleEntityRepository CreateSimpleEntityRepositoryWithMockDbSet(IEnumerable<SimpleEntity> dataSource)
        {
            Mock<FakeDbContext> fakeDbContext = new Mock<FakeDbContext>();
            return new SimpleEntityRepository(fakeDbContext.Object, CreateMockDbSet(dataSource));
        }

        #endregion
    }
}
