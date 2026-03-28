using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Services.SaveLoad;

namespace _Project.Scripts.UI.MetaCounter
{
    public class MetaCounterPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _metaCounterText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;
        private ISaveLoad _saveLoad; 
        
        [Inject]
        public void Construct(ISaveLoad saveLoad) => _saveLoad  = saveLoad;

        private void Awake()
        {
            _metaCounterText.text = _saveLoad.PlayerProgress.metaCoinsCount.ToString();
            _metaCounterText.ForceMeshUpdate();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(
                _layoutGroup.GetComponent<RectTransform>()
            );
        } 
    }
}