
using Newtonsoft.Json;
using System;
using System.IO;

using System.Collections.Generic;
using ColonelBot_v4.Models;
using ColonelBot_v4.Tools;

public class PatchCardLibrary
{
    public List<PatchCard> Library = new List<PatchCard>();

    private static PatchCardLibrary instance;
    public static PatchCardLibrary Instance {
        get { 
            if (instance == null) {
                instance = new PatchCardLibrary();   
            }
            return instance;
        }
    }
    

    public PatchCardLibrary()
    {
        if (Library.Count == 0)
        {
            //The patch card library isn't initialized.
            InitializeLibrary();
        }
    }

    private void InitializeLibrary()
    {
        Library = JsonConvert.DeserializeObject<List<PatchCard>>(File.ReadAllText(BotTools.GetSettingString(BotTools.ConfigurationEntries.ModcardLibraryFileLocation)));
        Console.WriteLine("Patch Card Library initialized.");        
    }
}