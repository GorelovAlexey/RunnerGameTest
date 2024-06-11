using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.OtherObjects
{
    public class MoneyObject : BasicPlayerIntractible
    {
        public int money;
        public override void OnPlayerInteraction(PlayerObject player)
        {
            player.MoneyCount.Value -= money;
            DestroySelf();
        }

        private void DestroySelf()
        {
            // TODO ANIM PARTICLE
            Destroy(gameObject, 5);
        }
    }
}