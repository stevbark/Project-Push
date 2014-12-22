var LookSensitivity : float = 3;
@HideInInspector
var yRotation : float=0;
@HideInInspector
var xRotation : float=0;
@HideInInspector
var currentYRotation : float=0;
@HideInInspector
var currentXRotation : float=0;
@HideInInspector
var YRotationV : float=0;
@HideInInspector
var XRotationV : float=0;
var CameraRotationSpeed : float = 0.5;
var CameraTiltSpeed : float = 0.5;

var CameraMoveSpeed : float= 1;
var Xmovement:float =0;
var Ymovement:float =0;

var MaxCameraAngle : float = 40;
var MinCameraAngle : float = 50;

var LookSmoothDamp : float = 0.2;

function Update () 
{
    var scrollSpeed = 2;
	var xAxisValue = Input.GetAxis("Horizontal");	
    var yAxisValue = Input.GetAxis("Vertical");
    var zAxisValue: float = Input.GetAxis("Mouse ScrollWheel");
    if(Camera.current != null)
    {
        Camera.current.transform.Translate(new Vector3(xAxisValue,yAxisValue ,zAxisValue));
    }

	transform.Translate(Vector3(0, 0, zAxisValue)*scrollSpeed );
if (Input.GetKey(KeyCode.E))
	{
	yRotation += CameraRotationSpeed;
	}
	
if (Input.GetKey(KeyCode.Q))
	{
	yRotation -= CameraRotationSpeed;
	}
if (Input.GetKey(KeyCode.X))
	{
	xRotation += CameraTiltSpeed;
	}
	
if (Input.GetKey(KeyCode.Z))
	{
	xRotation -= CameraTiltSpeed;
	}
	
	
if (Input.GetKey(KeyCode.W))
	{
	Xmovement += CameraMoveSpeed;
	}
	
if(Input.GetMouseButton(2))
	{
	
	yRotation += Input.GetAxis("Mouse X") * LookSensitivity;
	xRotation += Input.GetAxis("Mouse Y") * LookSensitivity;
	Debug.Log("mouse button 2 with "+ yRotation + "rotation");
	
	}
	
	if(Input.GetKey(KeyCode.R))
	{
	yRotation += Input.GetAxis("Mouse X") * LookSensitivity;
	}
	
	if (Input.GetKey(KeyCode.E))
	{
	yRotation += CameraRotationSpeed;
	}
	
xRotation = Mathf.Clamp(xRotation, MaxCameraAngle, MinCameraAngle);
	
currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, XRotationV, LookSmoothDamp);
currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, YRotationV, LookSmoothDamp);

transform.rotation = Quaternion.Euler(currentXRotation,currentYRotation,0);


}