using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour , ILevelSettlement
{
    [SerializeField]
    private GameObject particle;

    [SerializeField] float DoubleStarTimeLimit = 60;
    [SerializeField] float TribleStarTimeLimit = 30;
    float startTime;

    [HideInInspector] public int Score { get; set; }
    [SerializeField] LevelSettlementController _lsc;
    public LevelSettlementController LSC { get => _lsc; }

    public void Settle()
    {
        if(Time.time - startTime < TribleStarTimeLimit) 
            Score = 3;
        else if(Time.time - startTime < DoubleStarTimeLimit) 
            Score = 2;
        else Score = 1;

        LSC.Finished(this);
    }

    void Start() { 
        startTime = Time.time;
    }

    //LevelManager.Instance.FinishLevel()

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Soul"))
        {
            //就这点东西，懒得搞对象池了。。

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
                .OnComplete(() => Settle());
        }
    }
}
