﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Null
}

public class Movement : MonoBehaviour
{
    Renderer R;

    public float Speed;
    private Direction _direction;
    private bool _idle;
    private bool _immobile;

    public delegate void MovementChanged(Direction d, bool idle);
    public event MovementChanged OnMovementChange;

    public Direction Direction
    {
        get { return _direction; }
        set
        {
            if (!Immobile && _direction != value)
            {
                _direction = value;
                if (OnMovementChange != null)
                {
                    OnMovementChange(value, Idle);
                }
            }
        }
    }

    public bool Idle
    {
        get { return _idle; }
        set
        {
            if (!Immobile)
            {
                _idle = value;
                if (OnMovementChange != null)
                    OnMovementChange(Direction, value);
            }
        }
    }

    public bool Immobile
    {
        get { return _immobile; }
        set
        {
            if (value == true)
                Idle = true;
            _immobile = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        R = GetComponent<Renderer>();

        Immobile = false;
        Idle = true;
        Direction = Direction.Right;
        SetSortingLayer();
    }

    //Makes the player move
    public void Move(Direction direction)
    {
        if (!Immobile)
        {
            transform.Translate(DirectionToVector(direction) * Speed * Time.fixedDeltaTime);
            SetSortingLayer();
        }
    }

    //Makes the object move towards a location
    public void Move(Vector2 direction)
    {
        transform.Translate(direction * Speed * Time.fixedDeltaTime);
        VectorToDirection(direction);
        SetSortingLayer();
    }

    //Converts the Enum direction to a Vector. Used for moving around
    public Vector3 DirectionToVector()
    {
        Vector3 direction = Vector2.zero;
        switch(Direction)
        {
            case Direction.Up:
                direction = Vector2.up;
                break;
            case Direction.Down:
                direction = Vector2.down;
                break;
            case Direction.Left:
                direction = Vector2.left;
                break;
            case Direction.Right:
                direction = Vector2.right;
                break;
        }

        return direction;
    }

    //Converts the Enum direction to a Vector. Used for moving around
    public Vector3 DirectionToVector(Direction facing)
    {
        Vector3 direction = Vector2.zero;
        switch (facing)
        {
            case Direction.Up:
                direction = Vector2.up;
                break;
            case Direction.Down:
                direction = Vector2.down;
                break;
            case Direction.Left:
                direction = Vector2.left;
                break;
            case Direction.Right:
                direction = Vector2.right;
                break;
        }

        return direction;
    }

    //Makes the object look at the target
    public void LookAt(Movement target)
    {
        switch (target.Direction)
        {
            case Direction.Up:
                Direction = Direction.Down;
                break;
            case Direction.Down:
                Direction = Direction.Up;
                break;
            case Direction.Left:
                Direction = Direction.Right;
                break;
            case Direction.Right:
                Direction = Direction.Left;
                break;
        }
    }

    //Converts the target vector into a direction
    public void VectorToDirection(Vector2 target)
    {
        float xtest = Mathf.Abs(target.x);
        float ytest = Mathf.Abs(target.y);

        if (target.y > 0)
        {
            if (target.x > 0.25f && target.x < 0.75f)
            {
                Direction = Direction.Right;
            }
            else if (target.x < -0.33f && target.x > -0.75f)
            {
                Direction = Direction.Left;
            }
            else
            {
                Direction = Direction.Up;
            }
        }
        else if (target.y < 0)
        {
            if (target.x > 0.25f && target.x < 0.75f)
            {
                Direction = Direction.Right;
            }
            else if (target.x < -0.25f && target.x > -0.75f)
            {
                Direction = Direction.Left;
            }
            else
            {
                Direction = Direction.Down;
            }
        }
        else if (target.x > 0)
        {
            Direction = Direction.Right;         
        }
        else if (target.x < 0)
        {
            Direction = Direction.Left;
        }
    }

    //Sets the render-order, relative to the other objects in the scene,
    //making it look like the object walks behind other objects
    void SetSortingLayer()
    {
        float objHeight = transform.position.y - R.bounds.size.y / 2f;
        R.sortingOrder = (int)(objHeight * -100);
    }
}