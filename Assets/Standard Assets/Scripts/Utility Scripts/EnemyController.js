public var idleAnimation : AnimationClip;
public var runAnimation : AnimationClip;
public var attackAnimation : AnimationClip;

public var runMaxAnimationSpeed : float = 1.0;
public var attackAnimationSpeed : float = 1.0;

public var hands : GameObject[];
private var haveHands = false;
var hand1;
var hand2;

public var weaponattacklenght = 1.0;
public var weaponScale : float = 1.0;

private var _animation : Animation;

private var _characterState : CharacterState;

// The gravity for the character
var gravity = 10.0;

// The gravity in controlled descent mode
var speedSmoothing = 10.0;
var rotateSpeed = 500.0;
var trotAfterSeconds = 3.0;

// The current move direction in x-z
private var moveDirection = Vector3.zero;
// The current vertical speed

private var moveSpeed = 0.0;
private var verticalSpeed = 0.0;
private var groundedTimeout = 0.25;

// Is the user pressing any keys?
private var isMoving = false;
// When did the user start walking (Used for going into trot after a while)
private var walkTimeStart = 0.0;

// Last time we performed a jump
private var lastJumpTime = -1.0;


private var inAirVelocity = Vector3.zero;

private var lastGroundedTime = 0.0;


//EnemyAI
private
public var visioSphere : SphereCollider;
public var target : Transform;
public var attackRadius: float = 0.3f;

private var state = 0;
private var PATRULHA = 0;
private var PERSEGUIR = 1;
private var ATACAR = 2;

function Awake ()
{
	if(target == null && GameObject.FindWithTag("Player"))
		target = GameObject.FindWithTag("Player").transform;
			
	moveDirection = transform.TransformDirection(Vector3.forward);
	
	_animation = GetComponent(Animation);
	if(!_animation)
		Debug.Log("The character you would like to control doesn't have animations. Moving her might look weird.");
		
	if(!idleAnimation) {
		_animation = null;
		Debug.Log("No idle animation found. Turning off animations.");
	}
	if(!runAnimation) {
		_animation = null;
		Debug.Log("No run animation found. Turning off animations.");
	}
	if(!attackAnimation) {
		_animation = null;
		Debug.Log("No attack animation found. Turning off animations.");
	}
	if(hands.Length < 2){
		Debug.Log("Don't have 2 hands");
	}else{
		if(!hands[0]){
			Debug.Log("Hand 0 not defined");
		}else{
			if(!hands[1]){
				Debug.Log("Hand 1 not defined");
			}else{
				haveHands = true;
				hands[0].transform.localScale = new Vector3(weaponScale, weaponScale, weaponScale);
				hand1 = Instantiate(hands[0], gameObject.transform.FindChild("Dummy001").position,
										gameObject.transform.FindChild("Dummy001").rotation);
				hands[1].transform.localScale = new Vector3(-weaponScale, weaponScale, weaponScale);
				hand2 = Instantiate(hands[1], gameObject.transform.FindChild("Dummy002").position,
										gameObject.transform.FindChild("Dummy002").rotation);				
			}
		}
	}
}

function UpdateAI ()
{
	if(state == PATRULHA){
			
	}else if(state == PERSEGUIR){
		moveDirection = target.position - transform.position;
	}else if(state == ATACAR){
		ataque();
	}
}

function ApplyGravity ()
{
		if (IsGrounded ())
			verticalSpeed = 0.0;
		else
			verticalSpeed -= gravity * Time.deltaTime;
}

function Update() {
	UpdateAI();
	
	ApplyGravity ();
	
	// Calculate actual motion
	var movement = moveDirection * moveSpeed + Vector3 (0, verticalSpeed, 0) + inAirVelocity;
	movement *= Time.deltaTime;
	
	//MOVEMENT
	
	if(state == PERSEGUIR && moveDirection.magnitude > attackRadius){
		transform.position.x += moveDirection.x * Time.deltaTime;
		transform.position.y += moveDirection.y * Time.deltaTime;
		transform.position.z += moveDirection.z * Time.deltaTime;
	}
	
	if(haveHands){
			hand1.transform.position = gameObject.transform.FindChild("Dummy001").position;
			hand1.transform.rotation = gameObject.transform.FindChild("Dummy001").rotation;
			hand2.transform.position = gameObject.transform.FindChild("Dummy002").position;
			hand2.transform.rotation = gameObject.transform.FindChild("Dummy002").rotation;
	}
	
	// ANIMATION sector
	if(_animation) {
		
		if(state == PATRULHA) {//recebe ações do controle "A"
			_animation.CrossFade(idleAnimation.name);
		}
		else if(state == PERSEGUIR) {
				_animation[runAnimation.name].speed = Mathf.Clamp(0.7, 0.0, runMaxAnimationSpeed);
				_animation.CrossFade(runAnimation.name);	
		}
	}
	// ANIMATION sector
	
	// Set rotation to the move direction
	if (IsGrounded())
	{
		var xzOnlyMove = moveDirection;
		xzOnlyMove.y = 0;
		transform.rotation = Quaternion.LookRotation(xzOnlyMove);
		
	}	
	else
	{
		var xzMove = movement;
		xzMove.y = 0;
		if (xzMove.sqrMagnitude > 0.001)
		{
			transform.rotation = Quaternion.LookRotation(xzMove);
		}
	}	
	
	// We are in jump mode but just became grounded
	if (IsGrounded())
	{
		lastGroundedTime = Time.time;
		inAirVelocity = Vector3.zero;
	}
}

function OnTriggerEnter(collisor : Collider){
	if(collisor.tag == "Player"){
		state = PERSEGUIR;
		target = collisor.transform;
	}
}

function OnTriggerExit(collisor : Collider){
	if(collisor.tag == "Player" && state == PERSEGUIR){
		state = PATRULHA;
		target = null;
		print("Patrulha no ponto: "+collisor.transform.position.ToString());
	}
}

function OnTriggerStay(collisor : Collider){
	if(collisor.tag == "Player" && moveDirection.magnitude <= attackRadius)
		state = ATACAR;
}

function GetSpeed () {
	return moveSpeed;
}

function IsGrounded () {
	return CollisionFlags.CollidedBelow != 0;
}

function GetDirection () {
	return moveDirection;
}

function IsMoving ()  : boolean
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
}

function IsGroundedWithTimeout ()
{
	return lastGroundedTime + groundedTimeout > Time.time;
}

function ataque(){
	_animation[attackAnimation.name].speed = attackAnimationSpeed;
	_animation.Play(attackAnimation.name);
	state = PERSEGUIR;
	var h = GameObject.Find("Astra").GetComponent("HealthBar");
	h.health = h.health - 0.1;
}
