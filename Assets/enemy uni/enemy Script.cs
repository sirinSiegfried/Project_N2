using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{

	[SerializeField] private string animalName; // �� ���ָ�
	[SerializeField] private int hp; // ���� ü��

	[SerializeField] private float walkSpeed; // ��� ��� �ӵ�

	private Vector3 direction; // ����.
	private bool isAction;
	private bool isWalking; 
	[SerializeField] private float walkTime;
	[SerializeField] private float waitTime;
	private float currentTime;
	// �ʿ��� ������Ʈ
	[SerializeField] private Animator anim;
	[SerializeField] private Rigidbody rigid;
	[SerializeField] private BoxCollider boxCol;
	void Start()
	{
		currentTime = waitTime;
		isAction = true;
	}
	void Update()
	{
		Move();
		Rotation();
		ElapseTime();
	}

	private void Move()
	{
		if (isWalking)
			rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
	}

	private void Rotation()
	{
		if (isWalking)
		{
			Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);
			rigid.MoveRotation(Quaternion.Euler(_rotation));
		}
	}

	private void ElapseTime()
	{
		if (isAction)
		{
			currentTime -= Time.deltaTime;
			if (currentTime <= 0)
				ReSet();
		}
	}

	private void ReSet()
	{
		isWalking = false; isAction = true;
		anim.SetBool("Walking", isWalking);
		direction.Set(0f, Random.Range(0f, 360f), 0f);
		RandomAction();
	}

	private void RandomAction()
	{
		int _random = Random.Range(0, 4); // ���, ����, ����, �⺻ �̵�

		if (_random == 0)
			Wait();
		else if (_random == 1)
			Attack();
		else if (_random == 2)
			TryJump();
		else if (_random == 3)
			TryWalk();
	}
	private void Wait()//���
	{
		anim.SetBool("Walk", false);
	}
	private void Attack()//����
	{
		anim.SetBool("Walk", false);
		anim.SetTrigger("Attack");
	}
	private void TryJump()//����
	{
		anim.SetBool("Walk", false);
		anim.SetTrigger("Jump");
	}
	private void TryWalk()//�⺻ �̵�
	{
		anim.SetBool("Walk", true);
	}
}