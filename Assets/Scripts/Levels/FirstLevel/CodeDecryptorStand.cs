using System.Linq;
using Configs;
using Infrastructure.AssetManagement;
using Levels.Common;
using TMPro;

namespace Levels.FirstLevel
{
    public class CodeDecryptorStand : InputStand
    {
        public TextMeshProUGUI Encryption;
        
        public override InputStandData GetInputStandData()
        {
            SOStands stands = AssetProvider.LoadResource<SOStands>(AssetPath.CodeDecryptorsDataPath);

            InputStandData standData =
                stands.InputStandsData.FirstOrDefault(stand => stand.Id == Id);

            return standData;
        }

        protected override void CheckAccess() => 
            CodeAccessChecker.CodeDecryptorsCompleted++;
    }
}