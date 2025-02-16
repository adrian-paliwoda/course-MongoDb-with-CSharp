// See https://aka.ms/new-console-template for more information

using MongoDB.CRUD;

Console.WriteLine("Start");


var update = new Update();
await update.Example_6();

Console.WriteLine("End");