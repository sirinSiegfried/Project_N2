using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
	[SerializeField]
	private Transform CharacterBody;
	[SerializeField]
	private Transform CamaraArm;
	Animator animator;
	void Start()
	{
		animator = CharacterBody.GetComponent<Animator>();

	}
	void Update()
	{
		TryAttack();
		MoveWalk();
		LookAround();
	}
	private void TryAttack()
	{
		if (Input.GetButton("Fire1"))
		{
			int RandomAttack = Random.Range(1, 1234);
			RandomAttack = RandomAttack % 2;
			if(RandomAttack == 0)
			{
				animator.SetTrigger("Attack_01");
			}
			else if(RandomAttack == 1)
			{
				animator.SetTrigger("Attack_02");
			}
		}
		else if(Input.GetButton("Fire2"))
		{
			int RandomAttack = Random.Range(1, 1234);
			RandomAttack = RandomAttack % 2;
			if (RandomAttack == 0)
			{
				animator.SetTrigger("Attack_03");
			}
			else
			{
				animator.SetTrigger("Attack_04");
			}
		}
	}
	private void MoveWalk()
	{
		Vector2 MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		bool IsMove = MoveInput.magnitude != 0;
		animator.SetBool("Walk_01", IsMove);
		if (IsMove)
		{
			Vector3 lookForward = new Vector3(CamaraArm.forward.x, 0f, CamaraArm.forward.z).normalized;
			Vector3 lookRight = new Vector3(CamaraArm.forward.x, 0f, CamaraArm.forward.z).normalized;
			Vector3 moveDir = lookForward * MoveInput.y + lookRight * MoveInput.x;
			CharacterBody.forward = lookForward;
			transform.position += moveDir * Time.deltaTime * 1f;
		}
		Debug.DrawRay(CamaraArm.position, new Vector3(CamaraArm.forward.x, 0f, CamaraArm.forward.z).normalized, Color.red);
	}
	private void LookAround()
	{
		Vector2 mouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		Vector3 camAngle = CamaraArm.rotation.eulerAngles;
		float x = camAngle.x - mouseDelta.y;
		if (x < 180f)
		{
			x = Mathf.Clamp(x, -1f, 70f);
		}
		else
		{
			x = Mathf.Clamp(x, 335f, 361f);
		}
		CamaraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
	}
}
