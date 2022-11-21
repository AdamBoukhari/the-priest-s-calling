using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardSelector : MonoBehaviour
{
    private RawImage rawImage;
    private Texture faceTexture;
    private Texture backTexture;
    private TextMeshProUGUI childText;

    private bool facedUp = false;

    private const float HALF_ROTATED_POSITION = 90f;
    private const float ROTATED_POSITION = 180f;

    // Dividor of 90 for best effect.
    private const float ROTATE_SPEED = 3f;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        childText = transform.GetComponentInChildren<TextMeshProUGUI>();
        backTexture = rawImage.texture;

        childText.gameObject.SetActive(false);
    }

    public void SetTexture(Texture newTexture)
    {
        faceTexture = newTexture;
    }

    public void InvertTemplateArrival()
    {
        if (!facedUp)
        {
            RotateCardTowardsFace();
        }
        else
        {
            RotateCardTowardsBack();
        }
    }

    private void RotateCardTowardsFace()
    {
        StartCoroutine(RotateCard(faceTexture, true, true, 0));
    }

    private void RotateCardTowardsBack()
    {
        StartCoroutine(RotateCard(backTexture, false, false, ROTATED_POSITION));
    }

    private IEnumerator RotateCard(Texture texture, bool activateChild, bool facingUp, float currentRotation)
    {
        for (float i = currentRotation; i <= ROTATED_POSITION + currentRotation; i += ROTATE_SPEED)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i >= HALF_ROTATED_POSITION + currentRotation)
            {
                rawImage.texture = texture;
                childText.gameObject.SetActive(activateChild);
            }
            yield return new WaitForSeconds(0.01f);
        }

        facedUp = facingUp;
    }
}
