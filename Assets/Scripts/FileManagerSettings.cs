using System.Collections.Generic;
using UnityEngine;

namespace AlexisDev.Localization
{
    public class FileManagerSettings
    {
        public List<JsonFileData> JsonFiles;
        public Dictionary<string, JsonFileData> JsonFileDataMap;
        public Dictionary<string, TextAsset> ContnJson;

        public FileManagerSettings(List<JsonFileData> jsonFiles, Dictionary<string, JsonFileData> jsonFileDataMap, Dictionary<string, TextAsset> contnJson)
        {
            JsonFiles = jsonFiles;
            JsonFileDataMap = jsonFileDataMap;
            ContnJson = contnJson;
        }
    }
}
