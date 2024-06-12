using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.OtherObjects
{
    [ExecuteInEditMode]
    public class DoorsController : MonoBehaviour
    {
        [SerializeField] private bool leftBad;
        [SerializeField] private string textLeft;
        [SerializeField] private string textRight;
        [SerializeField] private int moneyDiff;

        [Space]
        [SerializeField] private TextMeshProUGUI textMeshLeft;
        [SerializeField] private TextMeshProUGUI textMeshRight;

        [SerializeField] private Material matBad;
        [SerializeField] private Material matGood;

        [SerializeField] private Transform doorLeft;
        [SerializeField] private Transform doorRight;

        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }

        void Setup()
        {
            var mr = doorLeft.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in mr)
            {
                r.material = leftBad ? matBad : matGood;
            }
            textMeshLeft.text = textLeft;


            var mr2 = doorRight.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in mr2)
            {
                r.material = leftBad ? matGood : matBad;
            }
            textMeshRight.text = textRight;

            var left = doorLeft.GetComponent<DoorMoneyCollider>();
            var right = doorRight.GetComponent<DoorMoneyCollider>();
            left.other = right;
            right.other = left;
            left.money = leftBad ? -moneyDiff : moneyDiff;
            right.money = -left.money;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}