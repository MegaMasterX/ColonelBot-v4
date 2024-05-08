
using Newtonsoft.Json;
using System;
using System.IO;

using System.Collections.Generic;
using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;

public class ChipLibrary
{
    public List<Chip> Library = new List<Chip>();


    private static ChipLibrary instance;
    public static ChipLibrary Instance {
        get { 
            if (instance == null) {
                Console.WriteLine("Instance was null");
                instance = new ChipLibrary();
                
            }
            
            return instance;
        }
    }


    public ChipLibrary()
    {
        if (Library.Count == 0)
        {
            //The library isn't initialized, lol.
            Console.WriteLine("Chip library initialized via constructor.");
            InitializeLibrary();
        }
    }

    private void InitializeLibrary()
    {
        Library = JsonConvert.DeserializeObject<List<Chip>>(File.ReadAllText(BotTools.GetSettingString(BotTools.ConfigurationEntries.ChipLibraryFileLocation)));
        Console.WriteLine("Chip Library Initialized.");
    }

}