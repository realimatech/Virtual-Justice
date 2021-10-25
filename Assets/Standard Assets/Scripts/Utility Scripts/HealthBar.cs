using UnityEngine;

public class HealthBar : MonoBehaviour {
	private Texture2D background;
    private Texture2D foreground;
	public Texture gui = null;
	
 
    public float health;
    public int maxHealth = 100;
	public int forca = 10;
	public int velocidade = 10;
	public int habilidade = 10;
	public int inteligencia = 10;
	public int dano = (10 * 10)/2;
	public int atqspd = (10 * 10)/6;
	

    void Start()
    {
		if(!gui){
			Debug.Log("Não tem Interface");
			return;
		}
 		health = maxHealth;
        background = new Texture2D(1, 1, TextureFormat.RGB24, false);
        foreground = new Texture2D(1, 1, TextureFormat.RGB24, false);

 
        background.SetPixel(0, 0, Color.red);
        foreground.SetPixel(0, 0, Color.green);
		
 
        background.Apply();
        foreground.Apply();

    }
 
    void Update()
    {
		
        if (health < 0){
			health = 0;
			if(gameObject)
			DestroyImmediate(gameObject);
		}
        if (health > maxHealth) {health = maxHealth;}
		
		
    }
 
    void OnGUI()
    {
		GUI.TextArea(new Rect(10,200,100,25), "Streng:"+forca);
		GUI.TextArea(new Rect(10,230,100,25), "Habilidade:"+habilidade);
		GUI.TextArea(new Rect(10,260,100,25), "Inteligencia:"+inteligencia);
		GUI.TextArea(new Rect(10,290,100,25), "Dano:"+dano);
		GUI.TextArea(new Rect(10,320,100,37), "Velocidade de Ataque:"+atqspd);
		
		
		if(Time.timeScale == 1.0){
		Rect box = new Rect(134.0f, 108.0f, 346.0f, 22.5f);
        GUI.BeginGroup(box);
        {
            GUI.DrawTexture(new Rect(0, 0, box.width, box.height), background, ScaleMode.StretchToFill,true);
            GUI.DrawTexture(new Rect(0, 0, box.width*health/maxHealth, box.height), foreground, ScaleMode.StretchToFill);
			
        }
        GUI.EndGroup(); ;
		GUI.DrawTexture(new Rect(10,10,256*2,64*2), gui);
		}
    }
}
