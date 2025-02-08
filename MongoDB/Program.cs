// See https://aka.ms/new-console-template for more information

using MongoDB.CRUD;
using MongoDB.Examples;

Console.WriteLine("Start");

var read = new Read();
await read.Read_Example_3("movieData", "movies");
// await read.Read_Example_4("user", "users");
// await read.Read_Example_4("movieData", "movies");


Console.WriteLine("End");