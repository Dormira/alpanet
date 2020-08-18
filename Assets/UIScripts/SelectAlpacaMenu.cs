using UnityEngine;

public class SelectAlpacaMenu : MonoBehaviour
{
    public Texture[] portraits;

    void OnGUI()
    {
        if (SpawnManager.alpacaIndex.Count != portraits.Length)
        {
            refreshAlpacaPortraits();
        }
        for (int i = 0; i < SpawnManager.alpacaIndex.Count; i++)
        {
            if (GUI.Button(new Rect(50*i, 25, 50, 50), portraits[i])){
                Camera.main.GetComponent<FollowGameObject>().setTarget(i);
            }
        }
    }
    
    void refreshAlpacaPortraits()
    {
        //We don't need to do this anymore
        portraits = new Texture[SpawnManager.alpacaIndex.Count];
        for(int i = 0; i < SpawnManager.alpacaIndex.Count; i++)
        {
            string alpacaID = SpawnManager.alpacaIndex[i];
            Snapshot snapscript = GameObject.Find(alpacaID).GetComponent<Snapshot>();
            portraits[i] = snapscript.getSnapshot();
            
        }
    }

}
