using UnityEngine;

public class LauncherWeapon : MonoBehaviour
{
    public Animator weaponAnimator;

    private void Start()
    {
        // Ensure you have assigned the Animator component in the Unity Editor
        if (weaponAnimator == null)
        {
            Debug.LogError("Animator component not assigned to LauncherWeapon script!");
        }
    }

    // Call this method to play the "Attack" animation
    public void PlayAttackAnimation()
    {
        // Play the "Attack" animation
        weaponAnimator.Play("Attack");
    }

    // Call this method to play the "Idle" animation
    public void PlayIdleAnimation()
    {
        // Play the "Idle" animation
        weaponAnimator.Play("Idle");
    }
}
