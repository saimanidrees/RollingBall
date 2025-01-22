using _RollingBall.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class LevelProgressTracker : MonoBehaviour
{
    private Transform _ball; // Reference to the ball
    [SerializeField] private Transform[] pathPoints; // Transform points defining the path
    [SerializeField] private Transform[] revivePoints;
    [SerializeField] private Slider progressSlider; // UI slider to show progress
    [SerializeField] private Text levelNoText;
    [SerializeField] private bool activateCameraReCentering = true;
    private float _totalPathDistance; // Total distance along the path
    private const string LevelString = "Level ";
    private void Start()
    {
        _ball = GamePlayManager.Instance.ball.transform;
        levelNoText.text = LevelString + PlayerPrefsHandler.LevelsCounter;
        // Calculate the total distance along the path at the start
        _totalPathDistance = CalculateTotalPathDistance();
        progressSlider.minValue = 0;
        progressSlider.maxValue = 1;
    }
    private void Update()
    {
        // Calculate the current distance along the path from the ball to the destination
        var distanceToDestination = CalculateDistanceToDestination();
        // Calculate progress as a percentage
        var progress = 1 - (distanceToDestination / _totalPathDistance);
        // Update the slider value
        progressSlider.value = Mathf.Clamp01(progress); // Ensure it's between 0 and 1
    }
    private float CalculateTotalPathDistance()
    {
        var distance = 0f;
        // Sum up the distances between consecutive path points
        for (var i = 0; i < pathPoints.Length - 1; i++)
        {
            distance += Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
        }
        return distance;
    }
    private float CalculateDistanceToDestination()
    {
        var distance = 0f;
        // Find the closest segment of the path to the ball
        var closestSegmentIndex = 0;
        var closestDistance = float.MaxValue;
        for (var i = 0; i < pathPoints.Length - 1; i++)
        {
            var segmentStart = pathPoints[i].position;
            var segmentEnd = pathPoints[i + 1].position;
            // Calculate the projected position of the ball on the segment
            var position = _ball.position;
            var projectedPoint = GetClosestPointOnSegment(position, segmentStart, segmentEnd);
            // Calculate the distance from the ball to the projected point
            var distanceToSegment = Vector3.Distance(position, projectedPoint);
            if (!(distanceToSegment < closestDistance)) continue;
            closestDistance = distanceToSegment;
            closestSegmentIndex = i;
        }
        // Add the distance from the ball to the destination, following the path
        var closestSegmentStart = pathPoints[closestSegmentIndex].position;
        var closestSegmentEnd = pathPoints[closestSegmentIndex + 1].position;
        // Project the ball's position onto the closest segment
        var ballProjection = GetClosestPointOnSegment(_ball.position, closestSegmentStart, closestSegmentEnd);
        // Add the distance from the ball's projection to the end of the segment
        distance += Vector3.Distance(ballProjection, closestSegmentEnd);
        // Add the remaining distances along the path
        for (var i = closestSegmentIndex + 1; i < pathPoints.Length - 1; i++)
        {
            distance += Vector3.Distance(pathPoints[i].position, pathPoints[i + 1].position);
        }
        return distance;
    }
    private static Vector3 GetClosestPointOnSegment(Vector3 point, Vector3 segmentStart, Vector3 segmentEnd)
    {
        // Calculate the vector from the segment start to the point
        var segmentVector = segmentEnd - segmentStart;
        var pointVector = point - segmentStart;
        // Project the point vector onto the segment vector
        var t = Vector3.Dot(pointVector, segmentVector) / Vector3.Dot(segmentVector, segmentVector);
        // Clamp t to the range [0, 1] to ensure the projected point lies on the segment
        t = Mathf.Clamp01(t);
        // Calculate the closest point on the segment
        return segmentStart + t * segmentVector;
    }
    public Transform GetClosestTransform(Vector3 ballLastPosition)
    {
        Transform closestTransform = null;
        var closestDistance = Mathf.Infinity;
        foreach (var t in revivePoints)
        {
            var distance = Vector3.Distance(ballLastPosition, t.position);
            if (!(distance < closestDistance)) continue;
            closestDistance = distance;
            closestTransform = t;
        }
        return closestTransform;
    }
}