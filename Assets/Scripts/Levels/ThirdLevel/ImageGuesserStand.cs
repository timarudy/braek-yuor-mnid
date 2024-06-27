using System.Linq;
using Configs;
using Infrastructure.AssetManagement;
using Levels.Common;
using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Levels.ThirdLevel
{
    public class ImageGuesserStand : InputStand
    {
        [SerializeField] private Image Image;
        [SerializeField] private Image BgMarker;
        [SerializeField] private LabyrinthDoor Door;

        [SerializeField] private Color AcceptedColor;
        [SerializeField] private Color RejectedColor;

        private SOStands _stands;
        private SOGuessImages _images;
        private PlayerHealth _player;

        [Inject]
        private void Construct(PlayerController player)
        {
            _player = player.GetComponent<PlayerHealth>();
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            _stands = AssetProvider.LoadResource<SOStands>(AssetPath.ImageGuessersDataPath);
            _images = AssetProvider.LoadResource<SOGuessImages>(AssetPath.GuessImagesDataPath);
            GuessImageData guessImageData = _images.GuessImages.Find(image => image.Sprite == Image.sprite);

            foreach (InputStandData stand in _stands.InputStandsData)
            {
                if (stand.Decryption == guessImageData.Name)
                {
                    Id = stand.Id;
                }
            }
        }

        protected override void DecryptionRejected()
        {
            base.DecryptionRejected();
            _player.GetDamage(1);
            BgMarker.color = new Color(RejectedColor.r, RejectedColor.g, RejectedColor.b, 0.1f);
        }

        public override InputStandData GetInputStandData()
        {
            InputStandData standData = _stands.InputStandsData.FirstOrDefault(stand => stand.Id == Id);

            return standData;
        }

        protected override void CheckAccess()
        {
            Door.OpenDoor();
            BgMarker.color = new Color(AcceptedColor.r, AcceptedColor.g, AcceptedColor.b, 0.1f);
            CodeAccessChecker.ImagesGuessed++;
        }
    }
}