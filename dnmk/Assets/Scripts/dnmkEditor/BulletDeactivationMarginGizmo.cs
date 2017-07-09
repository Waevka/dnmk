using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDeactivationMarginGizmo : MonoBehaviour {
    public DnmkPlayingField playingField;

    void OnDrawGizmos()
    {
        if (playingField != null)
        {
            Vector3 playingFieldCenter = playingField.transform.position;
            Vector3 topLeftCorner       = new Vector3(  playingFieldCenter.x + playingField.Width / 2.0f + playingField.BulletDeactivationMargin,
                                                        playingFieldCenter.y - playingField.Height / 2.0f - playingField.BulletDeactivationMargin, 0.0f);
            Vector3 topRightCorner      = new Vector3(  playingFieldCenter.x + playingField.Width / 2.0f + playingField.BulletDeactivationMargin, 
                                                        playingFieldCenter.y + playingField.Height / 2.0f + playingField.BulletDeactivationMargin, 0.0f);
            Vector3 bottomLeftCorner    = new Vector3(  playingFieldCenter.x - playingField.Width / 2.0f - playingField.BulletDeactivationMargin,
                                                        playingFieldCenter.y - playingField.Height / 2.0f - playingField.BulletDeactivationMargin, 0.0f);
            Vector3 bottomRightCorner   = new Vector3(  playingFieldCenter.x - playingField.Width / 2.0f - playingField.BulletDeactivationMargin,
                                                        playingFieldCenter.y + playingField.Height / 2.0f + playingField.BulletDeactivationMargin, 0.0f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(topLeftCorner, topRightCorner);
            Gizmos.DrawLine(topRightCorner, bottomRightCorner);
            Gizmos.DrawLine(bottomRightCorner, bottomLeftCorner);
            Gizmos.DrawLine(bottomLeftCorner, topLeftCorner);
        }
    }
}
