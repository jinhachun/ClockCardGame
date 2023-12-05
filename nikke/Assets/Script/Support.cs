using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    [SerializeField] Card _cardAddPrefab;
    [SerializeField] Card _cardDeletePrefab;
    [SerializeField] Card _cardEvolvePrefab;
    void Start()
    {
        Set();
    }
    public void Set()
    {
        _cardAddPrefab.Set(CardDatabase.Instance.cardByTier(3));
        _cardDeletePrefab.Set(Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)]);
        _cardEvolvePrefab.Set(Resource.Instance.Deck[Random.Range(0, Resource.Instance.Deck.Count)]);
    }
}
