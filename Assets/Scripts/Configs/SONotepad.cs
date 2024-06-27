using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Notepad", menuName = "Levels/Notepad")]
    public class SONotepad : ScriptableObject
    {
        public List<NotepadPageData> NotepadData;
    }
}