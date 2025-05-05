using UnityEngine;

public class BasketballController : MonoBehaviour
{
    [Header("Movimiento del Jugador")]
    public float MoveSpeed = 10f;

    [Header("Lanzamiento")]
    public float baseLaunchDistance = 3f;
    public float maxLaunchDistance = 10f;
    public float chargeSpeed = 5f;
    public float gravity = -9.8f;
    [SerializeField] private float maxHeight = 5f;

    private float currentLaunchDistance;
    private float launchDistance;

    [Header("Referencias")]
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

    [Header("Trayectoria")]
    public LineRenderer TrajectoryLine;
    public int trajectoryResolution = 30;

    private bool IsBallInHands = true;

    void Start()
    {
        TrajectoryLine.enabled = false;
        currentLaunchDistance = baseLaunchDistance;
        Ball.GetComponent<Rigidbody>().isKinematic = false;

        Physics.gravity = new Vector3(0, gravity, 0); // Aplicar gravedad personalizada
    }

    void Update()
    {
        // Movimiento del jugador
        Vector3 direction = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += MoveSpeed * Time.deltaTime * direction;
        if (direction != Vector3.zero)
            transform.LookAt(transform.position + direction);

        if (IsBallInHands)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Cargar lanzamiento
                currentLaunchDistance += chargeSpeed * Time.deltaTime;
                currentLaunchDistance = Mathf.Clamp(currentLaunchDistance, baseLaunchDistance, maxLaunchDistance);

                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // Dibujar trayectoria
                Vector3 launchDirection = transform.forward;
                DrawTrajectory(PosOverHead.position, launchDirection, currentLaunchDistance, maxHeight);
                TrajectoryLine.enabled = true;

                //Debug.Log("Posicion manos arriba: " + Ball.position);
            }
            else
            {
                // Animación de dribbling
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 7));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                // Lanzar balón
                launchDistance = currentLaunchDistance;
                currentLaunchDistance = baseLaunchDistance;

                Lanzar();
                IsBallInHands = false;
                TrajectoryLine.enabled = false;
            }
        }
    }

    void Lanzar()
    {
        Vector3 launchDirection = transform.forward;
        Vector3 A = PosOverHead.position;
        Ball.position = A;
        Vector3 B = A + launchDirection * launchDistance;

        Rigidbody ballRB = Ball.GetComponent<Rigidbody>();
        ballRB.useGravity = true;

        ballRB.velocity = CalcularVelocidadInicial(A, B);

        //Debug.Log("Posicion antes de lanzar: " + ballRB.position);
        Debug.Log("Velocidad inicial: " + ballRB.velocity);
    }

    Vector3 CalcularVelocidadInicial(Vector3 origen, Vector3 destino)
    {
        Vector3 desplazamiento = destino - origen;
        float g = Mathf.Abs(gravity);

        float velocidadY = Mathf.Sqrt(2 * g * maxHeight);
        float t1 = velocidadY / g;
        float h = desplazamiento.y + maxHeight;
        if (h < 0) h = 0;

        float t2 = Mathf.Sqrt(2 * h / g);
        float tTotal = t1 + t2;
        float velocidadX = desplazamiento.x / tTotal;
        float velocidadZ = desplazamiento.z / tTotal;

        return new Vector3(velocidadX, velocidadY, velocidadZ);
    }

    void DrawTrajectory(Vector3 start, Vector3 direction, float distance, float height)
    {
        TrajectoryLine.positionCount = trajectoryResolution;

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i / (float)(trajectoryResolution - 1);
            Vector3 linearPos = Vector3.Lerp(start, start + direction * distance, t);
            Vector3 arc = height * Mathf.Sin(t * Mathf.PI) * Vector3.up;
            TrajectoryLine.SetPosition(i, linearPos + arc);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands)
        {
            IsBallInHands = true;
        }
    }
}
