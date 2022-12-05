using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float mSpeed;

    public CharacterController pc;

    Vector3 move;

    public float movementX;
    public float movementZ;

    [SerializeField] protected List<string> Quest;

    [SerializeField] protected Inventory _inventory;

    [SerializeField] protected Animator _animator;

    [SerializeField] protected Transform _sprite;

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();    
        _sprite = GetComponentInChildren<Transform>();
    }

    void Update()
    {
        movementZ = Input.GetAxisRaw("Vertical");
        movementX = Input.GetAxisRaw("Horizontal");

        _animator.SetFloat("speedX", movementX);

        _animator.SetFloat("speedZ", movementZ);

        bool flip = movementX < 0;
        _sprite.rotation = Quaternion.Euler(new Vector3(0f, flip? 180f : 0f, 0f));
        move = new Vector3(movementX, 0f, movementZ) * mSpeed;
    }

    void FixedUpdate()
    {
        pc.SimpleMove(move);
    }

    public bool Search(string item_name)
    {
        return _inventory.Search(item_name);
    }

    public void setItem(Item item)
    {
        _inventory.add(item);
    }

    public void setQuest(string quest)
    {
        Quest.Add(quest);
    }

    public void UseItem(string item_name)
    {
        _inventory.use(item_name);
    }
}
