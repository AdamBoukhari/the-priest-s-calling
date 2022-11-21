using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Returns information about a raycast hit on a certain layer.
    /// </summary>
    /// <param name="self">Game object to shoot the raycasts from</param>
    /// <param name="layer">Target layer</param>
    /// <param name="distance">Raycast Length</param>
    /// <returns>0 if no raycast hit, - 1 if a raycast hit on the left, 1 if a raycast hit on the right</returns>
    public static int GetLayerDirection(this GameObject self, LayerMask layer, Vector2 position, float distance)
    {
        Vector2 offset = new Vector2(0, 0.1f);

        RaycastHit2D lowerLeft = Physics2D.Raycast(position - offset, Vector2.left, distance, layer);
        RaycastHit2D middleLeft = Physics2D.Raycast(position, Vector2.left, distance, layer);
        RaycastHit2D upperLeft = Physics2D.Raycast(position + offset, Vector2.left, distance, layer);

        RaycastHit2D lowerRight = Physics2D.Raycast(position - offset, Vector2.right, distance, layer);
        RaycastHit2D middleRight = Physics2D.Raycast(position, Vector2.right, distance, layer);
        RaycastHit2D upperRight = Physics2D.Raycast(position + offset, Vector2.right, distance, layer);

        if (lowerLeft.collider != null || middleLeft.collider != null || upperLeft.collider != null)
        {
            return -1;
        }
        else if (lowerRight.collider != null || middleRight.collider != null || upperRight.collider != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Returns information about a raycast hit on a certain layer.
    /// </summary>
    /// <param name="self">Game object to shoot the raycasts from</param>
    /// <param name="layer">Target layer</param>
    /// <param name="distance">Raycast Length</param>
    /// <returns>0 if no raycast hit, - 1 if a raycast hit on the left, 1 if a raycast hit on the right</returns>
    public static int GetLayerDirectionPrecisely(this GameObject self, LayerMask layer, Vector2 position, Vector2 offset, float distance)
    {
        RaycastHit2D lowerLeft = Physics2D.Raycast(position - offset, Vector2.left, distance, layer);
        RaycastHit2D lowerMiddleLeft = Physics2D.Raycast(position - offset / 2, Vector2.left, distance, layer);
        RaycastHit2D middleLeft = Physics2D.Raycast(position, Vector2.left, distance, layer);
        RaycastHit2D upperMiddleLeft = Physics2D.Raycast(position + offset / 2, Vector2.left, distance, layer);
        RaycastHit2D upperLeft = Physics2D.Raycast(position + offset, Vector2.left, distance, layer);

        RaycastHit2D lowerRight = Physics2D.Raycast(position - offset, Vector2.right, distance, layer);
        RaycastHit2D lowerMiddleRight = Physics2D.Raycast(position - offset / 2, Vector2.right, distance, layer);
        RaycastHit2D middleRight = Physics2D.Raycast(position, Vector2.right, distance, layer);
        RaycastHit2D upperMiddleRight = Physics2D.Raycast(position + offset / 2, Vector2.right, distance, layer);
        RaycastHit2D upperRight = Physics2D.Raycast(position + offset, Vector2.right, distance, layer);

        if (lowerLeft.collider != null || lowerMiddleLeft.collider != null || middleLeft.collider != null || upperMiddleLeft.collider != null || upperLeft.collider != null)
        {
            return -1;
        }
        else if (lowerRight.collider != null || lowerMiddleRight.collider != null || middleRight.collider != null || upperMiddleRight.collider != null || upperRight.collider != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static bool IsStandingOnThisLayer(this GameObject self, LayerMask layer, Vector3 position, float distance)
    {
        Vector3 offset = new Vector3(0.02f, 0, 0);
        Vector2 direction = Vector2.down;

        RaycastHit2D left = Physics2D.Raycast(position - offset, direction, distance, layer);
        RaycastHit2D middle = Physics2D.Raycast(position , direction, distance, layer);
        RaycastHit2D right = Physics2D.Raycast(position + offset, direction, distance, layer);

        if (left.collider != null || middle.collider != null || right.collider != null)
        {
            return true;
        }
        return false;
    }

}

