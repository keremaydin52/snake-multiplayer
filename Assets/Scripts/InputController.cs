using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [HideInInspector]
    public InputKeys inputKeys;
    
    private LinkedList<Vector2Int> _queue;
    private Vector2Int _lastDirection;
    
    private Vector2Int LastDirection
    {
        get
        {
            if (_queue.Count == 0) return _lastDirection;

            return _queue.Last.Value;
        }
    }
    
    void Start()
    {
        Reset();
    }

    public void Initialize(InputKeys inputs)
    {
        inputKeys = inputs;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(inputKeys.upKey) && LastDirection != Vector2.down)
        {
            Enqueue(Vector2Int.up);
        }
        else if (Input.GetKeyDown(inputKeys.downKey) && LastDirection != Vector2.up)
        {
            Enqueue(Vector2Int.down);
        }
        else if (Input.GetKeyDown(inputKeys.leftKey) && LastDirection != Vector2.right)
        {
            Enqueue(Vector2Int.left);
        }
        else if (Input.GetKeyDown(inputKeys.rightKey) && LastDirection != Vector2.left)
        {
            Enqueue(Vector2Int.right);
        }
    }
    
    private void Enqueue(Vector2Int direction)
    {
        _queue.AddLast(direction);
        _lastDirection = direction;
    }
    
    public Vector2Int NextDirection()
    {
        if (_queue.Count == 0) return _lastDirection;

        var first = _queue.First.Value;
        _queue.RemoveFirst();

        return first;
    }

    public void Reset()
    {
        _queue = new LinkedList<Vector2Int>();
        Enqueue(Vector2Int.up);
    }
}

[Serializable]
public struct InputKeys
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
}