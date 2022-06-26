using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask deathMask;

    const float skinWidth = 0.015f;
    public int horizontalRayCount;
    public int verticalRayCount;

    BoxCollider2D collider_;
    RaycastOrigins raycastOrigins;

    const float dstBetweenRays = .25f;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public CollisionInfo collisions;


    void Start()
    {
        collider_ = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (velocity.x != 0)
        {
            //ref é para que a variavel recebida de velocidade seja mudada mesmo estando em outro script, 
            //já que ref não cria uma cópia da variavel, ela usa a a própria variavel
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //se a direção de x for para trás, a origem do raio vai ser o ponto bottom left , caso contrário, será o ponto bottom right
            // ? seria um if , e ! seria um else
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            //cria o raio, que sai da origem, vai para a direita, e tem o tamanho da velocidade em x
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D death = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, deathMask);

            //raio para debug
            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit || death)
            {
                //se a distancia do quadrado e da parede for 0 por exemplo, a velocidade em x será 0, o que faz sentido no código abaixo
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                //se a direção em x for -1, a colisão foi na esquerda, caso tenha sido 1 foi na direita
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }

            if (death)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            // ? seria um if , e ! seria um else
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            //cria o raio, que sai da origem, vai para a direita, e tem o tamanho da velocidade em y
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D death = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, deathMask);

            //raio para debug
            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit || death)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                //se a direção em y for -1, a colisão foi embaixo, caso tenha sido 1 foi em cima
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
            if (death)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        //cria as bounds e expande ela para dentro, para que os raios sejam disparados mesmo quando o jogador estiver 
        //no chão por exemplo, esse valor para dentro se chama skin width
        Bounds bounds = collider_.bounds;
        bounds.Expand(skinWidth * -2);

        //cria o ponto de origem de cada raycast em cada canto do quadrado, baseando-se na bound definida anteriormente, que já está com o skin width
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider_.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        //aqui é necessário fazer com que o valor dos raios seja maior ou igual a 2, por isso é usado a função math clamp, 
        //onde o valor minimo é setado para 2 e o maximo para basicamente infinito
        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        //para achar o espaçamento entre os raios, divide-se o tamanho da aresta pela quantidade de raios -1
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
