using System.Collections;
using UnityEngine;
using Google.Play.Review;
public class ReviewManger : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

    private int _count;
    // Start is called before the first frame update
    private void Start()
    {
        //AppOpenAdCaller.IsInterstitialAdPresent = true;
        StartCoroutine(RequestReviews());
    }
    private IEnumerator RequestReviews()
    {
        //Debug.Log("RequestReviews");
        _reviewManager = new ReviewManager();
        // Request a ReviewInfo Object
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        //Debug.LogError("Responce_requestFlowOperation " + requestFlowOperation.GetResult().ToString());
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            FirebaseManager.Instance.ReportEvent("RateUs_Error_" + requestFlowOperation.Error.ToString());
             //Debug.LogError("Error_At_requestFlowOperation " + requestFlowOperation.Error.ToString());
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();
        //Lunch the InappReview
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        //Debug.LogError("Responce_launchFlowOperation "+launchFlowOperation.GetResult().ToString());
        //Debug.LogError("Responce_launchFlowOperation "+launchFlowOperation.IsDone.ToString());
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            FirebaseManager.Instance.ReportEvent("RateUs_Error_Lunch_"+launchFlowOperation.Error.ToString());
            //Debug.LogError("Error_At_launchFlowOperation "+launchFlowOperation.Error.ToString());
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
}