using UnityEngine;

public class ProperLerping : MonoBehaviour
{
    [Header("Set Dynamically")]
    /// <summary>
    /// Time taken to move from the start position to the end position
    /// </summary>
    public float timeTakenDuringLerp = 1f;

    /// <summary>
    /// Desired distance to move
    /// </summary>
    public float distanceToMove = 10;

    /// <summary>
    /// Whether we are currently interpolating or not
    /// </summary>
    private bool _isLerping = false;

    /// <summary>
    /// Start and finish positions for the interpolation
    /// </summary>
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    /// <summary>
    /// The time value when we started lerping
    /// </summary>
    private float timeStartedLerping;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartLerping();
        }
    }

    private void FixedUpdate()
    {
        if (_isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping. Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }

    private void StartLerping()
    {
        _isLerping = true;
        timeStartedLerping = Time.time;

        _startPosition = transform.position;
        _endPosition = transform.position + Vector3.right * distanceToMove;
    }
}
