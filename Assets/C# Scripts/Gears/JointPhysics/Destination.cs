using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Soul"))
        {
            //����㶫�������ø������ˡ���

            Destroy(collision.GetComponent<Rigidbody2D>());

            DOTween.Sequence()
                .Append(collision.transform.DOMove(transform.position,0.5f).SetEase(Ease.OutQuad))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    Instantiate(particle, transform.position, Quaternion.identity);
                    Destroy(collision.gameObject);
                })
                .AppendInterval(2f)
                .OnComplete(() => LevelManager.Instance.FinishLevel());
        }
    }
}
