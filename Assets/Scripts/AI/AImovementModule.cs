using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotationCommand {
    public rotationSource rotSource;
    public Quaternion rotation;

    public RotationCommand (Quaternion rot, rotationSource level) {
        rotation = rot;
        rotSource = level;
    }
}
public class ForceCommand {
    public Vector3 dir;
    public ForceMode mode;
    public forceSource forceSource;

    public ForceCommand (Vector3 _dir, ForceMode mod, forceSource level) {
        dir = _dir;
        forceSource = level;
        mode = mod;
    }
}


public enum forceSource {
    BaseBehavior,
    Follow,
    Fear,
    Obstacles,
}
public enum rotationSource {
    BaseBehavior,
    Follow,
    Fear,
    Obstacles,
}

public class AImovementModule : MonoBehaviour, IaiModule {
    [SerializeField] private AnimalAISystem sys;


    public class BaseBehaviorConfig {
        public float targetAngle;
        public float time = -1;

        public void Generate (float mnTime, float currentAngle = 0) {
            time = Random.Range (mnTime, 2 * mnTime);
            targetAngle = currentAngle + Random.Range (-90, 90);
        }
    }


    public List<RotationCommand> rotCommands = new List<RotationCommand>();
    public List<ForceCommand> forceCommands = new List<ForceCommand> ();
    public BaseBehaviorConfig config = new BaseBehaviorConfig();
    public bool isStunned = false;

    public void MainHandler () {
        HandleBaseBehavior ();
        HandleMovement ();
        HandleCommands ();
    }

    private void HandleBaseBehavior () {
        if (config.time < 0) {
            config.Generate (2, sys.mainTransform.eulerAngles.y);
        }
        config.time -= Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler (0f, Mathf.MoveTowardsAngle (sys.mainTransform.eulerAngles.y, config.targetAngle, Time.fixedDeltaTime * sys.rotationSpeed), 0f);

        sys.SendRotation (targetRotation, rotationSource.BaseBehavior);
    }
    private void HandleCommands () {
        rotCommands.Sort ((x, y) => x.rotSource.CompareTo (y.rotSource));
        rotCommands.Reverse ();

        forceCommands.Sort ((x, y) => x.forceSource.CompareTo (y.forceSource));
        forceCommands.Reverse ();

   /*     Debug.Log ("--start--");
        foreach (var item in forceCommands) {
            Debug.Log (item.forceSource);
        }
        Debug.Log ("--end--");
   */

        if (!isStunned) {
            if (rotCommands.Count != 0) {
                sys.mainTransform.rotation = rotCommands[0].rotation;
            }
            if (forceCommands.Count != 0) {
                sys.mainRb.AddForce (forceCommands[0].dir, forceCommands[0].mode);
            }
        }

        rotCommands.Clear ();
        forceCommands.Clear ();
    }
    private void HandleMovement () {
        sys.SendForce (sys.mainTransform.forward * sys.maxSpeed * sys.movementMultiplier, ForceMode.Force, forceSource.BaseBehavior);

        if (sys.mainRb.velocity.magnitude > sys.maxSpeed) {
            sys.mainRb.velocity = Vector3.ClampMagnitude (sys.mainRb.velocity, sys.maxSpeed);
        }

        sys.mainAnimator.SetFloat ("speed", sys.mainRb.velocity.magnitude / sys.maxSpeed, 0.2f, Time.deltaTime);
    }
}
