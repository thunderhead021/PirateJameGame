using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMesh textMesh;
    public float destroyTime;
    public Animator animator;
    public Vector3 randomIntensity = new(1, 1, 1);

    public void SetText(string text, bool isEnemy) 
    {
        textMesh.text = text;
        if (!isEnemy)
        {
            textMesh.anchor = TextAnchor.MiddleCenter;
            transform.localScale = Vector3.one;
            transform.localPosition += new Vector3(Random.Range(-randomIntensity.x * 10, randomIntensity.x * 10),
                                                   Random.Range(-randomIntensity.y * 10, randomIntensity.y * 10),
                                                   Random.Range(-randomIntensity.z * 10, randomIntensity.z * 10));
        }
        else 
        {
            transform.localPosition += new Vector3(Random.Range(-randomIntensity.x, randomIntensity.x),
                                                   Random.Range(-randomIntensity.y, randomIntensity.y),
                                                   Random.Range(-randomIntensity.z, randomIntensity.z));
        }
        gameObject.SetActive(true);
        animator.SetTrigger("Start");
    }

    public void DestroyOnTimer() 
    {
        Destroy(gameObject);
    }
}
