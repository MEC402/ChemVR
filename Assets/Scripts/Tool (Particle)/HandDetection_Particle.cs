using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDetection_Particle : MonoBehaviour
{
    [SerializeField]
    private GameObject chemical;
    private Collider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(11, 12);
        Physics.IgnoreLayerCollision(11, 0);
        Physics.IgnoreLayerCollision(11, 9);

        capsuleCollider = this.GetComponent<Collider>();
        StartCoroutine(SetSettings(1f));
    }

    // Update is called once per frame
    private IEnumerator SetSettings(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        this.transform.localScale = new Vector3(1, 1, 1);
        capsuleCollider.enabled = true;
        StartCoroutine(SetSettings(1f));
    }
    void Update()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        capsuleCollider.enabled = true;
        if (this.transform.childCount > 50)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
    public void addChemContact(Vector3 pos)
    {
        GameObject chem = Instantiate(chemical, pos, new Quaternion());
        chem.transform.parent = this.transform;
    }
}
