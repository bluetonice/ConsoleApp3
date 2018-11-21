using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            GenericRepository genericRepository = new GenericRepository("healthchecktestlog");
        }

        //private static void Old()
        //{
        //    try
        //    {

        //        string defaultIndex = "healthchecktestlog";
        //        string url = "http://localhost:9200";

        //        ElasticSearchCrudProvider client = new ElasticSearchCrudProvider(url, defaultIndex);

        //        var deleteIndexResponse = client.DeleteIndex();

        //        Thread.Sleep(2000);

        //        var now = DateTime.Now;
        //        HealthCheckTestLog doc = new HealthCheckTestLog();
        //        doc.TestPropertyDatetime = now;
        //        doc.TestPropertyDecimal = 12.5M;
        //        doc.TestPropertyInteger = 8;
        //        doc.TestPropertyString = "this is a string";

        //        var createResponse = client.Create<HealthCheckTestLog>(doc);

        //        if (!createResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {response.DebugInformation}");
        //        }

        //        Thread.Sleep(2000);

        //        var createSearchResponse = client.SearchByUniqueId<HealthCheckTestLog>(doc.UniqueId);

        //        if (!createSearchResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {response.DebugInformation}");
        //        }

        //        if (createSearchResponse.Documents.Count() != 1)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Unhealthy : Searching");
        //        }


        //        var createDocument = createSearchResponse.Documents.First();

        //        if (createDocument.UniqueId != doc.UniqueId
        //        || createDocument.TestPropertyDatetime != doc.TestPropertyDatetime
        //        || createDocument.TestPropertyDecimal != doc.TestPropertyDecimal
        //        || createDocument.TestPropertyInteger != doc.TestPropertyInteger
        //        || createDocument.TestPropertyString != doc.TestPropertyString)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Unhealthy : Mapping");
        //        }

        //        Thread.Sleep(2000);


        //        var updateResponse = client.Update<HealthCheckTestLog, object>(new { TestPropertyInteger = 6 }, createResponse.DocumentId);

        //        if (!updateResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {updateResponse.DebugInformation}");
        //        }



        //        var updateSearchResponse = client.SearchByUniqueId<HealthCheckTestLog>(doc.UniqueId);


        //        if (!updateSearchResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {updateSearchResponse.DebugInformation}");
        //        }

        //        if (updateSearchResponse.Documents.Count() != 1)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Unhealthy : Searching After Update");
        //        }

        //        var updateDocument = updateSearchResponse.Documents.First();

        //        if (updateDocument.UniqueId != doc.UniqueId || updateDocument.TestPropertyInteger != 6)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Unhealthy : Update");
        //        }

        //        if (!updateResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {updateResponse.DebugInformation}");
        //        }


        //        var deleteResponse = client.Delete<HealthCheckTestLog>(createResponse.DocumentId);


        //        if (!deleteResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {updateResponse.DebugInformation}");
        //        }


        //        Thread.Sleep(2000);


        //        var deleteSearchResponse = client.SearchByUniqueId<HealthCheckTestLog>(doc.UniqueId);


        //        if (!deleteSearchResponse.IsValid)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}) : {updateSearchResponse.DebugInformation}");
        //        }

        //        if (deleteSearchResponse.Documents.Count() != 0)
        //        {
        //            //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Unhealthy : Searching After Delete");
        //        }

        //        //return HealthCheckResult.Healthy($"ElasticSearchCheck({name}): Healthy");


        //    }
        //    catch (Exception ex)
        //    {
        //        //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Exception during check: {ex.GetType().FullName}");
        //    }
        //}

    }

    public interface IElasticDocument
    {

        [Keyword]

        Guid UniqueId { get; set; }
    }
    public class HealthCheckTestLog : BaseEntity
    {

        public HealthCheckTestLog()
        {
        }
        public string TestPropertyString { get; set; }

        public decimal TestPropertyDecimal { get; set; }

        public int TestPropertyInteger { get; set; }

        public DateTime TestPropertyDatetime { get; set; }

    }
}
