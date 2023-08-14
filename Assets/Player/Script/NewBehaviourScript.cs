using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//�÷��̾� �⺻ �̵� ���� ����
	[SerializeField]
	private float walkSpeed; //�⺻ �̵� �ӵ�
	[SerializeField]
	private float runSpeed; //�޸��� �� �̵��ӵ�
	private float applySpeed; //�÷��̾�  ������Ʈ�� ���� �̵� �ӵ�
	[SerializeField]
	private float jumpForce; //���� �� �÷��̾��� ��� �ӵ�

	//�÷��̾��� ���� ���� ǥ��
	private bool isRun = false; //�޸��� ����
	private bool isGround = true; //���� ���� ����

	// �÷��̾� ������Ʈ ���� ���� ����
	private CapsuleCollider capsuleCollider;

	// ���콺 �ΰ���
	[SerializeField]
	private float lookSensitivity;

	// ī�޶� ȸ�� ���� ����
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
		TryJump(); //���� ��� ���� ���� ���� üũ
		TryRun(); //�޸��� ��� ���� ���� ���� üũ
		Move(); //�÷��̾��� �⺻ �̵� ����
		CameraRotation(); //���� ī�޶� ��,�� ȸ�� ����
		CharacterRotation(); //���� ī�޶� ��,�� ȸ�� ����

	}
	private void Tryattack() // ���� ���� ���� �Ǵ�
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
	private void TryJump() // ���� ��� ���� ���� ���� �Ǵ�
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			Jump();
		}
	}
	private void Jump() //���� ��� ����
	{
		anim.SetTrigger("Jump");
		myRigid.velocity = transform.up * jumpForce;
	}
	private void TryRun() //�޸��� ��� ���� ���� ���� �Ǵ�
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
	private void Running() //�޸��� ��� ����
	{
		isRun = true;
		anim.SetBool("Run_01", true);
		applySpeed = runSpeed;
	}
	private void RunningCancel() //�޸��� ��� ���
	{
		isRun = false;
		anim.SetBool("Run_01", false);
		applySpeed = walkSpeed;
	}
	private void Move() //�÷��̾��� �⺻ �̵� ����
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
	private void CharacterRotation() //���� ī�޶� ��,�� ȸ��
	{
		float _yRotation = Input.GetAxisRaw("Mouse X");
		Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
		myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
	}
	private void CameraRotation() //���� ī�޶� ��,�� ȸ��
	{
		float _xRotation = Input.GetAxisRaw("Mouse Y");
		float _cameraRotationX = _xRotation * lookSensitivity;
		currentCameraRotationX -= _cameraRotationX;
		currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
		theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
	}
}