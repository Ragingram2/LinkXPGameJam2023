using UnityEngine;

public class EntityAnimationController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public MeshRenderer meshRenderer;

    private void Start()
    {
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
    }

    public void SetVelocity(Vector2 velocity)
    {
        float sqrMag = velocity.sqrMagnitude;
        if (sqrMag <= 0.000001f)
        {
            animator.SetFloat("speed", 0f);
            return;
        }

        bool vertical = Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y);

        animator.SetBool("vertical", vertical);
        animator.SetFloat("speed", Mathf.Sqrt(sqrMag) * (vertical ? Mathf.Sign(velocity.y) : Mathf.Sign(velocity.x)));
    }

    public void Update()
    {
    }
}
