using UnityEngine;

public abstract class BasicPlayerIntractible : MonoBehaviour
{
    bool collidedWithPlayer = false;

    // Для движущегося препядствия
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision event");
        ProcessCollisionGameObject(collision.gameObject);
    }

    // Для статичного триггера
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger event");
        ProcessCollisionGameObject(other.gameObject);
    }

    void ProcessCollisionGameObject(GameObject collision)
    {
        if (collidedWithPlayer)
            return;

        var player = GetPlayer(collision);
        if (!player)
            return;

        Debug.Log("Player event");

        collidedWithPlayer = true;
        OnPlayerInteraction(player);
    }

    private PlayerObject GetPlayer(GameObject o) => o.GetComponent<PlayerObject>();

    public abstract void OnPlayerInteraction(PlayerObject player);
}
