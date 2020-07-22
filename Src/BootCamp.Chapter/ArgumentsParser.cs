﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BootCamp.Chapter.Commands;
using BootCamp.Chapter.Exceptions;

namespace BootCamp.Chapter
{
    public static class ArgumentsParser
    {
        /// <summary>
        /// Attempts to parse the users inputs. Performs checks on the input arguments and returns a bool and the arguments depending on whether they are valid.
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static bool TryParse(string inputString, out string[] arguments)
        {
            arguments = default;
            List<string> args = ObtainArguments(inputString);
            string pathIn = args[0];
            string command = args[1];
            string pathOut = args[2];

            // potentially the help command can be a separate method outside of argument parser which is called within the application controller. Doesn't make too much sense otherwise
            bool[] inputsValid = new bool[3]{ false, false, false};
            inputsValid[0] = PathInFileExists(pathIn) ? true : throw new InputFileNotFoundOrEmptyException($"File at path {pathIn} was not found");
            inputsValid[1] = ValidateCommand(command) ? true : throw new InvalidCommandException($"Command '{command}' does not exist. Type \"help\" for a list of commands");
            inputsValid[2] = PathOutDirectoryExists(pathOut) ? true : throw new OutputPathNotFoundException($"Directory at path {pathOut} was not found");
            
            if (!inputsValid.Any(x => false))
            {
                arguments = new string[3]{ pathIn, command, pathOut};
                return true;
            }
            else
            {
                return false;
            }
        }

        private static List<string> ObtainArguments(string inputString)
        {
            List<char> currArg = new List<char>();
            List<string> args = new List<string>();
            int lenOfCommand = inputString.Length;

            for(int i = 0; i < lenOfCommand; i++)
            {
                // first char
                if (inputString[i] != '"' && currArg.Count == 0 && inputString[i] != ' ')
                {
                    currArg.Add(inputString[i]);
                }
                // Next char
                else if (inputString[i] != '"' && currArg.Count != 0)
                {
                    currArg.Add(inputString[i]);
                }
                // End of a Arg
                else if (inputString[i] == '"' && currArg.Count != 0)
                {
                    args.Add(currArg.CharListToString());
                    currArg.Clear();
                }
                // Skips if blank space between args
                else if (inputString[i] == ' ' && inputString[i-1] != '"' && inputString[i + 1] != '"')
                {
                    continue;
                }
            }

            return args;
        }

        private static string CharListToString(this List<char> input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach(char letter in input)
            {
                stringBuilder.Append(letter);
            }

            return stringBuilder.ToString();      
        }

        private static bool PathInFileExists(string pathIn)
        {
            return File.Exists(pathIn);
        }

        private static bool PathOutDirectoryExists(string pathOut)
        {
            return Directory.Exists(pathOut);
        }

        // ToDo: Implement the validation logic of the input command
        private static bool ValidateCommand(string input)
        {
            // update when commands class and functionality has been created
            string[] splitInput = input.Split(' ');
            Enum.TryParse(splitInput[0], out Command command);

            return command switch
            {
                Command.city => true,
                Command.time => true,
                Command.daily => true,
                Command.full => true,
                _ => false
            };
        }
    }
}
