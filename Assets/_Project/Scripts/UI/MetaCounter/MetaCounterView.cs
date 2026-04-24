using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.MetaCounter
{
    public class MetaCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _metaCounterText;
        [SerializeField] private HorizontalLayoutGroup _layoutGroup;

        public void UpdateView(string  metaCounterText)
        {
            _metaCounterText.text = metaCounterText;
            _metaCounterText.ForceMeshUpdate();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.GetComponent<RectTransform>());
        }
    }
}