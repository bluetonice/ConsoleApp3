using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{

    public class ElasticResponse
    {

        public bool IsValid { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    public class ElasticCreateResponse:ElasticResponse
    {

        public string DocumentId { get; set; }
    }

    public class ElasticSearchingResponse<T> : ElasticResponse where T : class,IElasticDocument
    {
        public IEnumerable<T> Documents { get; set; }
    }
    public class ElasticSearchCrudProvider
    {

        ElasticClient client;
        string defaultIndex;
        public ElasticSearchCrudProvider(string url,string defaultIndex)
        {
            this.defaultIndex = defaultIndex;

            var pool = new StaticConnectionPool(new List<Uri> { new Uri(url) });

            var connectionSettings = new ConnectionSettings(
                pool,
                new HttpConnection())
              .DisableDirectStreaming()
              ;

            client = new ElasticClient(connectionSettings);
        }


        public ElasticResponse DeleteIndex()
        {
            if (client.IndexExists(defaultIndex).Exists)
            {
                var response = client.DeleteIndex(defaultIndex);

                return new ElasticResponse()
                {
                    IsValid = response.IsValid,
                    Message = response.DebugInformation,
                    Exception = response.OriginalException
                };
            }

            return new ElasticResponse()
            {
                IsValid = true
            };
        }

        public ElasticCreateResponse Create<T>(T doc) where T : class,IElasticDocument
        {
            var response = client.Index<T>(doc, i => i
            
            .Index(defaultIndex)
            
            );

            return new ElasticCreateResponse()
            {
                DocumentId = response.Id,
                IsValid = response.IsValid,
                Message = response.DebugInformation,
                Exception = response.OriginalException
            };
        }

      

        public ElasticResponse Update<T,K>(K doc, string documentId) where T:class where K:class

        {
            var response = client.Update<T,K>(documentId, descriptor => descriptor
                                        .Doc(doc)
                                        .Index(defaultIndex)
                                        .Refresh(Refresh.True)
                                        );


            return new ElasticResponse()
            {
                IsValid = response.IsValid,
                Message = response.DebugInformation,
                Exception = response.OriginalException
            };
        }


        public ElasticSearchingResponse<T> SearchByUniqueId<T>(Guid UniqueId) where T : class,IElasticDocument
        {
            var response =  client.Search<T>(s => s.Index(defaultIndex).Query(q => q

            .Bool(bq => bq
            .Filter(fq => fq
            .Term(t => t.Field(f => f.UniqueId).Value(UniqueId)
             )))));

            return new ElasticSearchingResponse<T>()
            {
                IsValid = response.IsValid,
                Message = response.DebugInformation,
                Exception = response.OriginalException,
                Documents = response.Documents
            };
        }

        public ElasticResponse Delete<T>(string documentId) where T : class, IElasticDocument
        {
            var response = client.Delete<T>(documentId, d => d.Index(defaultIndex));


            return new ElasticResponse()
            {
                IsValid = response.IsValid,
                Message = response.DebugInformation,
                Exception = response.OriginalException
            };


        }

      
    }
}
