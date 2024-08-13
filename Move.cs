using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Move : Agent
{
    public float speed = 1f;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material SuccessMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material NoramalMaterial;
    [SerializeField] private MeshRenderer floorMeshRender;
    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(3.42f, 2.035f, -7.509f);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float xInput = actions.ContinuousActions[0];
        float zInput = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(xInput, 0, zInput) * Time.deltaTime * speed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(+2f);
            floorMeshRender.material = SuccessMaterial;
            EndEpisode();
        }

        if (other.gameObject.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint");
            SetReward(+1f);
            floorMeshRender.material = winMaterial;
        }

        if (other.gameObject.CompareTag("Bad_Checkpoint"))
        {
            Debug.Log("Bad_Checkpoint");
            SetReward(-1f);
            floorMeshRender.material = loseMaterial;
        }

        if (other.TryGetComponent <Cube>(out Cube cube)) {
            SetReward(-1f);
            floorMeshRender.material = loseMaterial;
            EndEpisode();
        }
        if(other.gameObject.CompareTag("wall"))
        {
            Debug.Log("hit wall");
            SetReward(-1f);
            floorMeshRender.material = loseMaterial;
            EndEpisode();
        }
    }

}
