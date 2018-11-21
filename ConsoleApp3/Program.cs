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
            try
            {
                GenericRepository client = new GenericRepository("healthchecktestlog");

                client.DeleteIndex();

                var now = DateTime.Now;
                HealthCheckTestLog doc = new HealthCheckTestLog();
                doc.TestPropertyDatetime = now;
                doc.TestPropertyDecimal = 12.5M;
                doc.TestPropertyInteger = 8;
                doc.TestPropertyString = "this is a string";

                client.Save(doc);

                Thread.Sleep(2000);

                var createDocument = client.Get<HealthCheckTestLog>(doc.Id);

                if (createDocument.Id != doc.Id
                    || createDocument.TestPropertyDatetime != doc.TestPropertyDatetime
                    || createDocument.TestPropertyDecimal != doc.TestPropertyDecimal
                    || createDocument.TestPropertyInteger != doc.TestPropertyInteger
                    || createDocument.TestPropertyString != doc.TestPropertyString)
                {
                    throw new Exception("Create");
                }


                doc.TestPropertyInteger = 1;

                client.Update(doc);

                Thread.Sleep(2000);

                var updatedDocument = client.Get<HealthCheckTestLog>(doc.Id);


                if (updatedDocument.Id != doc.Id || updatedDocument.TestPropertyInteger != 1)
                {
                    throw new Exception("Update");
                }


                Thread.Sleep(2000);

                BaseSearchModel baseSearchModel = new BaseSearchModel();
                baseSearchModel.From = 0;
                baseSearchModel.Size = 100;
                baseSearchModel.Fields = new Dictionary<string, string>();
                baseSearchModel.Fields.Add("TestPropertyInteger", doc.TestPropertyInteger.ToString());
                IEnumerable<HealthCheckTestLog> searchedDocuments = client.Search<HealthCheckTestLog>(baseSearchModel);

                if (searchedDocuments.Count() != 1)
                {
                    throw new Exception("Search");
                }


                var lastDocument = searchedDocuments.First();


                if (lastDocument.Id != doc.Id || lastDocument.TestPropertyInteger != doc.TestPropertyInteger)
                {
                    throw new Exception("Search");
                }

                client.Delete<HealthCheckTestLog>(doc.Id);


                Thread.Sleep(2000);

                var deletedDocuments = client.SearchById<HealthCheckTestLog>(doc.Id);

                if (deletedDocuments.Count() > 0)
                {
                    throw new Exception("Delete");
                }

            }
            catch (Exception ex)
            {

                //return HealthCheckResult.Unhealthy($"ElasticSearchCheck({name}): Exception during check: {ex.GetType().FullName}");
            }



        }

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
