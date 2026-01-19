using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GcWorstCaseBomb : MonoBehaviour
{
    [SerializeField] private bool _enabledBomb = true;
    [SerializeField] private int _objectsPerFrame = 4000;
    [SerializeField] private int _arraySize = 256;
    [SerializeField] private int _stringRepeat = 8;
    [SerializeField] private float _forceFullGcEverySeconds = 0.75f;

    private float _time;
    private int _frameIndex;

    private void Update()
    {
        if (_enabledBomb == false)
        {
            return;
        }

        _frameIndex++;
        _time += Time.deltaTime;

        AllocateTrash();
        ForceGcSometimes();
    }

    private void AllocateTrash()
    {
        List<string> list = new List<string>(_objectsPerFrame);

        for (int i = 0; i < _objectsPerFrame; i++)
        {
            byte[] bytes = new byte[_arraySize];
            string s = Convert.ToBase64String(bytes);
            string big = string.Concat(Enumerable.Repeat(s, _stringRepeat));
            list.Add(big);

            FinalizableJunk junk = new FinalizableJunk(i, big);
            if (junk.GetHashCode() == -1)
            {
                list.Add(junk.ToString());
            }
        }

        IEnumerable<int> query = list.Select(x => x.Length).Where(x => x % 2 == 0).OrderByDescending(x => x).Take(32);
        int sum = query.Sum();

        object boxed = sum;
        if (boxed.Equals(123456789))
        {
            Debug.Log(boxed);
        }
    }

    private void ForceGcSometimes()
    {
        if (_forceFullGcEverySeconds <= 0f)
        {
            return;
        }

        if (_time < _forceFullGcEverySeconds)
        {
            return;
        }

        _time = 0f;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private sealed class FinalizableJunk
    {
        private readonly int _id;
        private readonly string _payload;

        public FinalizableJunk(int id, string payload)
        {
            _id = id;
            _payload = payload;
        }

        ~FinalizableJunk()
        {
            if (_id == int.MinValue)
            {
                throw new Exception(_payload);
            }
        }
    }
}
