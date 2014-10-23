using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using MhanoHarkness;
using System.Text;
using Microsoft.Framework.ConfigurationModel;

namespace PollR.Model
{
    public class PollRepository
    {
        private const string TableName = "PollSnapshots";

        private Poll currentPoll;
        private string currentSession;
        private CloudTableClient tableClient;

        public PollRepository(CloudStorageAccount storageAccount)
        {
            tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(TableName);
            table.CreateIfNotExists();
            currentPoll = CreatePoll();
        }

        public Poll GetCurrentPoll()
        {
            return currentPoll;
        }

        public void StartSession(String name)
        {
            currentSession = name;
        }

        public void Store(string name)
        {
            DynamicTableEntity tableEntity = new DynamicTableEntity();
            tableEntity.PartitionKey = Base64Url.ToBase64ForUrlString(Encoding.UTF8.GetBytes(currentSession));
            tableEntity.RowKey = Base64Url.ToBase64ForUrlString(Encoding.UTF8.GetBytes(name));
            foreach (var option in currentPoll.Options)
            {
                tableEntity.Properties.Add(
                    Base64Url.ToBase64ForUrlString(Encoding.UTF8.GetBytes(option.Key)),
                    new EntityProperty(option.Value));
            }

            CloudTable table = tableClient.GetTableReference(TableName);
            TableOperation insertOperation = TableOperation.Insert(tableEntity);
            table.Execute(insertOperation);
        }

        public void Reset()
        {
            currentPoll = CreatePoll();
        }

        public string GetCurrentSession()
        {
            return currentSession;
        }

        public IEnumerable<Snapshot> GetSnapshots()
        {
            CloudTable table = tableClient.GetTableReference(TableName);
            TableQuery<DynamicTableEntity> query =
                new TableQuery<DynamicTableEntity>()
                .Where(TableQuery
                .GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                    Base64Url.ToBase64ForUrlString(Encoding.UTF8.GetBytes(currentSession))));

            foreach (var entity in table.ExecuteQuery(query))
            {
                Snapshot snapshot = new Snapshot(
                    entity.Timestamp,
                    Encoding.UTF8.GetString(Base64Url.FromBase64ForUrlString(entity.RowKey)));
                foreach (var property in entity.Properties)
                {
                    var voteCount = property.Value.Int32Value;
                    if (voteCount.HasValue)
                    {
                        snapshot.Votes.Add(
                            Encoding.UTF8.GetString(Base64Url.FromBase64ForUrlString(property.Key)),
                            property.Value.Int32Value.Value);
                    }                    
                }
                yield return snapshot;
            }
        }

        private static Poll CreatePoll()
        {
            Poll newPoll = new Poll();
            newPoll.AddOption("YES");
            newPoll.AddOption("NO");
            return newPoll;
        }
    }
}