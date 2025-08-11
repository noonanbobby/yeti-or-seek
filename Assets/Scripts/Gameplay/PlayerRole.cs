/// <summary>
/// PlayerRole defines whether a YetiMotor is currently seeking or hiding. This is
/// used by the HideSeekManager to assign behaviors and by HideableObject to
/// determine whether collisions result in a reveal.
/// </summary>
public enum PlayerRole
{
    Hider,
    Seeker
}
