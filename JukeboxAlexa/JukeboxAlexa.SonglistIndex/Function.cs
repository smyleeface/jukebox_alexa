﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using JukeboxAlexa.Library;
using JukeboxAlexa.Library.Model;
using Newtonsoft.Json;
using Document = Amazon.DynamoDBv2.DocumentModel.Document;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace JukeboxAlexa.SonglistIndex {
    public class Function : IDynamodbDependencyProvider  {
        
        //--- Fields ---
        private string _queueName;
        private readonly SonglistIndex _songlistUpload;
        private readonly JukeboxDynamoDb _jukeboxDynamoDb;
        private readonly JukeboxS3 _jukeboxS3;
        
        
        //--- Constructors ---
        public Function() {
            _queueName = Environment.GetEnvironmentVariable("STACK_SQSSONGQUEUE");
            var tableName = Environment.GetEnvironmentVariable("STACK_DYNAMODBSONGS");
            var indexNameSearchTitle = Environment.GetEnvironmentVariable("INDEX_NAME_SEARCH_TITLE");
            var indexNameSearchTitleArtist = Environment.GetEnvironmentVariable("INDEX_NAME_SEARCH_TITLE_ARTIST");
            var indexTableName = Environment.GetEnvironmentVariable("STACK_DYNAMODBTITLEWORDCACHE");
            _jukeboxDynamoDb = new JukeboxDynamoDb(new AmazonDynamoDBClient(), tableName, indexNameSearchTitle, indexNameSearchTitleArtist, indexTableName);
            _songlistUpload = new SonglistIndex(this);
        }

        //--- FunctionHandler ---
        public async Task FunctionHandlerAsync(DynamoDBEvent dynamoDbEvent, ILambdaContext context) {
            LambdaLogger.Log($"*** INFO: Event: {JsonConvert.SerializeObject(dynamoDbEvent)}");
            
            // process request
            try {
                await _songlistUpload.HandleRequest(dynamoDbEvent.Records.FirstOrDefault());
            }
            catch (Exception e) {
                LambdaLogger.Log($"Exception occured: {e}");
            }
        }
        
        Task<GetItemResponse> IDynamodbDependencyProvider.DynamodbGetItemAsync(IDictionary<string, AttributeValue> key) => _jukeboxDynamoDb.GetItemAsync(key);
        Task<UpdateItemResponse> IDynamodbDependencyProvider.DynamodbUpdateItemAsync(Dictionary<string, AttributeValue> key, string updateExpression, IDictionary<string, AttributeValue> expressionAttributeValues) => _jukeboxDynamoDb.UpdateItemAsync(key, updateExpression, expressionAttributeValues);
        Task<PutItemResponse> IDynamodbDependencyProvider.DynamodbPutItemAsync(IDictionary<string, AttributeValue> expressionAttributeValues) => _jukeboxDynamoDb.PutItemAsync(expressionAttributeValues);
        Task IDynamodbDependencyProvider.DynamodbDeleteWordInDbAsync(IDictionary<string, AttributeValue> key) => _jukeboxDynamoDb.DeleteItemAsync(key);
    }
}