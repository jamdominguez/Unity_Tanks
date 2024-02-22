using UnityEngine;

public class ShellExplosion : MonoBehaviour {
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 100f;
    public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_ExplosionRadius = 5f;


    private void Start() {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other) {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        foreach(Collider target in colliders) {
            Rigidbody targetRgb = target.GetComponent<Rigidbody>();
            if (!targetRgb) continue;
            targetRgb.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = target.GetComponent<TankHealth>();
            if (!targetHealth) continue;
            float damage = CalculateDamage(target.transform.position);
            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.SetParent(null); // para evitar destuir el sistema de particulas cuando se destrulle el proyectil
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Destroy(gameObject);
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
    }


    private float CalculateDamage(Vector3 targetPosition) {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 vectorDistance = targetPosition - transform.position;
        float distance = Mathf.Min(vectorDistance.magnitude, m_ExplosionRadius); // para asegurar un daño mínimo independientemente de la distancia al centro del target
        float relativeDistance = (m_ExplosionRadius - distance) / m_ExplosionRadius; // 0 - 1;

        return m_MaxDamage * relativeDistance;
    }
}