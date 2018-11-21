using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ElasticClient _elasticClient;
        private readonly string _indexName;

        public GenericRepository(string indexName)
        {
            _elasticClient = ElasticSearchClientHelper.CreateElasticClient();
            _indexName = indexName;
        }

        public IEnumerable<T> All<T>() where T :class
        {
            return _elasticClient.Search<T>(search =>
                search.MatchAll().Index(_indexName)).Documents;
        }

        public T Get<T>(Guid id) where T : class
        {
            var result = _elasticClient.Get<T>(id.ToString(), idx => idx.Index(_indexName));

            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return result.Source;
        }

        public bool Delete<T>(Guid id) where T : class
        {
            var result = _elasticClient.Delete<T>(id.ToString(), idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return result.IsValid;
        }

        public Guid Save<T>(T entity) where T : class,IBaseEntity
        {
            ElasticSearchClientHelper.CheckIndex<T>(_elasticClient, _indexName);

            entity.Id = Guid.NewGuid();
            var result = _elasticClient.Index(entity, idx => idx.Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
            return entity.Id;
        }

        public void Update<T>(T entity) where T : class
        {
            var result = _elasticClient.Update(
                    new DocumentPath<T>(entity), u =>
                        u.Doc(entity).Index(_indexName));
            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }
        }

        public IEnumerable<T> Search<T>(BaseSearchModel request) where T : class
        {
            var dynamicQuery = new List<QueryContainer>();
            foreach (var item in request.Fields)
            {
                dynamicQuery.Add(Query<T>.Match(m => m.Field(new Field(item.Key.ToLower())).Query(item.Value)));
            }

            var result = _elasticClient.Search<T>(s => s
                                       .From(request.From)
                                       .Size(request.Size)
                                       .Index(_indexName)
                                        .Query(q => q.Bool(b => b.Must(dynamicQuery.ToArray()))));

            if (!result.IsValid)
            {
                throw new Exception(result.OriginalException.Message);
            }

            return result.Documents;
        }

        public void DeleteIndex() 
        {
            if (_elasticClient.IndexExists(_indexName).Exists)
            {
                var result = _elasticClient.DeleteIndex(_indexName);

                if (!result.IsValid)
                {
                    throw new Exception(result.OriginalException.Message);
                }
            }
        }

        public  void SearchByUniqueId<T>(Guid UniqueId) where T : class, IBaseEntity
        {
            var response = this._elasticClient.Search<T>(s => s.Index(_indexName).Query(q => q

           .Bool(bq => bq
           .Filter(fq => fq
           .Term(t => t.Field(f => f.Id).Value(UniqueId)
            )))));

         
        }

        private void CheckIndex<T>() where T : class
        {
            var response = this._elasticClient.IndexExists(this._indexName);

            if (!response.Exists)
            {
                this._elasticClient.CreateIndex(this._indexName, index =>
                   index.Mappings(ms =>
                       ms.Map<T>(x => x.AutoMap())));
            }
        }

    }
}
