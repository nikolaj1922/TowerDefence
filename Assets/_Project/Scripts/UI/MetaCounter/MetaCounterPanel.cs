using TMPro;
using Zenject;
using UnityEngine;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.UI.MetaCounter
{
    public class MetaCounterPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _metaCounterText;
        private ISaveLoad _saveLoad; 
        
        [Inject]
        public void Construct(ISaveLoad saveLoad) => _saveLoad  = saveLoad;

        private void Awake() =>  _metaCounterText.text = _saveLoad.PlayerProgress.metaCoinsCount.ToString();
    }
}