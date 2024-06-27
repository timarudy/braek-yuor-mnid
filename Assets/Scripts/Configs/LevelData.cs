using System;

namespace Configs
{
    [Serializable]
    public class LevelData
    {
        public string LevelName;
        public string LevelCode;
        public int Id;
        public bool IsOpenedLevel;
        public SONotepad NotepadData;
    }
}