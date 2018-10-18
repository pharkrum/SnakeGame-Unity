using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyFood : Food {

    [SerializeField] AudioClip sound;

    public override void SetValues() {
        lifeTime = 10;
        value = 30;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        AudioSource.PlayClipAtPoint(sound, this.transform.position);
    }
}
