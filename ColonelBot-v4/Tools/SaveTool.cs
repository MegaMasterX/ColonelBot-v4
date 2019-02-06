//Assistance and code provided by GreigaMaster and used with permission.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using ColonelBot_v4.Tools.BN6.BN6SaveData;

//This tool is for accepting user's Save Files if an event is active and generating a text-file entry for that user's Folder 1, Folder 2, and NaviCust setups.
//  Theroetically, the user just has to upload their save to Discord - ColonelBot will cache the save and delete the Discord message, process it, and release the thread.


namespace ColonelBot_v4.Tools.BN6
{
    public static class SaveTool
    {
        struct ChipEntry
        {
            public ChipEntry(ushort value)
            {
                _value = value;
            }
            public ushort _value;
            public ushort ID
            {
                get { return (ushort)(_value & 0x1FF); }
            }
            public ushort Code
            {
                get { return (ushort)(_value >> 9); }
            }


        }

        public static bool ProcessSave(string FileLocation, ulong SubmitterUserID, string NetbattlerName)
        {
            //This method returns True if the save file is accepted and applied, False if there is an error or it's not accepted

            //This method is also asynchronously called when ColonelBot detects a file that's uploaded with the extention SA1.
            //  That portion of the code should download the file and place it in Cache so this method is just dedicated to pulling the proper data out.

            //1. Open the file FilePath.
            BN6SaveData.BN6SaveData bn6Save = new BN6SaveData.BN6SaveData(FileLocation);
            if (bn6Save.ValidSave)
            {//Is the save validated?
                //Set up the offsets to extract the folder data
                int baseFolderOffset = bn6Save.GetSectionOffset(SaveSection.SAVESECTION_FOLDER);
                int naviStatsOffset = bn6Save.GetSectionOffset(SaveSection.SAVESECTION_STATS);
                int naviCustProgsOffset = bn6Save.GetSectionOffset(SaveSection.SAVESECTION_NAVICUST_PROGS);

                int folder1Offset = baseFolderOffset + (0 * 30 * 2);
                int folder2Offset = baseFolderOffset + (1 * 30 * 2);
                int folder1SelectedChip = bn6Save.Data[naviStatsOffset + 0x2E];
                int folder2SelectedChip = bn6Save.Data[naviStatsOffset + 0x2F];
                int folder1Tag1 = bn6Save.Data[naviStatsOffset + 0x56];
                int folder1Tag2 = bn6Save.Data[naviStatsOffset + 0x57];
                int folder2Tag1 = bn6Save.Data[naviStatsOffset + 0x58];
                int folder2Tag2 = bn6Save.Data[naviStatsOffset + 0x59];

                //Set up the StringBuilder to extract the data. 
                StringBuilder outString = new StringBuilder();
                
                outString.AppendLine($"Folder1:");
                for (int i = 0; i < 30; i++)
                {
                    ChipEntry ce = new ChipEntry(BitConverter.ToUInt16(bn6Save.Data, folder1Offset + i * 2));
                    string tag = "";
                    if (i == folder1SelectedChip)
                        tag = "[REG]";
                    if (i == folder1Tag1)
                        tag = "[TAG1]";
                    if (i == folder1Tag2)
                        tag = "[TAG2]";
                    outString.AppendLine($"{tag} {Globals.chipNames[ce.ID]} {Globals.chipCodes[ce.Code]}");
                }

                outString.AppendLine($"\r\nFolder2:");
                for (int i = 0; i < 30; i++)
                {
                    ChipEntry ce = new ChipEntry(BitConverter.ToUInt16(bn6Save.Data, folder2Offset + i * 2));
                    string tag = "";
                    if (i == folder2SelectedChip)
                        tag = "[REG]";
                    if (i == folder2Tag1)
                        tag = "[TAG1]";
                    if (i == folder2Tag2)
                        tag = "[TAG1]";
                    outString.AppendLine($"{tag} {Globals.chipNames[ce.ID]} {Globals.chipCodes[ce.Code]}");
                }

                outString.AppendLine($"\r\nNaviCust:");
                for (int i = 0; i < 30; i++)
                {
                    ushort ncpID = (ushort)(BitConverter.ToUInt16(bn6Save.Data, naviCustProgsOffset + i * 8) >> 2);
                    if (ncpID != 0)
                        outString.AppendLine($"{Globals.naviCustProgNames[ncpID]}");
                }

                //Check to see if the user has a folder in Setups.

                //If so, write the Setup.txt file - overwriting any previous file that may exist. 
                System.IO.File.WriteAllText($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}Setups{Path.DirectorySeparatorChar}{NetbattlerName}({SubmitterUserID.ToString()}).txt", outString.ToString());
                return true; //We're done - setup accepted.
            }
            return false; //There was an error somewhere and we didn't reach the end of the extraction.
        }

    }
}