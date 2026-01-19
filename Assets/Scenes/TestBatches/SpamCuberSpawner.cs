using UnityEngine;

public sealed class CubeMover : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _amplitude = 2f;

    private Vector3 _startPosition;
    private float _time;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        float x = Mathf.Sin(_time * _speed) * _amplitude;
        float z = Mathf.Cos(_time * (_speed * 0.8f)) * _amplitude;

        transform.position = _startPosition + new Vector3(x, 0f, z);
        transform.Rotate(0f, 180f * Time.deltaTime, 0f, Space.World);
    }
}