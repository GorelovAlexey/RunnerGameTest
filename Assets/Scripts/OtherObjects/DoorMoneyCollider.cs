using Assets.Scripts.Player;

namespace Assets.Scripts.OtherObjects
{
    public class DoorMoneyCollider : BasicPlayerIntractible
    {
        public DoorMoneyCollider other;
        public int money;

        private bool deactivated;
        public override void OnPlayerInteraction(PlayerObject player)
        {
            if (deactivated) return;

            other.Deactivate();
            player.MoneyCount.Value += money;
        }

        public void Deactivate()
        {
            deactivated = true;
        }
    }
}