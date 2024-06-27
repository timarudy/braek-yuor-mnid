using System.Collections.Generic;
using Data;
using Services.Progress;
using UI.Factory;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    // public class AnimalsWindow : WindowBase
    // {
        // [SerializeField] private Transform AnimalsContainer;
        //
        // private IPersistentProgressService _progressService;
        // private IUIFactory _uiFactory;
        //
        // [Inject]
        // private void Construct(IPersistentProgressService progressService, IUIFactory uiFactory)
        // {
        //     _uiFactory = uiFactory;
        //     _progressService = progressService;
        // }
        //
        // protected override void CloseSelf()
        // {
        //     WindowService.CloseCurrentWindow();
        //     UIInput.IsWindowOpened = false;
        //     WindowService.Open(WindowType.SETTINGS);
        // }
        //
        // protected override void OnAwake()
        // {
        //     base.OnAwake();
        //     
        //     List<AnimalData> animalsData = _progressService.PlayerProgress.PlayerProgressData.AnimalsData;
        //
        //     foreach (AnimalData animalData in animalsData) 
        //         _uiFactory.CreateAnimalItem(animalData.Name, animalData.IsActive, AnimalsContainer);
        // }
    // }
}