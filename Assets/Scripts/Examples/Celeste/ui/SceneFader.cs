using UnityEngine;

namespace Celeste.ui
{
	public class SceneFader : MonoBehaviour {
		Animator anim;
		Examples.Celeste.Player.Player player;

		void Start() {
			anim = GetComponent<Animator>();
			player = GameObject.Find("Player").GetComponent<Examples.Celeste.Player.Player>();
			player.DeathEvent += OnPlayerDeath;
		}

		void OnPlayerDeath(){
			anim.SetTrigger("Fade");
		}
	}
}
