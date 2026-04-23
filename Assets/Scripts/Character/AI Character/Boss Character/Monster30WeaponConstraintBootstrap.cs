using UnityEngine;
using UnityEngine.Animations;

public class Monster30WeaponConstraintBootstrap : MonoBehaviour
{
    void Awake()
    {
        foreach (var constraint in GetComponentsInChildren<ParentConstraint>(true))
        {
            if (constraint == null)
            {
                continue;
            }

            ApplyCurrentPoseAsConstraintOffset(constraint);
            constraint.constraintActive = true;
            constraint.locked = true;
        }
    }

    static void ApplyCurrentPoseAsConstraintOffset(ParentConstraint constraint)
    {
        var constrainedTransform = constraint.transform;

        for (int i = 0; i < constraint.sourceCount; i++)
        {
            ConstraintSource source = constraint.GetSource(i);

            if (source.sourceTransform == null)
            {
                continue;
            }

            Transform sourceTransform = source.sourceTransform;
            Vector3 localPositionOffset = sourceTransform.InverseTransformPoint(constrainedTransform.position);
            Quaternion localRotationOffset = Quaternion.Inverse(sourceTransform.rotation) * constrainedTransform.rotation;

            constraint.SetTranslationOffset(i, localPositionOffset);
            constraint.SetRotationOffset(i, localRotationOffset.eulerAngles);
        }
    }
}
