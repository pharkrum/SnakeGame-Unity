using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFood : Food {

    [SerializeField] AudioClip sound;

    public override void SetValues() {
        lifeTime = 12;
        value = -100;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        AudioSource.PlayClipAtPoint(sound, this.transform.position);
    }
}
