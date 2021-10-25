using UnityEngine;
using System.Threading;
using System.Collections;
using VJLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CameraInterface : MonoBehaviour
{

	public int width = 10, height = 10;
	private string[] selGrid = {  };
	private string[] urlGrid = {  };
	
	private int btnGrid = 0;
	private int btnFcs = 0;
	private int btnMds = 0;
	private int btnDfs = 0;
	private int btnClass = 0;
	
	public float vSbarValue = 1.0f;
	public int cont = 0;
	
	private int storeIndex = -1;
	private int storeIndexFcs = -1;
	private int storeIndexMds = -1;	
	private int storeIndexDfs = -1;
	private int storeIndexNclass = -1;
	private int[] storeTuto = new int[3];
	
	private string stringToEdit = "";
	private string stringTuto = "";
	private string tutoTitle = "";
	private string classpath = "C:\\Users\\Felipe\\Documents\\Desktop_2012\\WORKSTATION\\Nova pasta (11)\\Virtual Justice\\UserClasses\\";
	private string tutoPath = "C:\\Users\\Felipe\\Desktop\\WORKSTATION\\Nova pasta (11)\\Virtual Justice\\Tutos\\";
	private string className = "";
	private string errorMsg = "";
	private string superClass = "Nada";
	//private Thread thread;
	
	private bool lTuto = false;
	private bool lIDE = false;
	private bool lNewClass = false;
	
	void Start ()
	{
		UtilsIO.CLASSPATH = classpath;
	}

	void Awake ()
	{
		//thread = new Thread (exe);
	}

	void OnApplicationQuit ()
	{
		//thread.Abort ();
	}
	
	
	
	void pause(){
		if(!lIDE && !lTuto && !lNewClass){
			if (Time.timeScale == 1.0f) {
				Time.timeScale = 0.0f;
			} else {
				Time.timeScale = 1.0f;
			}	
		}
	}
	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Tab) && lTuto == false) {
			pause();
			lIDE = true;
		}
		
		if (Input.GetKeyDown (KeyCode.T) && lIDE == false) {
			pause();
			lTuto = true;
		}
		
		
	}
	
	void loadTutorial(){
		GUI.Box (new Rect (10, 10, Screen.width - 20, Screen.height - 20), "Tutorial");
		
			//DIREITA
			GUI.BeginGroup (new Rect (1000, 50, 340, Screen.height - 30 - (Screen.height * 2) / 25));
				// GRUPO 1
				GUI.BeginGroup(new Rect(0,0, 300, 150));
					string[] faceis = loadAllTutos("facil");
		            string[] fcNames = justTheName(faceis);
					btnFcs = GUI.SelectionGrid (new Rect (35, 20, 200, 100),btnFcs, fcNames, 1);			
					if(storeIndexFcs != btnFcs){
						storeIndexFcs = btnFcs;
						if(storeIndexFcs != -1){
							stringTuto = UtilsIO.load(faceis[storeIndexFcs]);
							tutoTitle = fcNames[storeIndexFcs];
							btnMds = -1;
							btnDfs = -1;
						}
					}
		
				GUI.EndGroup();
				// FIM GRUPO 1
		
				// GRUPO 2
				GUI.BeginGroup(new Rect(0,160, 300, 400));
					string[] medios = loadAllTutos("medio");
					string[] mdNames = justTheName(medios);
					btnMds = GUI.SelectionGrid (new Rect (35, 20, 200, 100), btnMds, mdNames, 1);
					if(storeIndexMds != btnMds ){
						storeIndexMds = btnMds;
						if(storeIndexMds != -1){
							stringTuto = UtilsIO.load(medios[storeIndexMds]);
							tutoTitle = mdNames[storeIndexMds];
							btnFcs = -1;
							btnDfs = -1;
						}
					}
				GUI.EndGroup();
				// FIM GRUPO 2
		
				// GRUPO 3
				GUI.BeginGroup(new Rect(0,310, 300, 400));
					string[] dificeis = loadAllTutos("dificil");
					string[] dfNames = justTheName(dificeis);
					btnDfs = GUI.SelectionGrid (new Rect (35, 20, 200, 33), btnDfs, dfNames, 1);
					if(storeIndexDfs != btnDfs){
						storeIndexDfs = btnDfs;
						if(storeIndexDfs != -1){
							stringTuto = UtilsIO.load(dificeis[storeIndexDfs]);
							tutoTitle = dfNames[storeIndexDfs];
							btnFcs = -1;
							btnMds = -1;
						}
					}
				GUI.EndGroup();
				// FIM GRUPO 3
				
			GUI.EndGroup ();
			//FIM DIREITA
			
			//ESQUERDA
			GUI.BeginGroup (new Rect (40, 50, Screen.width - 400, Screen.height - (Screen.height * 2) / 25));
				//TOPO
				GUI.BeginGroup (new Rect (3, 3, Screen.width - 410, 95));
					//tutoTitle = GUI.TextArea(new Rect (0, 0, Screen.width - 410, 95), tutoTitle);
					GUI.Label(new Rect (450, 0, Screen.width - 410, 95), tutoTitle);
				GUI.EndGroup();
				//FIM TOPO
				
				//CONTEUDO
				GUI.BeginGroup(new Rect (3, 100, Screen.width - 410, Screen.height - (Screen.height * 2) / 25));
					GUI.Box (new Rect (0, 0, Screen.width - 410, Screen.height - 130 - (Screen.height * 2) / 25),"");
					GUI.Label(new Rect (0, 0, Screen.width - 410, Screen.height - 130 - (Screen.height * 2) / 25), stringTuto);
				GUI.EndGroup();
				//FIM CONTEUDO
		
				if (GUI.Button (new Rect ((Screen.width * 2) / 6, Screen.height - (Screen.height * 2) / 25 - 80, Screen.width / 5, (Screen.height * 2) / 25), "Voltar ao Jogo")) {
					lTuto = false;
					pause();
				}
			GUI.EndGroup ();
			//FIM ESQUERDA	
	}
	
	string[] justTheName(string[] fullName){
		string[] n = new string[fullName.Count()];
		for(int i = 0; i < fullName.Count(); i++){
			string name = "";
			for(int j = fullName[i].Count() - 1; j >= 0;j--){
				if(fullName[i][j] != '\\'){
					name = fullName[i][j] + name;
				}else{
					break;	
				}
			}
			n[i] = name;
		}
		return n;
	}
	
	void loadIDE(){
		selGrid = loadAllClasses ();
		GUI.Box (new Rect (10, 10, Screen.width - 20, Screen.height - 20), "IDE");
		GUI.Box (new Rect (20, 30, Screen.width - 40, Screen.height - 50 - (Screen.height * 2) / 25), "");
		GUI.BeginGroup (new Rect (20, 30, Screen.width - 40, Screen.height - 50 - (Screen.height * 2) / 25));
			btnGrid = GUI.SelectionGrid (new Rect (10, 10, 100, 100), btnGrid, selGrid, 1);
			if (storeIndex != btnGrid) {
				if(storeIndex != -1){
					System.IO.File.WriteAllText(@urlGrid[storeIndex],stringToEdit);
				}
				storeIndex = btnGrid;
				stringToEdit = UtilsIO.load(urlGrid[btnGrid]);
			}
			stringToEdit = GUI.TextArea (new Rect (120, 10, Screen.width - 120 - 50, Screen.height - 70 - (Screen.height * 2) / 25), stringToEdit, 99999);
			
			if(GUI.Button(new Rect(10, 400, 100, 30), "Nova Classe")){
				lIDE = false;
				lNewClass = true; 
			}
		GUI.EndGroup ();
		
		if (GUI.Button (new Rect ((Screen.width * 2) / 7, Screen.height - (Screen.height * 2) / 25 - 15, Screen.width / 5, (Screen.height * 2) / 25), "Compilar")) {
			System.IO.File.WriteAllText(@urlGrid[btnGrid],stringToEdit);
			exe();
			lIDE = false;
			pause();
		}
		
		if (GUI.Button (new Rect ((Screen.width * 2) / 4, Screen.height - (Screen.height * 2) / 25 - 15, Screen.width / 5, (Screen.height * 2) / 25), "Cancelar")) {
			lIDE = false;
			pause();
		}
	}
	
	void OnGUI ()
	{
		if (lIDE) {
			loadIDE();
		}
		
		if(lTuto){
			loadTutorial();
		}
		
		if(lNewClass){
			loadNewClass();
		}
	}
	
	void loadNewClass(){
		GUI.Box (new Rect ((Screen.width/2) - 200, (Screen.height/2) - 150, 400, Screen.height/3), "Nova Classe");
		GUI.BeginGroup(new Rect ((Screen.width/2) - 200, (Screen.height/2) - 150, 400, Screen.height/3));
			GUI.Label(new Rect(35, 25, 100, 20), "Herda de:");
			string[] opt = {"Nada","Cubo"};
			btnClass = GUI.SelectionGrid(new Rect(35, 50, 350, 20),btnClass ,opt, 2);
			if(btnClass != storeIndexNclass){
				superClass = opt[btnClass];
				storeIndexNclass = btnClass;
			}
			GUI.Label(new Rect(35, 85, 100, 20), "Nome: ");
			GUI.Label(new Rect(35, 110, 300, 20), errorMsg);
			className = GUI.TextField(new Rect(75, 85, 270, 20), className);
		
			if(GUI.Button(new Rect(125, 140, 80, 50), "Criar")){
				string template = "";
				if(storeIndexNclass != 0){
					template = "classe "+className+" herda "+superClass+"{ \n\n}";
				}else{
					template = "classe "+className+"{\n\n}";
				}
				
				string urlArquivo = classpath+className+".txt";
				if (!System.IO.File.Exists(urlArquivo)){
					if(className != ""){
  						System.IO.File.Create(urlArquivo).Close();
						System.IO.TextWriter arquivo = System.IO.File.AppendText(urlArquivo);
						arquivo.WriteLine(template);
				    	arquivo.Close();
						lNewClass = false;
						lIDE = true;
					}else{
						errorMsg = "Preencha um nome para a classe";
					}
				}else{
					errorMsg = "O arquivo ja existe...";
				}
			}
				
			if(GUI.Button(new Rect(215, 140, 80, 50), "Cancelar")){
				lNewClass = false;
				lIDE = true;
			}
		
			
		GUI.EndGroup();
	}

	string[] loadAllClasses ()
	{
		Archive[] files = UtilsIO.getAllClassFiles ();
		int x = files.Count ();
		string[] nomes = new string[x];
		urlGrid = new string[x];
		for (int i = 0; i < x; i++) {
			nomes[i] = files[i].getName ();
			urlGrid[i] = files[i].getUrl();
		}
		return nomes;
	}
	
	string[] loadAllTutos (string path)
	{
		string pat = tutoPath+"\\"+path;
		
		DirectoryInfo diretorio = new DirectoryInfo(@pat);
		FileInfo[] arquivos = diretorio.GetFiles("*.*");
		string[] tuts = new string[arquivos.Count()];
		//Começamos a listar os arquivos
		for (int i = 0; i < tuts.Count(); i++)
		{
			tuts[i] = arquivos[i].FullName;
		}
		return tuts;
	}

	void exe ()
	{
		try {
			//UtilsIO.CLASSPATH = "C:\\VJLObjects\\HelloWorldVJL\\";
			new SemanticalAnalyzer ();
			VJObject t = new VJObject (ClassPathManager.getByName ("Principal"), new LinkedList<Adress> ());
			t.getMyMethods ().ElementAt (0).execute ();
			Adress e = t.getByIdentifier ("astra");
			VJObject ast = (VJObject)e.acessInCascade ();
			Adress ca = ast.getByIdentifier ("cub");
			VJObject cub = (VJObject)ca.acessInCascade ();
			
			HealthBar hb = (HealthBar)GameObject.Find("Astra").GetComponent("HealthBar");
			
			
			VJSimpleObject forc = (VJSimpleObject)ast.getByIdentifier ("forca").acessInCascade();
			int fc = Int16.Parse(forc.getValue());
			hb.forca = fc;
			
			VJSimpleObject habil = (VJSimpleObject)ast.getByIdentifier ("habilidade").acessInCascade();
			int ha = Int16.Parse(habil.getValue());
			hb.habilidade = ha;
			
			VJSimpleObject intel = (VJSimpleObject)ast.getByIdentifier ("inteligencia").acessInCascade();
			int it = Int16.Parse(intel.getValue());
			hb.inteligencia = it;
			
			VJSimpleObject cx = (VJSimpleObject)cub.getByIdentifier ("tamx").acessInCascade ();
			string x = cx.getValue ();
			VJSimpleObject cy = (VJSimpleObject)cub.getByIdentifier ("tamy").acessInCascade ();
			string y = cy.getValue ();
			VJSimpleObject cz = (VJSimpleObject)cub.getByIdentifier ("tamz").acessInCascade ();
			string z = cz.getValue ();
			
			GameObject astr = GameObject.Find ("Astra");
			GameObject cubo = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cubo.transform.position = new Vector3 (astr.transform.position.x, astr.transform.position.y + 10, astr.transform.position.z);
			float fx = (float)Double.Parse (x);
			float fy = (float)Double.Parse (y);
			float fz = (float)Double.Parse (z);
			cubo.transform.localScale = new Vector3 (fx, fy, fz);
			cubo.AddComponent <Rigidbody>();
			
			print (x);
		} catch (IOException e) {
			print ("e1");
		} catch (MemoryException e) {
			print ("e2");
		} catch (LexicalException e) {
			print ("e3");
		} catch (SintaxException e) {
			print ("e4");
		} catch (SemanticalException e) {
			print ("e5");
		} catch (Exception e) {
			print ("e6");
		}
	}
}
