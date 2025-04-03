using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableObject : MonoBehaviour
{
    [SerializeField] private float _smoothSpeed = 10f;
    
    private float _zCoord;
    private bool _isDragging;

    private Camera _camera;
    private Vector3 _offset;

    private int _originalSortingOrder;
    private SpriteRenderer _sr;

    [SerializeField] private bool _usePhysics;
    [SerializeField] private float _forceMultiplier = 10f;
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        
        _originalSortingOrder = _sr.sortingOrder;
        _sr.sortingOrder = 100;
        
        _zCoord = _camera.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!_isDragging)
            return;

        var targetPos = GetMouseWorldPosition() + _offset;
        
        if (_usePhysics && _rb)
        {
            var forceDirection = (targetPos - transform.position) * _forceMultiplier;
            _rb.linearVelocity = forceDirection;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, _smoothSpeed * Time.deltaTime);
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;
        
        _sr.sortingOrder = _originalSortingOrder;
    }

    private Vector3 GetMouseWorldPosition()
    {
        var position = Input.mousePosition;
        position.z = _zCoord;
        
        return _camera.ScreenToWorldPoint(position);
    }
}
