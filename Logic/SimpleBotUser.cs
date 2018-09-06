using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleBot
{
    public class SimpleBotUser
    {
        public static string Reply(Message message)
        {

            //var client = new MongoClient("mongodb://localhost:27017");

            //var doc = new BsonDocument()
            //{
            //    { "id", message.Id},
            //    { "texto", message.Text },
            //    { "app", "teste" },
            //    { "campo", "ok"},
            //    { "campo2", 2 },
            //    { "campo3", new BsonDocument { { "campo2", 2 } } }
            //};

            //var db = client.GetDatabase("db01");
            //var col = db.GetCollection<BsonDocument>("tabela01");
            //col.InsertOne(doc);

            var proFile = GetProfile(message.Id);

            proFile.Visitas += 1;

            SetProfile(message.Id, proFile);
            
            return $"{message.User} disse '{message.Text}'";
        }

        public static UserProfile GetProfile(string id)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("db01");
            var col = db.GetCollection<UserProfile>("profile");

            var filtro = Builders<UserProfile>.Filter.Eq("id", id);

            return col.Find(filtro).First() ?? new UserProfile { Id = id , Visitas = 0 };
        }

        public static void SetProfile(string id, UserProfile profile)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("db01");
            var col = db.GetCollection<UserProfile>("profile");

            var filtro = Builders<UserProfile>.Filter.Eq("id", id);

            col.ReplaceOne(filtro, profile);
        }
    }
}