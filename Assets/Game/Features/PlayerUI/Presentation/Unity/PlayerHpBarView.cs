using Game.Features.PlayerUI.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.PlayerUI.Presentation.Unity
{
    public sealed class PlayerHpBarView : MonoBehaviour, IPlayerHpView
    {
        [SerializeField] private Image _fill;

        public void SetNormalized(float value)
        {
            _fill.fillAmount = value;
        }
    }
}