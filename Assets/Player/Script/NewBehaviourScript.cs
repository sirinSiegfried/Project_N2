using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//플레이어 기본 이동 제어 변수
	[SerializeField]
	private float walkSpeed; //기본 이동 속도
	[SerializeField]
	private float runSpeed; //달리기 시 이동속도
	private float applySpeed; //플레이어  오브젝트의 현재 이동 속도
	[SerializeField]
	private float jumpForce; //점프 시 플레이어의 상승 속도

	//플레이어의 현재 상태 표시
	private bool isRun = false; //달리기 여부
	private bool isGround = true; //지면 착지 여부

	// 플레이어 오브젝트 지면 착지 여부
	private CapsuleCollider capsuleCollider;

	// 마우스 민감도
	[SerializeField]
	private float lookSensitivity;

	// 카메라 회전 각도 제한
	[SerializeField]
	private float cameraRotationLimit;
	private float currentCameraRotationX = -10;

	[SerializeField]
	private Camera theCamera;
	private Rigidbody myRigid;
	public Animator anim;
	void Start()
	{
		capsuleCollider = GetComponent<CapsuleCollider>();
		myRigid = GetComponent<Rigidbody>();
		applySpeed = walkSpeed;
	}
	void Update()
	{
		Tryattack();
		IsGround();
		TryJump(); //점프 모션 실행 가능 여부 체크
		TryRun(); //달리기 모션 실행 가능 여부 체크
		Move(); //플레이어의 기본 이동 제어
		CameraRotation(); //메인 카메라 상,하 회전 제어
		CharacterRotation(); //메인 카메라 좌,우 회전 제어

	}
	private void Tryattack() // 공격 가능 여부 판단
	{
		if (Input.GetButton("Fire1"))
		{
			anim.SetTrigger("Attack");
		}
	}
	private void IsGround()
	{
		isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
	}
	private void TryJump() // 점프 모션 실행 가능 여부 판단
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			Jump();
		}
	}
	private void Jump() //점프 모션 실행
	{
		anim.SetTrigger("Jump");
		myRigid.velocity = transform.up * jumpForce;
	}
	private void TryRun() //달리기 모션 실행 가능 여부 판단
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			Running();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			RunningCancel();
		}
	}
	private void Running() //달리기 모션 실행
	{
		isRun = true;
		anim.SetBool("Run_01", true);
		applySpeed = runSpeed;
	}
	private void RunningCancel() //달리기 모션 취소
	{
		isRun = false;
		anim.SetBool("Run_01", false);
		applySpeed = walkSpeed;
	}
	private void Move() //플레이어의 기본 이동 제어
	{
		float _moveDirX = Input.GetAxisRaw("Horizontal");
		float _moveDirZ = Input.GetAxisRaw("Vertical");
		Vector3 _moveHorizontal = transform.right * _moveDirX;
		Vector3 _moveVertical = transform.forward * _moveDirZ;
		Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
		myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
		if(_moveDirZ != 0)
		{
			anim.SetBool("Walk_01", true);
		}
	}
	private void CharacterRotation() //메인 카메라 좌,우 회전
	{
		float _yRotation = Input.GetAxisRaw("Mouse X");
		Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
		myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
	}
	private void CameraRotation() //메인 카메라 상,하 회전
	{
		float _xRotation = Input.GetAxisRaw("Mouse Y");
		float _cameraRotationX = _xRotation * lookSensitivity;
		currentCameraRotationX -= _cameraRotationX;
		currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
	}
}