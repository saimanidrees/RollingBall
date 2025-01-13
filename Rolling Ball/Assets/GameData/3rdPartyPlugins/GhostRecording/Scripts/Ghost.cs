using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class WB_Vector3 {

	private float x;
	private float y;
	private float z;

	public WB_Vector3() { }
	public WB_Vector3(Vector3 vec3) {
		this.x = vec3.x;
		this.y = vec3.y;
		this.z = vec3.z;
	}

	public static implicit operator WB_Vector3(Vector3 vec3) {
		return new WB_Vector3(vec3);
	}
	public static explicit operator Vector3(WB_Vector3 wb_vec3) {
		return new Vector3(wb_vec3.x, wb_vec3.y, wb_vec3.z);
	}
}

[System.Serializable]
public class WB_Quaternion {

    private float w;
	private float x;
	private float y;
	private float z;

	public WB_Quaternion() { }
	public WB_Quaternion(Quaternion quat3) {
		this.x = quat3.x;
		this.y = quat3.y;
		this.z = quat3.z;
        this.w = quat3.w;
	}

	public static implicit operator WB_Quaternion(Quaternion quat3) {
		return new WB_Quaternion(quat3);
	}
	public static explicit operator Quaternion(WB_Quaternion wb_quat3) {
		return new Quaternion(wb_quat3.x, wb_quat3.y, wb_quat3.z, wb_quat3.w);
	}
}

[System.Serializable]
public class GhostShot
{
    public float timeMark = 0.0f;       // mark at which the position and rotation are of af a given shot

    private WB_Vector3 _posMark;
    public Vector3 posMark {
		get {
			if (_posMark == null) {
				return Vector3.zero;
			} else {
				return (Vector3)_posMark;
			}
		}
		set {
			_posMark = (WB_Vector3)value;
		}
	}

    private WB_Quaternion _rotMark;
    public Quaternion rotMark {
		get {
			if (_rotMark == null) {
				return Quaternion.identity;
			} else {
				return (Quaternion)_rotMark;
			}
		}
		set {
			_rotMark = (WB_Quaternion)value;
		}
	}

}

public class Ghost : MonoBehaviour {

    private List<GhostShot> _framesList;
    private List<GhostShot> _lastReplayList = null;

    private GameObject _theGhost;

    private const float ReplayTimescale = 1;
    private int _replayIndex = 0;
    private float _recordTime = 0.0f;
    private float _replayTime = 0.0f;

    //Check whether we should be recording or not
    private bool _startRecording = false, _recordingFrame = false, _playRecording = false, _isRecorded = false;

    public void loadFromFile() {
		//Check if Ghost file exists. If it does load it
		var path = Application.persistentDataPath + "/Ghost";
		if(File.Exists(path)) {
			var bf = new BinaryFormatter();
			var file = File.Open(path, FileMode.Open);
			_lastReplayList = (List<GhostShot>)bf.Deserialize(file);
			file.Close();
			Debug.Log("Ghost Loaded");
		} else {
			Debug.Log("No Ghost Found");
		}
	}

	private void FixedUpdate () {
        if (_startRecording) {
            _startRecording = false;
			Debug.Log("Recording Started");
            StartRecording();
        } else if (_recordingFrame) {
		    RecordFrame();
        }
        if (_lastReplayList != null && _playRecording) {
            MoveGhost();
        }
	}

	private void RecordFrame() {
		_recordTime += Time.smoothDeltaTime * 1000;
        GhostShot newFrame = new GhostShot()
        {
            timeMark = _recordTime,
			posMark = this.transform.position,
			rotMark = this.transform.rotation
        };

        _framesList.Add(newFrame);
	}

	public void StartRecording() {
        _framesList = new List<GhostShot>();
        _replayIndex = 0;
        _recordTime = Time.time * 1000;
        _recordingFrame = true;
		_playRecording = false;
    }

	public void StopRecordingGhost() {
		_recordingFrame = false;
		_lastReplayList = new List<GhostShot>(_framesList);

		//This will overwrite any previous Save
		//Run function if new highscore achieved or change filename in function
        //SaveGhostToFile(); //Save Ghost to file on device/computer

	}

	public void playGhostRecording() {
		CreateGhost();
		_replayIndex = 0;
		_playRecording = true;
	}

	public void StartRecordingGhost() {
        _startRecording = true;
    }

    public void MoveGhost()
    {
        _replayIndex++;

        if (_replayIndex < _lastReplayList.Count)
        {
            GhostShot frame = _lastReplayList[_replayIndex];
            DoLerp(_lastReplayList[_replayIndex - 1], frame);
            _replayTime += Time.smoothDeltaTime * 1000 * ReplayTimescale;
        }
    }

    private void DoLerp(GhostShot a, GhostShot b)
    {
		if(GameObject.FindWithTag("Ghost") != null) {
	        _theGhost.transform.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(_replayTime, a.timeMark, b.timeMark));
	        _theGhost.transform.rotation = Quaternion.Slerp(a.rotMark, b.rotMark, Mathf.Clamp(_replayTime, a.timeMark, b.timeMark));
		}
    }

    public void SaveGhostToFile()
    {
        // Prepare to write
        var path = Application.persistentDataPath + "/Ghost";
        var bf = new BinaryFormatter();
        var file = File.Create(path);
        Debug.Log("File Location: " + path);
        // Write data to disk
        bf.Serialize(file, _lastReplayList);
        file.Close();
    }

    public void CreateGhost()
    {
		//Check if ghost exists or not, no reason to destroy and create it everytime.
		if(GameObject.FindWithTag("Ghost") == null) {
	        _theGhost = Instantiate(Resources.Load("GhostPrefab", typeof(GameObject))) as GameObject;
	        if (_theGhost == null) return;
	        _theGhost.gameObject.tag = "Ghost";

	        //Disable RigidBody
	        //theGhost.GetComponent<Rigidbody>().isKinematic = true;

	        var mr = _theGhost.gameObject.GetComponent<MeshRenderer>();
	        mr.material = Resources.Load("Ghost_Shader", typeof(Material)) as Material;
		}
    }
}
