﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.SQS;
using JukeboxAlexa.Library;
using JukeboxAlexa.Library.Model;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace JukeboxAlexa.PlaySongTitleArtistRequest {
    public class Function : ICommonDependencyProvider, IDynamodbDependencyProvider  {
        
        //--- Fields ---
        private readonly PlaySongTitleArtistRequest _playSongArtistRequest;
        private readonly JukeboxDynamoDb _jukeboxDynamoDb;

        //--- Constructors ---
        public Function() {
            var queueName = Environment.GetEnvironmentVariable("STACK_SQSSONGQUEUE");
            var tableName = Environment.GetEnvironmentVariable("STACK_DYNAMODBSONGS");
            var indexNameSearchTitle = Environment.GetEnvironmentVariable("INDEX_NAME_SEARCH_TITLE");
            var indexNameSearchTitleArtist = Environment.GetEnvironmentVariable("INDEX_NAME_SEARCH_TITLE_ARTIST");
            var indexTableName = Environment.GetEnvironmentVariable("STACK_DYNAMODBTITLEWORDCACHE");
            _jukeboxDynamoDb = new JukeboxDynamoDb(new AmazonDynamoDBClient(), tableName, indexNameSearchTitle, indexNameSearchTitleArtist, indexTableName);
            _playSongArtistRequest = new PlaySongTitleArtistRequest(this, new AmazonSQSClient(), queueName, this);
        }

        //--- FunctionHandler ---
        public async Task<APIGatewayProxyResponse> FunctionHandlerAsync(APIGatewayProxyRequest inputRequest, ILambdaContext context) {
            LambdaLogger.Log($"*** INFO: API Request input from user: {JsonConvert.SerializeObject(inputRequest)}");
            var body = inputRequest.Body;
            LambdaLogger.Log($"*** INFO: API Request body from user: {body}");
            var input = JsonConvert.DeserializeObject<CustomSkillRequest>(body);
            LambdaLogger.Log($"*** INFO: Request input from user: {JsonConvert.SerializeObject(input)}");
    
            // process request
            var requestResult = await _playSongArtistRequest.HandleRequest(input);
            var response = new APIGatewayProxyResponse {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(requestResult),
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "application/json" }
                }
            };
            return response;
        }
        
        string ICommonDependencyProvider.DateNow() => new DateTime().ToUniversalTime().ToString("yy-MM-ddHH:mm:ss");
        Task<IEnumerable<SongModel.Song>> IDynamodbDependencyProvider.DynamoDbFindSongsByTitleArtistAsync(string title, string artist) => _jukeboxDynamoDb.FindSongsByTitleArtistAsync(title, artist);
    }
}
