using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLockJoint : InGameJoint
{
    private Color showColor = Color.yellow;
    //AudioController audioController;
    private void Start()
    {
        //audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = showColor;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    public void Release()
    {
        AudioManager.Instance.PlaySe("Ä¾¹÷ÕÛ¶ÏÒôÐ§");
        onRelease?.Invoke(this);
<<<<<<< Updated upstream
        
        //audioController.PlaySfx(audioController.wood_break);
=======
        AudioManager.Instance.PlaySe("LockBreak");
>>>>>>> Stashed changes
    }

}
