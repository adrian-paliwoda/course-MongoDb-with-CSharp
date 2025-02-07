// See https://aka.ms/new-console-template for more information

using MongoDB.CRUD;
using MongoDB.Examples;

Console.WriteLine("Start");

// var createExamples = new Create();
var readExamples = new Read();
await readExamples.Read_Example_2("boxData", "exboxoffice");
// var deleteExamples = new Delete();
// var updateExamples = new Update();
//
// // updateExamples.Example_0();
// // Basics.Example_3();
// await Basics.Example_4_Import();


Console.WriteLine("End");