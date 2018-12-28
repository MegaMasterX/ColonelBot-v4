//Provided in whole by GreigaMaster - used and implemented with permission.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ColonelBot_v4.Tools.BN6
{
    namespace BN6SaveData
    {
        enum SaveSection
        {
            SAVESECTION_MAP = 0,//0x3C
            SAVESECTION_PLAY,//0x40
            SAVESECTION_FLAG,//0x44
            SAVESECTION_FOLDER,//0x48
            SAVESECTION_PACK,//0x4C
            SAVESECTION_KEYITEM,//0x50
            SAVESECTION_SHOP,//0x54
            SAVESECTION_NAVICUST_TILES,//0x58
            SAVESECTION_NAVICUST_PROGS,//0x5C
            SAVESECTION_9,//0x60
            SAVESECTION_10,//0x64
            SAVESECTION_11,//0x68
            SAVESECTION_12,//0x6C
            SAVESECTION_MYSTERYDATA,//0x70
            SAVESECTION_STATS,//0x74
            SAVESECTION_KEYITEM_ANTICHEAT,//0x78
            SAVESECTION_CHIP_ANTICHEAT,//0x7C
            SAVESECTION_UNK0_ANTICHEAT,//0x80
            SAVESECTION_ZENNY_ANTICHEAT,//0x84
            SAVESECTION_BUGFRAG_ANTICHEAT,//0x88
            SAVESECTION_UNK1_ANTICHEAT//0x8C
        };

        enum BN6GameRegion
        {
            GAMEREGION_JP = 0,
            GAMEREGION_US = 1,
            GAMEREGION_EU = 2,
            GAMEREGION_UNK = 3,
        };

        enum BN6GameVersion
        {
            GAMEVERSION_GREGAR = 0,
            GAMEVERSION_FALZER = 1,
            GAMEVERSION_UNK = 2,
        };

        class BN6SaveData
        {
            //Used for memory shuffling always 0 in normal saves
            private const int sectionShiftValueOffset = 0x1060;
            //Offset of the first section
            private const int globalSectionBaseOffset = 0x1B80;
            //Number of bytes in the save file that dont map to RAM
            private const int saveOffset = 0x100;
            private const int saveRegionSize = 0x6710;
            // F or G from game name string
            //and J, U, or P from region
            //offsets should be the same between version/regions
            private const int versionLetterOffset = 0x1C76;
            private const int buildNumberOffset = 0x1C78;
            private const int regionLetterOffset = 0x1C82;
            //Offset of decryption key in the save
            private const int decryptionKeyOffset = 0x1064;
            private static readonly int[] sectionOffsetsJP = {
        0x00000000,0x00000084,0x00000108,
        0x000005F8,0x000006B0,0x000015B4,
        0x00001748,0x0000258C,0x000025D0,
        0x0000275C,0x00002770,0x00002774,
        0x00002778,0x00002788,0x00002C0C,
        0x00002ECC,0x00003060,0x00003264,
        0x00003468,0x00003470,0x00003478 };

            private static readonly int[] sectionOffsetsUS = {
        0x00000000,0x00000084,0x00000108,
        0x000005F8,0x000006B0,0x000015B4,
        0x00001748,0x000025CC,0x00002610,
        0x0000279C,0x000027B0,0x000027B4,
        0x000027B8,0x000027C8,0x00002C4C,
        0x00002F0C,0x000030A0,0x000032A4,
        0x000034A8,0x000034B0,0x000034B8 };

            private static readonly int[] sectionOffsetsPL = {
        0x00000000,0x00000084,0x00000108,
        0x000005F8,0x000006B0,0x000015B4,
        0x00001748,0x000025CC,0x00002610,
        0x0000279C,0x000027B0,0x000027B4,
        0x000027B8,0x000027C8,0x00002C4C,
        0x00002F0C,0x000030A0,0x000032A4,
        0x000034A8,0x000034B0,0x000034B8 };

            private static readonly int[] sectionSizesJP = {
        0x00000084,0x00000084,0x000004F0,
        0x000000B8,0x00000F04,0x00000194,
        0x00000E44,0x00000044,0x0000018C,
        0x00000014,0x00000004,0x00000004,
        0x00000010,0x00000484,0x000002C0,
        0x00000194,0x00000204,0x00000204,
        0x00000008,0x00000008,0x00000104,
        };

            private static readonly int[] sectionSizesUS = {
        0x00000084,0x00000084,0x000004F0,
        0x000000B8,0x00000F04,0x00000194,
        0x00000E84,0x00000044,0x0000018C,
        0x00000014,0x00000004,0x00000004,
        0x00000010,0x00000484,0x000002C0,
        0x00000194,0x00000204,0x00000204,
        0x00000008,0x00000008,0x00000104,
        };

            private static readonly int[] sectionSizesPL = {
        0x00000084,0x00000084,0x000004F0,
        0x000000B8,0x00000F04,0x00000194,
        0x00000E84,0x00000044,0x0000018C,
        0x00000014,0x00000004,0x00000004,
        0x00000010,0x00000484,0x000002C0,
        0x00000194,0x00000204,0x00000204,
        0x00000008,0x00000008,0x00000104,
        };

            private static readonly int[][] sectionOffsets = {
        sectionOffsetsJP,
        sectionOffsetsUS,
        sectionOffsetsPL
    };
            private static readonly int[][] sectionSizes = {
        sectionSizesJP,
        sectionSizesUS,
        sectionSizesPL
    };
            private readonly int[] sectionCombinedSize = { 0x357C, 0x35BC, 0x35BC };

            private bool _validSave;
            private int[] _saveSectionTable;
            private byte[] _saveData;
            BN6GameRegion _region;
            BN6GameVersion _version;

            public bool ValidSave
            {
                get
                {
                    return _validSave;
                }
            }

            public byte[] Data
            {
                get
                {
                    return _saveData;
                }
            }

            public BN6GameVersion Version
            {
                get
                {
                    return _version;
                }
            }

            public BN6GameRegion Region
            {
                get
                {
                    return _region;
                }
                set
                {
                    BN6GameRegion oldregion = _region;
                    _region = value;
                    //Save current save sections
                    byte[][] currentSections = new byte[21][];
                    for (int i = 0; i < 21; i++)
                    {
                        int size = sectionSizes[(int)oldregion][i];
                        currentSections[i] = new byte[size];
                        Buffer.BlockCopy(_saveData, GetSectionOffset((SaveSection)i), currentSections[i], 0, size);
                    }
                    //Clear old section data
                    Array.Clear(_saveData, globalSectionBaseOffset, sectionCombinedSize[(int)oldregion]);
                    //Init Section Table for new region
                    InitSectionTable();
                    for (int i = 0; i < 21; i++)
                    {
                        int oldSize = sectionSizes[(int)oldregion][i];
                        int newSize = sectionSizes[(int)_region][i];
                        //This truncates the size of Shop section when converting US to JP
                        int copysize = Math.Min(oldSize, newSize);
                        Buffer.BlockCopy(currentSections[i], sectionSizes[(int)_region][i], _saveData, GetSectionOffset((SaveSection)i), copysize);
                    }

                    string buildString = "";
                    switch (_region)
                    {
                        case BN6GameRegion.GAMEREGION_JP:
                            buildString = "20050924a JP";
                            break;
                        case BN6GameRegion.GAMEREGION_US:
                            buildString = "20060110a US";
                            break;
                        case BN6GameRegion.GAMEREGION_EU:
                            buildString = "20060110a PL";
                            break;
                        case BN6GameRegion.GAMEREGION_UNK:
                            break;
                        default:
                            break;
                    }
                    for (int i = 0; i < buildString.Length; i++)
                    {
                        _saveData[buildNumberOffset + i] = (byte)(buildString[i]);
                    }


                    RecalculateChecksum();
                }
            }

            public BN6SaveData(string fileName)
            {
                _validSave = true;
                if (!File.Exists(fileName))
                {
                    _validSave = false;
                    return;
                }
                _saveData = new byte[saveRegionSize];
                _saveSectionTable = new int[21];
                FileStream fs = File.OpenRead(fileName);
                fs.Position = saveOffset;
                int bytesRead = fs.Read(_saveData, 0, saveRegionSize);
                fs.Close();
                if (bytesRead == saveRegionSize)
                {
                    DecryptSave();
                    DetectVersionRegion();
                    if ((_region != BN6GameRegion.GAMEREGION_UNK) && (_version != BN6GameVersion.GAMEVERSION_UNK))
                    {
                        InitSectionTable();
                        _validSave = VerifyChecksum();
                    }
                    else
                    {
                        _validSave = false;
                        return;
                    }
                }
                else
                {
                    _validSave = false;
                    return;
                }

            }

            void DecryptSave()
            {
                uint decryptionKey = BitConverter.ToUInt32(_saveData, decryptionKeyOffset);
                for (int i = 0; i < saveRegionSize; i++)
                {
                    _saveData[i] = (byte)((_saveData[i] ^ decryptionKey) & 0xFF);
                }
                //Write back decryption key for checksum calculation
                Buffer.BlockCopy(BitConverter.GetBytes(decryptionKey), 0, _saveData, decryptionKeyOffset, 4);
            }


            public byte[] GetEncrypedtSave()
            {
                byte[] encryptedSave = new byte[saveRegionSize];
                Buffer.BlockCopy(_saveData, 0, encryptedSave, 0, saveRegionSize);
                uint decryptionKey = BitConverter.ToUInt32(encryptedSave, decryptionKeyOffset);
                for (int i = 0; i < saveRegionSize; i++)
                {
                    encryptedSave[i] = (byte)((encryptedSave[i] ^ decryptionKey) & 0xFF);
                }
                Buffer.BlockCopy(BitConverter.GetBytes(decryptionKey), 0, encryptedSave, decryptionKeyOffset, 4);
                return encryptedSave;

            }

            void DetectVersionRegion()
            {
                char versionLetter = (char)_saveData[versionLetterOffset];
                switch (versionLetter)
                {
                    case 'F':
                        _version = BN6GameVersion.GAMEVERSION_FALZER;
                        break;
                    case 'G':
                        _version = BN6GameVersion.GAMEVERSION_GREGAR;
                        break;
                    default:
                        _version = BN6GameVersion.GAMEVERSION_UNK;
                        break;
                }
                char regionLetter = (char)_saveData[regionLetterOffset];
                switch (regionLetter)
                {
                    case 'J':
                        _region = BN6GameRegion.GAMEREGION_JP;
                        break;
                    case 'U':
                        _region = BN6GameRegion.GAMEREGION_US;
                        break;
                    case 'P':
                        _region = BN6GameRegion.GAMEREGION_EU;
                        break;
                    default:
                        _region = BN6GameRegion.GAMEREGION_UNK;
                        break;
                }
            }

            void InitSectionTable()
            {
                //In game this value is zeroed before use so not used
                //uint32_t sectionShiftValue = *(uint32_t*)(&_saveData[sectionShiftValueOffset]);
                //sectionShiftValue &= 0x00;
                for (int i = 0; i < 21; i++)
                {
                    _saveSectionTable[i] = globalSectionBaseOffset + sectionOffsets[(int)_region][i];
                }
            }

            public int GetSectionOffset(SaveSection section)
            {
                return _saveSectionTable[(int)section];
            }

            bool VerifyChecksum()
            {
                int saveChecksumOffset = GetSectionOffset(SaveSection.SAVESECTION_PLAY) + 0x68;
                uint saveChecksum = BitConverter.ToUInt32(_saveData, saveChecksumOffset);
                //Clear checksum for calculation
                Array.Clear(_saveData, saveChecksumOffset, 4);
                uint currentChecksum = 0;
                for (int i = saveRegionSize - 1; i >= 0; i--)
                {
                    currentChecksum += _saveData[i];
                }
                switch (_version)
                {
                    case BN6GameVersion.GAMEVERSION_GREGAR:
                        currentChecksum += 0x72;
                        break;
                    case BN6GameVersion.GAMEVERSION_FALZER:
                        currentChecksum += 0x18;
                        break;
                }
                //restore checksum
                Buffer.BlockCopy(BitConverter.GetBytes(saveChecksum), 0, _saveData, saveChecksumOffset, 4);
                return saveChecksum == currentChecksum;
            }

            public void RecalculateChecksum()
            {
                int saveChecksumOffset = GetSectionOffset(SaveSection.SAVESECTION_PLAY) + 0x68;
                //Clear checksum for calculation
                Array.Clear(_saveData, saveChecksumOffset, 4);
                uint currentChecksum = 0;
                for (int i = saveRegionSize - 1; i >= 0; i--)
                {
                    currentChecksum += _saveData[i];
                }
                switch (_version)
                {
                    case BN6GameVersion.GAMEVERSION_GREGAR:
                        currentChecksum += 0x72;
                        break;
                    case BN6GameVersion.GAMEVERSION_FALZER:
                        currentChecksum += 0x18;
                        break;
                }
                //set checksum
                Buffer.BlockCopy(BitConverter.GetBytes(currentChecksum), 0, _saveData, saveChecksumOffset, 4);
            }
        }
    }
}