using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public Transform SnakeHead;
    public float CircleDiameter;
    public float CollisionInterval = 0.2f;
    public Text hitpointText;
    public Player Player;
    public int Hitpoints { get; private set; }

    private List<Transform> snakeCircles = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();
    private float collisionTimer = 0;

    void Start()
    {
        positions.Add(SnakeHead.position);
        Hitpoints = 1;
        AddCircle();

    }

    void Update()
    {
        collisionTimer -= Time.deltaTime;

        float distance = (SnakeHead.position - positions[0]).magnitude;

        if (distance > CircleDiameter)
        {
            Vector3 direction = (SnakeHead.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * CircleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= CircleDiameter;
        }

        for (int i = 0; i < snakeCircles.Count; i++)
        {
            snakeCircles[i].position = Vector3.Lerp(positions[i + 1], positions[i], distance / CircleDiameter);
        }
    }

    public void AddCircle()
    {
        Hitpoints++;
        hitpointText.text = Hitpoints.ToString();
        Transform circle = Instantiate(SnakeHead, positions[positions.Count - 1], Quaternion.identity, transform);
        snakeCircles.Add(circle);
        positions.Add(circle.position);
    }

    public void RemoveCircle()
    {
        int lastIndex = snakeCircles.Count - 1;

        if (snakeCircles.Count == 0)
        {
            //����� �����������
            Destroy(gameObject);
            Player.Die();
        }
        else
        {
            Hitpoints--;
            hitpointText.text = Hitpoints.ToString();
            Destroy(snakeCircles[lastIndex].gameObject);
            snakeCircles.RemoveAt(lastIndex);
            positions.RemoveAt(lastIndex + 1);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Food food))
        {
            for (int i = 0; i < food.Amount; i++)
            {
                AddCircle();
            }

            Destroy(other.gameObject);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (collisionTimer <= 0 && other.gameObject.TryGetComponent(out Block block))
        {
            block.ApplyDamage();
            RemoveCircle();
            collisionTimer = CollisionInterval;
        }
    }
}
