using UnityEngine;

/// <summary>
/// Where the desktop/WebGL player is standing and what they are looking at.
///
/// The desktop player has no hand colliders, so interactions that VR drives by touch have to be
/// driven by "stood close enough and looking at it" instead. Walking a body capsule into a small
/// trigger volume is fiddly at best and impossible where the volume sits inside a sink basin or a
/// bin, which is what this exists to avoid.
/// </summary>
public static class PlayerReach
{
    private static Transform player;
    private static Camera view;

    /// <summary>
    /// The player's body, found by the Player tag. Re-found automatically after a scene change.
    /// </summary>
    public static Transform Player
    {
        get
        {
            if (player == null)
            {
                GameObject found = GameObject.FindGameObjectWithTag("Player");
                player = (found != null) ? found.transform : null;
            }
            return player;
        }
    }

    /// <summary>
    /// The player's viewpoint. Falls back to the body when there is no main camera.
    /// </summary>
    public static Transform Eye
    {
        get
        {
            if (view == null) view = Camera.main;
            return (view != null) ? view.transform : Player;
        }
    }

    /// <summary>
    /// True when the player is standing within range of a point and looking roughly at it.
    /// </summary>
    /// <param name="point">World position to test against - usually a collider's bounds centre.</param>
    /// <param name="range">How far away the player can stand, in metres.</param>
    /// <param name="maxAngle">How far off-centre the point may sit in the player's view.
    /// Pass 180 to accept any facing.</param>
    public static bool IsWithinReach(Vector3 point, float range, float maxAngle = 180f)
    {
        Transform body = Player;
        if (body == null) return false;
        if ((point - body.position).sqrMagnitude > range * range) return false;
        if (maxAngle >= 180f) return true;

        Transform eye = Eye;
        if (eye == null) return true;

        Vector3 toPoint = point - eye.position;
        // Standing right on top of the point counts however you are facing.
        if (toPoint.sqrMagnitude < 0.0001f) return true;

        return Vector3.Angle(eye.forward, toPoint) <= maxAngle;
    }
}
