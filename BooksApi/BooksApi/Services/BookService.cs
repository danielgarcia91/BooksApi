using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService (IConfiguration config)
        {
            //MongoClient reads the server instance for performing DB operations.
            var client = new MongoClient(config.GetConnectionString("BookstoreDb"));
            var database = client.GetDatabase("BookstoreDb");
            _books = database.GetCollection<Book>("Books");
        }


        //GET Method
        public List<Book> Get()
        {
            return _books.Find(book => true).ToList();
        }

        public Book Get(string id)
        {
            var docId = new ObjectId(id);
            return _books.Find<Book>(book => book.Id == docId).FirstOrDefault();
        }

        //Create method
        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        //Update method
        public void Update(string id, Book bookIn)
        {
            var docId = new ObjectId(id);
            _books.ReplaceOne(book => book.Id == docId, bookIn);
        }


        //Remove method
        public void Remove(Book bookIn)
        {
            _books.DeleteOne(book => book.Id == bookIn.Id);
        }

        public void Remove(ObjectId id)
        {
            _books.DeleteOne(book => book.Id == id);
        }
    }
}
