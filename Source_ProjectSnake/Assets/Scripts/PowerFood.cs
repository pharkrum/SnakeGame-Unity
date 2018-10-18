using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFood : Food {

    [SerializeField] AudioClip sound;

    public override void SetValues() { //ve se n fica mehor como construtor
        lifeTime = 8;
        value = 40;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        AudioSource.PlayClipAtPoint(sound, this.transform.position);
    }
}
