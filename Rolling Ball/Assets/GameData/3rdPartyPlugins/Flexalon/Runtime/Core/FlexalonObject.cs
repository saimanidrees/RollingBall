using UnityEngine;

namespace Flexalon
{
    /// <summary> To control the size of an object, add a Flexalon Object
    /// component to it and edit the width, height, or depth properties. </summary>
    [ExecuteAlways, DisallowMultipleComponent, AddComponentMenu("Flexalon/Flexalon Object"), HelpURL("https://www.flexalon.com/docs/flexalonObject")]
    public class FlexalonObject : FlexalonComponent
    {
        /// <summary> The fixed size of the object. </summary>
        public Vector3 Size
        {
            get => new Vector3(_width, _height, _depth);
            set
            {
                Width = value.x;
                Height = value.y;
                Depth = value.z;
            }
        }

        /// <summary> The relative size of the object. </summary>
        public Vector3 SizeOfParent
        {
            get => new Vector3(_widthOfParent, _heightOfParent, _depthOfParent);
            set
            {
                WidthOfParent = value.x;
                HeightOfParent = value.y;
                DepthOfParent = value.z;
            }
        }

        [SerializeField]
        private SizeType _widthType = SizeType.Component;
        /// <summary> The width type of the object. </summary>
        public SizeType WidthType
        {
            get { return _widthType; }
            set {
                _widthType = value;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _width = 1;
        /// <summary> The fixed width of the object. </summary>
        public float Width
        {
            get { return _width; }
            set {
                _width = Mathf.Max(value, 0);
                _widthType = SizeType.Fixed;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _widthOfParent = 1;
        /// <summary> The relative width of the object. </summary>
        public float WidthOfParent
        {
            get { return _widthOfParent; }
            set {
                _widthOfParent = Mathf.Max(value, 0);
                _widthType = SizeType.Fill;
                MarkDirty();
            }
        }

        [SerializeField]
        private SizeType _heightType = SizeType.Component;
        /// <summary> The height type of the object. </summary>
        public SizeType HeightType
        {
            get { return _heightType; }
            set {
                _heightType = value;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _height = 1;
        /// <summary> The fixed height of the object. </summary>
        public float Height
        {
            get { return _height; }
            set {
                _height = Mathf.Max(value, 0);
                _heightType = SizeType.Fixed;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _heightOfParent = 1;
        /// <summary> The relative height of the object. </summary>
        public float HeightOfParent
        {
            get { return _heightOfParent; }
            set {
                _heightOfParent = Mathf.Max(value, 0);
                _heightType = SizeType.Fill;
                MarkDirty();
            }
        }

        [SerializeField]
        private SizeType _depthType = SizeType.Component;
        /// <summary> The depth type of the object. </summary>
        public SizeType DepthType
        {
            get { return _depthType; }
            set {
                _depthType = value;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _depth = 1;
        /// <summary> The fixed depth of the object. </summary>
        public float Depth
        {
            get { return _depth; }
            set {
                _depth = Mathf.Max(value, 0);
                _depthType = SizeType.Fixed;
                MarkDirty();
            }
        }

        [SerializeField]
        private float _depthOfParent = 1;
        /// <summary> The relative depth of the object. </summary>
        public float DepthOfParent
        {
            get { return _depthOfParent; }
            set {
                _depthOfParent = Mathf.Max(value, 0);
                _depthType = SizeType.Fill;
                MarkDirty();
            }
        }

        [SerializeField]
        private Vector3 _offset = Vector3.zero;
        /// <summary> Use offset to add an offset to the final position of the gameObject after layout is complete. </summary>
        public Vector3 Offset
        {
            get { return _offset; }
            set { _offset = value; MarkDirty(); }
        }

        [SerializeField]
        private Vector3 _scale = Vector3.one;
        /// <summary> Use rotation to scale the size of the gameObject before layout runs.
        /// This will generate a new size to encapsulate the scaled object. </summary>
        public Vector3 Scale
        {
            get { return _scale; }
            set { _scale = value; MarkDirty(); }
        }

        [SerializeField]
        private Quaternion _rotation = Quaternion.identity;
        /// <summary> Use rotation to set the rotation of the gameObject before layout runs.
        /// This will generate a new size to encapsulate the rotated object. </summary>
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginLeft;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginLeft
        {
            get { return _marginLeft; }
            set { _marginLeft = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginRight;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginRight
        {
            get { return _marginRight; }
            set { _marginRight = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginTop;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginTop
        {
            get { return _marginTop; }
            set { _marginTop = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginBottom;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginBottom
        {
            get { return _marginBottom; }
            set { _marginBottom = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginFront;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginFront
        {
            get { return _marginFront; }
            set { _marginFront = value; MarkDirty(); }
        }

        [SerializeField]
        private float _marginBack;
        /// <summary> Margin to add additional space around a gameObject. </summary>
        public float MarginBack
        {
            get { return _marginBack; }
            set { _marginBack = value; MarkDirty(); }
        }

        /// <summary> Margin to add additional space around a gameObject. </summary>
        public Directions Margin
        {
            get => new Directions(new float[] {
                _marginRight, _marginLeft, _marginTop, _marginBottom, _marginBack, _marginFront});
            set
            {
                _marginRight = value.Values[0];
                _marginLeft = value.Values[1];
                _marginTop = value.Values[2];
                _marginBottom = value.Values[3];
                _marginBack = value.Values[4];
                _marginFront = value.Values[5];
                MarkDirty();
            }
        }

        [SerializeField]
        private float _paddingLeft;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingLeft
        {
            get { return _paddingLeft; }
            set { _paddingLeft = value; MarkDirty(); }
        }

        [SerializeField]
        private float _paddingRight;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingRight
        {
            get { return _paddingRight; }
            set { _paddingRight = value; MarkDirty(); }
        }

        [SerializeField]
        private float _paddingTop;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingTop
        {
            get { return _paddingTop; }
            set { _paddingTop = value; MarkDirty(); }
        }

        [SerializeField]
        private float _paddingBottom;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingBottom
        {
            get { return _paddingBottom; }
            set { _paddingBottom = value; MarkDirty(); }
        }

        [SerializeField]
        private float _paddingFront;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingFront
        {
            get { return _paddingFront; }
            set { _paddingFront = value; MarkDirty(); }
        }

        [SerializeField]
        private float _paddingBack;
        /// <summary> Padding to reduce available space inside a layout. </summary>
        public float PaddingBack
        {
            get { return _paddingBack; }
            set { _paddingBack = value; MarkDirty(); }
        }

        /// <summary> Padding to reduce available space inside a layout. </summary>
        public Directions Padding
        {
            get => new Directions(new float[] {
                _paddingRight, _paddingLeft, _paddingTop, _paddingBottom, _paddingBack, _paddingFront});
            set
            {
                _paddingRight = value.Values[0];
                _paddingLeft = value.Values[1];
                _paddingTop = value.Values[2];
                _paddingBottom = value.Values[3];
                _paddingBack = value.Values[4];
                _paddingFront = value.Values[5];
                MarkDirty();
            }
        }

        /// <inheritdoc />
        protected override void ResetProperties()
        {
            _node.SetFlexalonObject(null);
        }

        /// <inheritdoc />
        protected override void UpdateProperties()
        {
            _node.SetFlexalonObject(this);
        }

#if UNITY_EDITOR
        private Transform _lastParent;

        /// <inheritdoc />
        public override void DoUpdate()
        {
            if (Application.isPlaying || Node.Dirty)
            {
                return;
            }

            // Don't update prefab instances
            if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                return;
            }

            var result = _node.Result;

            // Don't do any of this if the parent changed.
            if (_lastParent != transform.parent)
            {
                _lastParent = transform.parent;
                result.TargetScale = transform.localScale;
                result.TransformScale = transform.localScale;
                result.TargetRotation = transform.localRotation;
                result.TransformRotation = transform.localRotation;
                result.TargetPosition = transform.localPosition;
                result.TransformPosition = transform.localPosition;
            }

            // Detect changes to the object's position, rotation, scale, and rect size which may happen
            // when the developer uses the transform control, enters new values in the
            // inspector, or various other scenarios. Maintain those edits
            // by modifying the offset, rotation, and scale on the FlexalonObject.

            if (transform is RectTransform rectTransform)
            {
                if (_widthType == SizeType.Fixed)
                {
                    if (rectTransform.rect.size.x != _width)
                    {
                        // Avoid recording changes here to avoid screen size changes causing edits.
                        _width = rectTransform.rect.size.x;
                    }
                }

                if (_heightType == SizeType.Fixed)
                {
                    if (rectTransform.rect.size.y != _height)
                    {
                        // Avoid recording changes here to avoid screen size changes causing edits.
                        _height = rectTransform.rect.size.y;
                    }
                }
            }

            bool shouldScale = _node.Adapter.TryGetScale(_node, out var s);
            if (shouldScale && result.TransformScale != transform.localScale)
            {
                Debug.Log("Scale change from " + result.TransformScale + " to " + transform.localScale);
                UnityEditor.Undo.RecordObject(this, "Scale change");
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                UnityEditor.Undo.RecordObject(result, "Scale change");
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(result);
                Flexalon.RecordFrameChanges = true;
                _scale = Math.Mul(Scale, Math.Div(transform.localScale, result.TransformScale));
                if (float.IsNaN(_scale.x) || Mathf.Abs(_scale.x) < 1e-5f) _scale.x = 0;
                if (float.IsNaN(_scale.y) || Mathf.Abs(_scale.y) < 1e-5f) _scale.y = 0;
                if (float.IsNaN(_scale.z) || Mathf.Abs(_scale.z) < 1e-5f) _scale.z = 0;
                if (Mathf.Abs(1f - _scale.x) < 1e-5f) _scale.x = 1;
                if (Mathf.Abs(1f - _scale.y) < 1e-5f) _scale.y = 1;
                if (Mathf.Abs(1f - _scale.z) < 1e-5f) _scale.z = 1;
                result.TargetScale = transform.localScale;
                result.TransformScale = transform.localScale;
                _node.Parent?.MarkDirty();

                if (_node.Constraint != null)
                {
                    _node.MarkDirty();
                }
                else
                {
                    _node.ApplyScaleAndRotation();
                }

                // The scale and rect transform controls affect both position and scale,
                // That's not expected in a layout, so early out here to avoid setting the position.
                return;
            }

            bool inLayoutOrConstraint =
                (_node.Parent != null && !_node.Parent.Dirty && transform.parent == _node.Parent.GameObject.transform) ||
                (_node.Constraint != null && _node.Constraint.Target != null);

            if (inLayoutOrConstraint)
            {
                if (result.TransformPosition != transform.localPosition)
                {
                    UnityEditor.Undo.RecordObject(this, "Offset change");
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                    UnityEditor.Undo.RecordObject(result, "Offset change");
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(result);

                    if (Node.Constraint != null && Node.Constraint.Target != null)
                    {
                        _offset += Quaternion.Inverse(Node.Constraint.Target.transform.rotation) * (transform.localPosition - result.TransformPosition);
                    }
                    else
                    {
                        _offset += Math.Mul(Node.Parent.Result?.ComponentScale ?? Vector3.one, (transform.localPosition - result.TransformPosition));
                    }

                    if (float.IsNaN(_offset.x) || Mathf.Abs(_offset.x) < 1e-5f) _offset.x = 0;
                    if (float.IsNaN(_offset.y) || Mathf.Abs(_offset.y) < 1e-5f) _offset.y = 0;
                    if (float.IsNaN(_offset.z) || Mathf.Abs(_offset.z) < 1e-5f) _offset.z = 0;

                    if (transform is RectTransform offsetRectTransform)
                    {
                        offsetRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    }

                    result.TargetPosition = transform.localPosition;
                    result.TransformPosition = transform.localPosition;
                }

                if (result.TransformRotation != transform.localRotation)
                {
                    UnityEditor.Undo.RecordObject(this, "Rotation change");
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
                    UnityEditor.Undo.RecordObject(result, "Rotation change");
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(result);
                    Flexalon.RecordFrameChanges = true;

                    if (Node.Constraint != null && Node.Constraint.Target != null)
                    {
                        _rotation = Quaternion.Inverse(Node.Constraint.Target.transform.rotation) * transform.rotation;
                    }
                    else
                    {
                        _rotation *= transform.localRotation * Quaternion.Inverse(result.TransformRotation);
                    }

                    if (float.IsNaN(_rotation.x) || Mathf.Abs(_rotation.x) < 1e-5f) _rotation.x = 0;
                    if (float.IsNaN(_rotation.y) || Mathf.Abs(_rotation.y) < 1e-5f) _rotation.y = 0;
                    if (float.IsNaN(_rotation.z) || Mathf.Abs(_rotation.z) < 1e-5f) _rotation.z = 0;
                    if (float.IsNaN(_rotation.w) || Mathf.Abs(1 - _rotation.w) < 1e-5f) _rotation.w = 1;

                    _rotation.Normalize();
                    result.TargetRotation = transform.localRotation;
                    result.TransformRotation = transform.localRotation;
                    _node.Parent?.MarkDirty();

                    if (_node.Constraint != null)
                    {
                        _node.MarkDirty();
                    }
                    else
                    {
                        _node.ApplyScaleAndRotation();
                    }
                }
            }
            else
            {
                if (result.TransformRotation != transform.localRotation)
                {
                    UnityEditor.Undo.RecordObject(result, "Rotation change");
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(result);
                    result.TargetRotation = transform.localRotation;
                    result.TransformRotation = transform.localRotation;
                    _node.ApplyScaleAndRotation();
                }
            }
        }
#endif

        protected override void Initialize()
        {
            if (transform is RectTransform || (transform.parent && transform.parent is RectTransform))
            {
                _width = 100;
                _height = 100;
            }
        }

        protected override void Upgrade(int fromVersion)
        {
#if UNITY_UI
            // UPGRADE FIX: In v4.0 canvas no longer scales to fit layout size.
            // Instead, scale needs to be set on the FlexalonObject.
            if (fromVersion < 4 && TryGetComponent<Canvas>(out var canvas))
            {
                _widthType = SizeType.Component;
                _heightType = SizeType.Component;

                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    _scale = canvas.transform.localScale;
                    _node.Result.AdapterBounds = new Bounds(Vector3.zero, (transform as RectTransform).rect.size);
                }
            }
#endif
        }
    }
}