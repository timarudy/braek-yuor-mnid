using Services.SceneServices;
using UnityEngine.SceneManagement;

namespace Services.LevelAccess
{
    public class CodeAccessChecker : ICodeAccessChecker
    {
        public int CodeDecryptorsCompleted { get; set; }
        public bool IsPigRescued { get; set; }
        public int ImagesGuessed { get; set; }

        public bool CheckAccess()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case SceneNames.FirstLevel:
                    return CodeDecryptorsCompleted == 3;
                case SceneNames.SecondLevel:
                    return IsPigRescued;
                case SceneNames.ThirdLevel:
                    return ImagesGuessed == 4;
            }

            return false;
        }
    }
}