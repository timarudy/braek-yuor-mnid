using System;

namespace Configs
{
    [Serializable]
    public struct NotepadPageData
    {
        public PageType PageType;
        public string Text;
        public int PageNumber;
    }
}