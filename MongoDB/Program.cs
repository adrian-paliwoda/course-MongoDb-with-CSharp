// See https://aka.ms/new-console-template for more information

using MongoDB.CRUD;

Console.WriteLine("Start");

var exercises = new ReadWithAggregations();
await exercises.Example_0();

Console.WriteLine("End");