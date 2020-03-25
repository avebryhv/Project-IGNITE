using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingGun : EnemyBaseGun
{
    public override void Shoot()
    {
        GameObject currentBullet = Instantiate(baseBullet, transform.position, Quaternion.identity);
        float xDifference = behaviour.player.transform.position.x - transform.position.x;
        float yDifference = behaviour.player.transform.position.y - transform.position.y;
        Vector2 dir = new Vector2(xDifference, yDifference);
        dir.Normalize();
        currentBullet.GetComponent<EnemyBullet>().SetMovementDirection(dir);
        shootNow = false;
        Invoke("Cancel", cooldownAnimLength);

    }
}
