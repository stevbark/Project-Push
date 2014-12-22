var LookSensitivity : float = 3;
@HideInInspector
var yRotation : float;
@HideInInspector
var xRotation : float;
@HideInInspector
var currentYRotation : float;
@HideInInspector
var currentXRotation : float;
@HideInInspector
var YRotationV : float;
@HideInInspector
var XRotationV : float;
var CameraRotationSpeed : float = 0.5;
var CameraTiltSpeed : float = 0.5;

var MaxCameraAngle : float = 40;
var MinCameraAngle : float = 50;

var LookSmoothDamp : float = 0.2;

function Update () 
{
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
	
if(Input.GetMouseButton(2))
	{
	yRotation += Input.GetAxis("Mouse X") * LookSensitivity;
	}
	
	if(Input.GetKey(KeyCode.R))
	{
	yRotation += Input.GetAxis("Mouse X") * LookSensitivity;
	}
xRotation = Mathf.Clamp(xRotation, MaxCameraAngle, MinCameraAngle);
	
currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, XRotationV, LookSmoothDamp);
currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, YRotationV, LookSmoothDamp);

transform.rotation = Quaternion.Euler(currentXRotation,currentYRotation,0);
}