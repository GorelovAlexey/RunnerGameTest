using UniRx;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class MoneyParticles : MonoBehaviour {

        [SerializeField] private ParticleSystem moneyInc;
        [SerializeField] private ParticleSystem moneyRemove;

        public void Start()
        {
            var player = GetComponentInParent<PlayerObject>();
            player.MoneyCount.StartWith(player.MoneyCount.Value).Pairwise().Subscribe((pair) =>
            {
                var delta = pair.Current - pair.Previous;
                if (delta == 0)
                    return;

                delta = (int) (delta * 2f);

                if (delta > 0 && pair.Previous < player.MaxMoney)
                    moneyInc.Emit(delta);

                if (delta < 0 && pair.Previous <= player.MaxMoney)
                    moneyRemove.Emit(-delta);

            }).AddTo(gameObject);
        }
    }

}