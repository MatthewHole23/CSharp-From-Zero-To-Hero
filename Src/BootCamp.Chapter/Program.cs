﻿using System;
using System.IO;

namespace BootCamp.Chapter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Start");
            CredentialsManager credentials_manager = new CredentialsManager(@$"C:\Users\matth\source\repos\CSharp-From-Zero-To-Hero\Src\BootCamp.Chapter\Credentials\Credentials.txt");
            CredentialsCollector initial_credentials = new CredentialsCollector("Matthew2", "Thisisverysafe");
            credentials_manager.Register(initial_credentials.Credentials);
            Console.WriteLine($"{initial_credentials.Credentials.Username} {initial_credentials.Credentials.Password}");
            Console.WriteLine("Program End");
            Console.ReadLine();
        }
    }
}
