using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] AudioSource DropCoin;

    private void OnTriggerEnter(Collider other)
    {
        DropCoin.Play();
        this.gameObject.SetActive(false);
    }
}
