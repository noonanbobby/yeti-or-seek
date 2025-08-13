using UnityEngine;

public class HideSeekGenerator : MonoBehaviour {
    public Vector2 areaSize = new Vector2(24f,24f);
    public int hideObjectCount = 12;
    public HideableObject[] hideObjects;

    public void Generate(){
        for(int i=transform.childCount-1;i>=0;i--) DestroyImmediate(transform.GetChild(i).gameObject);

        // Ground
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name="Ground"; ground.transform.SetParent(transform,false);
        ground.transform.localScale = new Vector3(areaSize.x/10f,1f,areaSize.y/10f);
        ground.GetComponent<Renderer>().material.color = Theme.Ground;

        // Sky/Light
        RenderSettings.ambientLight = Theme.Sky;
        if (FindAnyObjectByType<Light>()==null){
            var sun = new GameObject("Sun"); var l = sun.AddComponent<Light>();
            l.type=LightType.Directional; l.intensity=1.25f; sun.transform.rotation=Quaternion.Euler(50,30,0);
            l.color = new Color(1f,0.97f,0.92f);
        }

        // Walls
        CreateWall(new Vector3(0,1, areaSize.y*0.5f+0.5f), new Vector3(areaSize.x+2,2,1));
        CreateWall(new Vector3(0,1,-areaSize.y*0.5f-0.5f), new Vector3(areaSize.x+2,2,1));
        CreateWall(new Vector3( areaSize.x*0.5f+0.5f,1,0), new Vector3(1,2,areaSize.y+2));
        CreateWall(new Vector3(-areaSize.x*0.5f-0.5f,1,0), new Vector3(1,2,areaSize.y+2));

        // Hideables
        hideObjects = new HideableObject[hideObjectCount];
        for (int i=0;i<hideObjectCount;i++){
            var b = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            b.name=$"Hideable_{i}"; b.transform.SetParent(transform,false);
            float x = Random.Range(-areaSize.x*0.45f, areaSize.x*0.45f);
            float z = Random.Range(-areaSize.y*0.45f, areaSize.y*0.45f);
            b.transform.position = new Vector3(x,0.5f,z);
            b.transform.localScale=new Vector3(1.1f,1f,1.1f);
            var r = b.GetComponent<Renderer>(); r.material.color = (i%2==0)? Theme.Accent : Theme.Primary;
            hideObjects[i] = b.AddComponent<HideableObject>();
        }
    }

    void CreateWall(Vector3 center, Vector3 size){
        var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
        w.transform.SetParent(transform,false);
        w.transform.position=center; w.transform.localScale=size;
        w.GetComponent<Renderer>().material.color = new Color(0.85f,0.9f,1f);
    }
}
