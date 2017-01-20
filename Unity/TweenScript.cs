using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class TweenScript : MonoBehaviour {

	public Slider slider;
	public InputField inputField;
	public Transform wall;
	public Transform btn;
	public Transform cube;
	public Transform finger;

	private int duration = 8;
	private int tweenMode = -1;
	private Tweener tween;
	private Sequence[] seq = new Sequence[5];

	private Vector3 wallInitPos;
	private Vector3 btnInitScale;
	private Vector3 cubeInitPos;
	private Vector3 fingerInitPos;

	// Use this for initialization
	void Start () {
		wallInitPos = wall.localPosition;
		btnInitScale = btn.localScale;
		cubeInitPos = cube.localPosition;
		fingerInitPos = finger.localPosition;
	}

	public void onPlay(){

		if (Int32.TryParse (inputField.text, out duration));
		else
			duration = 3;

		print ("duration :" + duration);

		if (tweenMode == 0) {
			seq [2] = DOTween.Sequence ().PrependInterval (2.0f)
				.Insert (0.0f, finger.DOLocalMoveX (-0.9f, 2))
				.Append (DOTween.To (() => 220, x => slider.value = x, 255, 2.0f))
				.AppendInterval (2.0f)
				.OnComplete (() => {
				initObjPos ();
			});
		} else if (tweenMode == 1) {
			seq [0] = DOTween.Sequence ().PrependInterval (2.0f)
				.Insert (0.0f, finger.DOLocalMoveX (-0.75f, 2))
				.Append (DOTween.To (() => 120, x => slider.value = x, 255, 2.0f).SetEase (Ease.InOutQuint))
				.Insert (2.0f, btn.DOScaleX (0.1f, 2))
				.Insert (2.0f, finger.DOLocalMoveX (-0.95f, 2))
				.AppendInterval (2.0f)
				.OnComplete (() => {
				initObjPos ();
			});
		} else if (tweenMode == 2) {
			seq [1] = DOTween.Sequence ().PrependInterval (2.0f)
				.Insert (0.0f, finger.DOLocalMoveX (-0.49f, 2))
				.Append (DOTween.To (() => 200, x => slider.value = x, 255, 2.0f).OnComplete (() => {
				slider.value = 150;
			}))
				.Insert (4.0f, finger.DOLocalMoveX (-1.99f, 4).OnComplete (() => {
				initObjPos ();
			}))
				.Insert (4.0f, cube.DOLocalMoveX (-3, 4).OnComplete (() => {
				slider.value = 0;
				initObjPos ();
			}));
		} else if (tweenMode == 3) {
			seq [3] = DOTween.Sequence ().PrependInterval (1.0f)
				.Append (DOTween.To (() => 0, x => slider.value = x, 255, duration).SetLoops(10,LoopType.Yoyo));
		}
	}

	public void onPasue(){
		if (tweenMode == 0) {
			seq[2].Pause ();
		} else if (tweenMode == 1) {
			seq[0].Pause ();
		}else if (tweenMode == 2) {
			seq[1].Pause ();
		}else if (tweenMode == 3) {
			seq[3].Pause ();
		}
	}

	public void onRewind(){
		if (tweenMode == 0) {
			seq[2].Play ();
		} else if (tweenMode == 1) {
			seq[0].Play ();
		}else if (tweenMode == 2) {
			seq[1].Play ();
		}else if (tweenMode == 3) {
			seq[3].Play ();
		}
	}

	public void selectWallMode(){
		finger.gameObject.SetActive (true);
		cube.gameObject.SetActive (false);
		wall.gameObject.SetActive (true);
		btn.gameObject.SetActive (false);
		tweenMode = 0;
	}

	public void selectBtnMode(){
		finger.gameObject.SetActive (true);
		cube.gameObject.SetActive (false);
		wall.gameObject.SetActive (false);
		btn.gameObject.SetActive (true);
		tweenMode = 1;
	}

	public void selectCubeMode(){
		finger.gameObject.SetActive (true);
		cube.gameObject.SetActive (true);
		wall.gameObject.SetActive (false);
		btn.gameObject.SetActive (false);

		tweenMode = 2;
	}

	public void selectOthersMode(){
		cube.gameObject.SetActive (false);
		wall.gameObject.SetActive (false);
		btn.gameObject.SetActive (false);
		finger.gameObject.SetActive (false);

		tweenMode = 3;
	}

	private void initObjPos(){

		cube.localPosition = cubeInitPos;
		wall.localPosition = wallInitPos;
		btn.localScale = btnInitScale;
		finger.localPosition = fingerInitPos;
		slider.value = 0;

	}

	void OnDestory()
	{
		DOTween.KillAll ();
	}
}
