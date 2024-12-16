using UnityEngine;

public class SpecialAttack : MonoBehaviour
{
    public float duration;
    private float _timer;

    private AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
