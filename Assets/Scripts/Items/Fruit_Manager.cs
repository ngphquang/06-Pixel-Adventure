using System;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

internal enum FruitsType
{
    Apple,
    Banana,
    Cherry,
    Kiwi,
    Melon,
    Orange,
    Pineapple,
    Strawberry
}

public class Fruit_Manager : MonoBehaviour
{
    [SerializeField] private FruitsType FruitsType;
    private Animator FruitAnimator;
    private Game_Manager GameManager;

    private void Awake()
    {
        FruitAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        GameManager = Game_Manager.GameManagerInstance;
        SetRandomFruits();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.gameObject.GetComponent<Player_Controller>();
        if (player != null) GameManager.AddFruits();
        FruitIsCollected();
    }

    void FruitIsCollected()
    {
        Destroy(gameObject, 0.5f);
        FruitAnimator.SetTrigger("IsCollected");
    }

    private void SetRandomFruits()
    {
        if (!GameManager.GetRandomFruits())
        {
            ChooseSpecificFruits();
        }
        else
        {
            var randomFruitsIndex = Random.Range(0, 8);
            FruitAnimator.SetFloat("Fruits_Index", randomFruitsIndex);
        }
    }

    private void ChooseSpecificFruits()
    {
        FruitAnimator.SetFloat("Fruits_Index", Convert.ToInt32(FruitsType));
    }
}