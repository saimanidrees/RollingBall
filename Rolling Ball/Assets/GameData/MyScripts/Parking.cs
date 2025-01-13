using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class Parking : MonoBehaviour
{
    [Tooltip("isTriggered array length must be equal to the trigger boxes, children of Parking (Trigger)")]
    [ReadOnly, SerializeField] private bool[] isTriggered;
    [SerializeField] private UnityEvent onParked;
    public void Trig(int index)
    {
        isTriggered[index] = true;
        if(IsVehicleParked())
            OnParked();
    }
    public void NonTrig(int index)
    {
        isTriggered[index] = false;
    }
    private bool IsVehicleParked()
    {
        return isTriggered.All(t => t);
    }
    private void OnParked()
    {
        //Debug.Log("VehicleParked");
        SoundController.Instance.PlayParkingSound();
        onParked.Invoke();
    }
}