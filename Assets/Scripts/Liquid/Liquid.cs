using UnityEngine;

[ExecuteInEditMode]
public class Liquid : MonoBehaviour
{
    public enum UpdateMode
    {
        Normal,
        UnscaledTime
    }

    public ChemicalComponent chemicalComponent;
    public ChemicalIndicator ChemicalIndicator;
    public UpdateMode updateMode;

    //Fancy liquid params
    [SerializeField] private float maxWobble = 0.03f;
    [SerializeField] private float wobbleSpeedMove = 1f;
    [SerializeField] private float fillAmount = 0.5f;
    [SerializeField] private float recovery = 1f;
    [SerializeField] private float thickness = 1f;
    [Range(0, 1)] [SerializeField] private Mesh mesh;
    [SerializeField] private Renderer rend;
    public float compensateShapeAmount;


    private Vector3 _pos;
    private Vector3 _lastPos;
    private Vector3 _velocity;
    private Quaternion _lastRot;
    private Vector3 _angularVelocity;
    private float _wobbleAmountX;
    private float _wobbleAmountZ;
    private float _wobbleAmountToAddX;
    private float _wobbleAmountToAddZ;
    private float _pulse;
    private float _sinewave;
    private float _time = 0.5f;
    private Vector3 _comp;

    public void SetChemicalMaterial(Material material)
    {
        rend.material = material;
    }

    private void Start()
    {
        chemicalComponent = GetComponent<ChemicalComponent>();
        GetMeshAndRend();
    }

    private void OnValidate()
    {
        GetMeshAndRend();
    }

    void GetMeshAndRend()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }

        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
    }

    void Update()
    {
        float deltaTime = 0;
        switch (updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;

            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        _time += deltaTime;

        if (deltaTime != 0)
        {
            HandleWobble(deltaTime);
            HandleSineWave(deltaTime);
            HandleVelocity(deltaTime);
            AddClampedVelocity();
        }

        UpdateShader();
        UpdatePos(deltaTime);
        HandleLastPosition();
    }

    private void HandleLastPosition()
    {
        _lastPos = transform.position;
        _lastRot = transform.rotation;
    }

    private void UpdateShader()
    {
        rend.sharedMaterial.SetFloat("_WobbleX", _wobbleAmountX);
        rend.sharedMaterial.SetFloat("_WobbleZ", _wobbleAmountZ);
    }

    private void HandleWobble(float deltaTime)
    {
        _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, (deltaTime * recovery));
        _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, (deltaTime * recovery));
    }

    private void HandleSineWave(float deltaTime)
    {
        _pulse = 2 * Mathf.PI * wobbleSpeedMove;
        _sinewave = Mathf.Lerp(_sinewave, Mathf.Sin(_pulse * _time),
            deltaTime * Mathf.Clamp(_velocity.magnitude + _angularVelocity.magnitude, thickness, 10));

        _wobbleAmountX = _wobbleAmountToAddX * _sinewave;
        _wobbleAmountZ = _wobbleAmountToAddZ * _sinewave;
    }

    private void HandleVelocity(float deltaTime)
    {
        _velocity = (_lastPos - transform.position) / deltaTime;
        _angularVelocity = GetAngularVelocity(_lastRot, transform.rotation);
    }

    private void AddClampedVelocity()
    {
        _wobbleAmountToAddX +=
            Mathf.Clamp((_velocity.x + (_velocity.y * 0.2f) + _angularVelocity.z + _angularVelocity.y) * maxWobble,
                -maxWobble, maxWobble);
        _wobbleAmountToAddZ +=
            Mathf.Clamp((_velocity.z + (_velocity.y * 0.2f) + _angularVelocity.x + _angularVelocity.y) * maxWobble,
                -maxWobble, maxWobble);
    }

    private void UpdatePos(float deltaTime)
    {
        var worldPos =
            transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));
        if (compensateShapeAmount > 0)
        {
            // only lerp if not paused/normal update
            if (deltaTime != 0)
            {
                _comp = Vector3.Lerp(_comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            }
            else
            {
                _comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));
            }

            _pos = worldPos - transform.position - new Vector3(0, fillAmount - (_comp.y * compensateShapeAmount), 0);
        }
        else
        {
            _pos = worldPos - transform.position - new Vector3(0, fillAmount, 0);
        }

        rend.sharedMaterial.SetVector("_FillAmount", _pos);
    }

    private Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;
        float gain;

        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }

        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);

        if (float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }

        return angularVelocity;
    }

    private float GetLowestPoint()
    {
        var lowestY = float.MaxValue;
        var lowestVert = Vector3.zero;
        var vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = transform.TransformPoint(vertices[i]);

            if (position.y < lowestY)
            {
                lowestY = position.y;
                lowestVert = position;
            }
        }

        return lowestVert.y;
    }
}