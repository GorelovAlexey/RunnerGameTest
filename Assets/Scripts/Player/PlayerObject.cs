using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    PlayerSlideController slideController;
    PlayerMoneyPanel panel;
    

    public PlayerSlideController PlayerSlideController => slideController;

    // Start is called before the first frame update
    void Start()
    {
        slideController = GetComponentInParent<PlayerSlideController>();
        panel = GetComponentInChildren<PlayerMoneyPanel>();
        panel.MoneyValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
