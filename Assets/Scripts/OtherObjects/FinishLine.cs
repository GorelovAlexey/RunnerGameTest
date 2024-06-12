using Assets.Scripts.Player;
using Assets.Scripts.UI;

namespace Assets.Scripts.OtherObjects
{
    public class FinishLine : BasicPlayerIntractible
    {
        public override void OnPlayerInteraction(PlayerObject player)
        {
            var gameManager = FindAnyObjectByType<GameManager>();

            gameManager.LevelComplete();
        }
    }

}