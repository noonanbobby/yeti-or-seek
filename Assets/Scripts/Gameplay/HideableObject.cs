using UnityEngine;

/// <summary>
/// HideableObject allows a YetiMotor to hide inside of it. When a player hides
/// in this object, their renderer is disabled and they become immobile until
/// they choose to unhide. Only one player may occupy a hideable object at
/// a time. Seekers can reveal hidden players by colliding with the object.
/// </summary>
public class HideableObject : MonoBehaviour
{
    /// <summary>The motor currently occupying this object. Null if empty.</summary>
    private YetiMotor occupant;
    /// <summary>Whether a seeker has discovered the hidden occupant.</summary>
    private bool isRevealed = false;

    /// <summary>Called by a YetiMotor when attempting to hide.</summary>
    /// <param name="motor">The player attempting to hide.</param>
    /// <returns>True if hiding was successful, false otherwise.</returns>
    public bool TryHide(YetiMotor motor)
    {
        if (occupant != null || motor == null) return false;
        occupant = motor;
        motor.EnterHide(this);
        isRevealed = false;
        return true;
    }

    /// <summary>Unhides the current occupant if present.</summary>
    public void Unhide()
    {
        if (occupant == null) return;
        occupant.ExitHide();
        occupant = null;
        isRevealed = false;
    }

    private void OnCollisionEnter(Collision c)
    {
        // If a seeker collides with this hideable, reveal the occupant
        var motor = c.collider.GetComponent<YetiMotor>();
        if (motor != null && motor.role == PlayerRole.Seeker && occupant != null && !isRevealed)
        {
            occupant.FoundBySeeker();
            isRevealed = true;
        }
    }

    /// <summary>Checks if the given motor is currently hiding here.</summary>
    public bool Contains(YetiMotor motor) => occupant == motor;
}
