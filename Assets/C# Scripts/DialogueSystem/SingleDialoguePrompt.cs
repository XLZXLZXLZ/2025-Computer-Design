using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleDialoguePrompt : MonoBehaviour
{


    public static void Interact(SingleDialogueController coroutineStarter,GameObject gameObject,SingleDialoguePromptType promptType) {
        Debug.Log("Interact");
        switch (promptType) {
            case SingleDialoguePromptType.Emphsize_Size:
                coroutineStarter.StartCoroutine(EmphsizeSize(gameObject));
                break;

            case SingleDialoguePromptType.Emphsize_Light:
                coroutineStarter.StartCoroutine(EmphsizeLight(gameObject));
                break;

            case SingleDialoguePromptType.Point:
                coroutineStarter.StartCoroutine(Point(gameObject));
                break;
        }
    }

    static IEnumerator EmphsizeSize(GameObject gameObject) {
        Debug.Log("EmphsizeSize"+gameObject.name);
        Vector2 origin_size = gameObject.transform.localScale;

        for (int i=0;i < 1; i++) {
            for (;gameObject.transform.localScale.x / origin_size.x < 1.48f;)
            {
                Debug.Log("变大");
                gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, origin_size * 1.5f, Time.deltaTime*4);
                yield return new WaitForEndOfFrame();
            }

            for (; gameObject.transform.localScale.x / origin_size.x > 1.02f ;)
            {
                Debug.Log("变小");
                gameObject.transform.localScale = Vector2.Lerp(gameObject.transform.localScale, origin_size, Time.deltaTime*4);
                yield return new WaitForEndOfFrame();
            }
            gameObject.transform.localScale = origin_size;
        }

        yield return null;
    }

    static IEnumerator EmphsizeLight(GameObject gameObject) {
        Debug.Log("EmphsizeLight" + gameObject.name);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Vector4 origin_color = spriteRenderer.color;

        for (int i=0;i < 2; i++) {
            for (;spriteRenderer.color.a > 0.01f;)
            {
                Debug.Log("变淡");
                spriteRenderer.color = Vector4.Lerp(spriteRenderer.color, new Vector4(origin_color.x, origin_color.y, origin_color.z, 0), Time.deltaTime*12);
                yield return new WaitForEndOfFrame();
            }

            for (;origin_color.w - spriteRenderer.color.a > 0.01f; ) {
                Debug.Log("变深");
                spriteRenderer.color = Vector4.Lerp(spriteRenderer.color, origin_color, Time.deltaTime*12);
                yield return new WaitForEndOfFrame();
            }
            spriteRenderer.color = origin_color;
        }

        yield return null;
    }

    static IEnumerator Point(GameObject gameObject) {
        //Debug.Log("Point" + gameObject.name);
        GameObject Pointer_pre = Resources.Load<GameObject>("DialogueData/PromptPrefab/Pointer");
        GameObject newPointer = Instantiate(Pointer_pre);
        newPointer.transform.position = gameObject.transform.position - new Vector3(-1,1,0);
        Destroy(newPointer, 2.1f);
        yield return null;
    }
}

public enum SingleDialoguePromptType { 
    Emphsize_Size,
    Emphsize_Light,
    Point
}
