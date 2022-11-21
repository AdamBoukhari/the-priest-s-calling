using UnityEngine;

public class BatPath : MonoBehaviour
{
    //Ce fichier sert uniquement a afficher des guizmos afin de previsualiser la trajectoire que l'on donne a la chauve-souris
    //Les points sont dessine en fonction de la courbe de bezier pour creer la courbe il est necessaire de positionner 4 points.
    [SerializeField] private Transform[] controlPoints;

    private Vector2 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;
            Gizmos.DrawSphere(gizmosPosition, 0.05f);
        }

        Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
            new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
            new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
    }
}
