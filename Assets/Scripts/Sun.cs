using UnityEngine;

namespace Strungerhulder.Environment
{
    public class Sun : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            this.transform.RotateAround(this.transform.position, transform.right, Time.deltaTime);
        }
    }
}
