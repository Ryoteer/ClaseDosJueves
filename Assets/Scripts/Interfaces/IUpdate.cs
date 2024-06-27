using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
    public void ArtificalUpdate();
    public void ArtificalFixedUpdate();
    public void ArtificalLateUpdate();
}
